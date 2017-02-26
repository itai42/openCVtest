using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using System.Diagnostics;
using Emgu.CV.Structure;

namespace Celiameter
{
  public partial class frmMain : Form
  {
    public uiMan _uiMan = new uiMan();
    public frmMain()
    {
      InitializeComponent();
    }
    /*  override mouse drag

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
          bool overrideMsg = false;
          if (pbMain != null)
          {
            // Listen for operating system messages.
            switch (m.Msg)
            {
              case (int)WM.MOUSEMOVE:
              case (int)WM.LBUTTONDOWN:
                if (m.HWnd == pbMain.Handle)
                {
                  overrideMsg = true;
                }
                break;
              default:
                break;
            }
          }
          if (!overrideMsg)
            base.WndProc(ref m);
        }*/
    private void newToolStripButton_Click(object sender, EventArgs e)
    {
      if (_activeSession.Modified)
      {
        var ync = MessageBox.Show("Save current session?", "Save session?", MessageBoxButtons.YesNoCancel);
        switch (ync)
        {
          case DialogResult.Cancel:
          default:
            return;
          case DialogResult.Yes:
            if (!_activeSession.Save())
            {
              return;
            }
            break;
          case DialogResult.No:
            break;
        }
      }
      OpenFileDialog folderBrowserDialog1 = new OpenFileDialog();
      folderBrowserDialog1.Multiselect = false;
      folderBrowserDialog1.CheckPathExists = true;
      folderBrowserDialog1.RestoreDirectory = true;
      Object oFolder = Application.UserAppDataRegistry.GetValue("browseFolder", System.IO.Path.GetDirectoryName(Application.ExecutablePath));
      folderBrowserDialog1.InitialDirectory = (String)oFolder;
      if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        bool isSessionFileLoaded = false;
        SessionMan newSession;
        String folder = Path.GetDirectoryName(folderBrowserDialog1.FileName);
        Application.UserAppDataRegistry.SetValue("browseFolder", folder);
        if (Path.GetExtension(folderBrowserDialog1.FileName) == SessionMan.SessionFileExt)
        {
          newSession = SessionMan.loadSession(folderBrowserDialog1.FileName);
          isSessionFileLoaded = (newSession != null);
        }
        else
        {
          String sessionFileName = folder + "\\" + Path.GetFileNameWithoutExtension(folderBrowserDialog1.FileName) + SessionMan.SessionFileExt;
          SessionMan.createNewSession(out newSession, sessionFileName, Path.GetExtension(folderBrowserDialog1.FileName));
        }
        if (newSession == null)
        {
          MessageBox.Show("Failed to load file:\r\n" + folderBrowserDialog1.FileName);
        }
        else
        {
          setActiveSession(ref newSession, isSessionFileLoaded);
        }
      }

    }
    CheckBox btnAutoload = new CheckBox();
    private void frmMain_Load(object sender, EventArgs e)
    {
      btnAutoload.Text = "Autoload last session";
      btnAutoload.CheckStateChanged += btnAutoload_CheckStateChanged;
      btnAutoload.BackColor = Color.Transparent;
      ToolStripControlHost host = new ToolStripControlHost(btnAutoload);
      host.Margin = new Padding(3, 0, 0, 1);
      btnAutoload.BackColor = Color.Transparent;
      host.BackColor = Color.Transparent;
      toolStrip1.Items.Insert(toolStrip1.Items.IndexOf(cbSeperator) + 1, host);
    }
    SessionMan _activeSession = new SessionMan();
    private void setActiveSession(ref SessionMan session, bool isSessionFileLoaded)
    {
      _activeSession = session;
      if (isSessionFileLoaded)
      {
        SetFPS(session._options.FPS);
        SetDiffOverlay(session._options.DrawingParams.ShowDiffOverlay);
      }
      else
      {
        GetFPS(ref session._options.FPS);
        GetDiffOverlay(out session._options.DrawingParams.ShowDiffOverlay);
      }
      _uiMan.PopulateSessionThumbs(ref lvThumbs, ref session);
      if (lvThumbs.Items.Count > 0)
      {
        lvThumbs.Items[0].Selected = true;
      }
    }

    private void GetFPS(ref double fps)
    {
      if (double.TryParse(cbSessioFPS.Text, out fps))
      {
        _activeSession._options.FPS = fps;
      }
      else
      {
        SetFPS(fps);
      }
    }
    private void SetFPS(double fps)
    {
      int sels = cbSessioFPS.SelectionStart;
      cbSessioFPS.Text = fps.ToString("##.###");
      cbSessioFPS.SelectionStart = sels;
      cbSessioFPS.SelectionLength = 0;
    }

    private void GetDiffOverlay(out bool drawDiffOverlay)
    {
      drawDiffOverlay = chkOverlayDiff.Checked;
    }
    private void SetDiffOverlay(bool drawDiffOverlay)
    {
      chkOverlayDiff.Checked = drawDiffOverlay;
    }

    bool _autoload = false;
    private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
    {
      Application.UserAppDataRegistry.SetValue("wndTop", Top);
      Application.UserAppDataRegistry.SetValue("wndLeft", Left);
      Application.UserAppDataRegistry.SetValue("wndW", Size.Width);
      Application.UserAppDataRegistry.SetValue("wndH", Size.Height);
      Application.UserAppDataRegistry.SetValue("leftSplitPos", splitLeft.SplitterDistance);
      Application.UserAppDataRegistry.SetValue("rightSplitPos", splitRight.SplitterDistance);
      Application.UserAppDataRegistry.SetValue("mainSplitPos", splitMain.SplitterDistance);
      Application.UserAppDataRegistry.SetValue("prevSplitPos", splitContorl.SplitterDistance);
      Application.UserAppDataRegistry.SetValue("autoload", _autoload.ToString());
      if (_activeSession != null && _activeSession._loaded)
      {
        Application.UserAppDataRegistry.SetValue("lastSessionFile", _activeSession._sessionFileName);
      }
    }

    private void frmMain_Shown(object sender, EventArgs e)
    {
      Object oTop = Application.UserAppDataRegistry.GetValue("wndTop", Top);
      Object oLeft = Application.UserAppDataRegistry.GetValue("wndLeft", Left);
      Object oW = Application.UserAppDataRegistry.GetValue("wndW", Size.Width);
      Object oH = Application.UserAppDataRegistry.GetValue("wndH", Size.Height);
      Object leftSplitPos = Application.UserAppDataRegistry.GetValue("leftSplitPos", splitLeft.SplitterDistance);
      Object rightSplitPos = Application.UserAppDataRegistry.GetValue("rightSplitPos", splitRight.SplitterDistance);
      Object mainSplitPos = Application.UserAppDataRegistry.GetValue("mainSplitPos", splitMain.SplitterDistance);
      Object splitContorlPos = Application.UserAppDataRegistry.GetValue("prevSplitPos", splitContorl.SplitterDistance);
      Object autoload = Application.UserAppDataRegistry.GetValue("autoload", _autoload.ToString());
      Object sessionFileName = Application.UserAppDataRegistry.GetValue("lastSessionFile", null);
      Width = (int)oW;
      Height = (int)oH;
      Left = (int)oLeft;
      Top = (int)oTop;
      splitLeft.SplitterDistance = (int)leftSplitPos;
      splitRight.SplitterDistance = (int)rightSplitPos;
      splitMain.SplitterDistance = (int)mainSplitPos;
      splitContorl.SplitterDistance = (int)splitContorlPos;
      _autoload = Boolean.Parse((string)autoload);
      if (_autoload && sessionFileName != null)
      {
        SessionMan session = SessionMan.loadSession((string)sessionFileName);
        if (session == null)
        {
          MessageBox.Show("session load failed.\r\nfile path: \"" + sessionFileName + "\"");
        }
        else
        {
          setActiveSession(ref session, true);
        }
      }
      btnAutoload.Checked = _autoload;
    }

    private void pbMain_MouseMove(object sender, MouseEventArgs e)
    {
      Point mp = new Point(e.X, e.Y);
      Point ip = new Point(e.X, e.Y);
      if (uiMan.pbToImgCoord(ref pbMain, e.X, e.Y, ref ip))
      {
        txtPointerCoords.Text = "Mouse: " + e.Location.ToString() + "  Image: " + ip.ToString();
        _uiMan.updateOverlayAndZoom(ref pbMain, ref pbZoom, mp, ip);

        switch (_uiMan._dragPhase)
        {
          default:
          case 0:
            break;
          case 1:
            txtDebugMsg.Text = "Selection: {" + _uiMan._p1.ToString() + ", " + ip.ToString() + ", ...: ";
            break;
          case 2:
            txtDebugMsg.Text = "Selection: {" + _uiMan._p1.ToString() + ", " + _uiMan._p2.ToString() + ", " + ip.ToString() + "}";
            break;
        }
      }

    }

    private void pbMain_MouseDown(object sender, MouseEventArgs e)
    {
      Point ip = new Point(e.X, e.Y);
      if (e.Button == System.Windows.Forms.MouseButtons.Middle && uiMan.pbToImgCoord(ref pbMain, e.X, e.Y, ref ip))
      {
        btnCacheAllFrames_Click(btnCacheAllFrames, new EventArgs());

        try
        {
          //_tmpCursor = Cursor.Current;
          //Cursor.Current = Cursors.WaitCursor;
          //Application.UseWaitCursor = true;
          List<double> vals = new List<double>(lvThumbs.Items.Count);
          foreach (ListViewItem i in lvThumbs.Items)
          {
            if (i.Tag == null)
            {
              continue;
            }
            SessionFrameTag itemTag = (SessionFrameTag)i.Tag;
            itemTag._frame.loadImage(ModifierKeys == Keys.Control);
            vals.Add(itemTag._frame._matOrig.ToImage<Gray, Byte>(false)[ip.Y, ip.X].Intensity);
          }
          _uiMan.ShowVals(vals, pbOutView);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
          throw;
        }
        finally
        {
          //Application.UseWaitCursor = false;
          //Cursor.Current = _tmpCursor;
        }
      }
      else if (e.Button == System.Windows.Forms.MouseButtons.Right)
      {
        if (_uiMan._dragPhase > 0)
        {
          _uiMan._dragPhase = 0;
          txtDebugMsg.Text = "";
        }
      }
      else if (e.Button == System.Windows.Forms.MouseButtons.Left && uiMan.pbToImgCoord(ref pbMain, e.X, e.Y, ref ip))
      {
        switch (_uiMan._dragPhase)
        {
          default:
            break;
          case 0:
            _uiMan._p1 = ip;
            _uiMan._dragPhase = 1;
            txtDebugMsg.Text = "Selection: {" + _uiMan._p1.ToString() + ", ...: ";
            break;
          case 1:
            _uiMan._p2 = ip;
            _uiMan._dragPhase = 2;
            txtDebugMsg.Text = "Selection: {" + _uiMan._p1.ToString() + ", " + _uiMan._p2.ToString() + ", ...: ";
            break;
          case 2:
            _uiMan._p3 = ip;
            _uiMan._dragPhase = 0;
            if (!_currItem.Equals(nullItemTag))
            {
              RoiItem roi = new RoiItem(_uiMan._p1, _uiMan._p2, _uiMan._p3);
              CvInvoke.Polylines(_uiMan._imgDisp, roi.Points, true, uiMan.RoisColor);
              CvInvoke.Polylines(_uiMan._img, roi.Points, true, uiMan.RoisColor);
              _currItem._frame._roiItems.Add(_currItem._frame._roiItems.Count.ToString("0##"), roi);
            }
            _uiMan.drawPreview(ref pbMain, ref pbOutView, _uiMan._p1, _uiMan._p2, _uiMan._p3);
            txtDebugMsg.Text = "Selection: {" + _uiMan._p1.ToString() + ", " + _uiMan._p2.ToString() + ", " + _uiMan._p3.ToString() + "}";
            break;
        }
      }

    }

    private void pbMain_MouseUp(object sender, MouseEventArgs e)
    {

    }

    private void pbMain_MouseClick(object sender, MouseEventArgs e)
    {

    }

    private void pbMain_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      /*
      if (pbMain.SizeMode == PictureBoxSizeMode.Normal)
      {
        pbMain.SizeMode = PictureBoxSizeMode.Zoom;
        pbMain.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
        pbMain.HorizontalScrollBar.Value = 0;
        pbMain.VerticalScrollBar.Value = 0;
      }
      else
      {
        pbMain.SizeMode = PictureBoxSizeMode.Normal;
        pbMain.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Everything;
      }
      */

    }

    private void btnSURF_Click(object sender, EventArgs e)
    {
      _uiMan.SURF(ref pbMain);
    }

    private void btnCanny_Click(object sender, EventArgs e)
    {
      _uiMan.drawCanny(ref pbMain);
    }

    private void lvThumbs_DrawItem(object sender, DrawListViewItemEventArgs e)
    {
      ListView lv = (ListView)sender;
      SessionFrameTag frm = (SessionFrameTag)e.Item.Tag;
      if (!frm._frame._imgLoaded)
      {
        frm._frame.loadImage();
      }
      if (!lv.LargeImageList.Images.ContainsKey(frm._key))
      {
        if (!frm._frame._imgLoaded)
        {
          lv.LargeImageList.Images.Add(frm._key, Image.FromFile(frm._frame._imageFilePath));
        }
        else
        {
          lv.LargeImageList.Images.Add(frm._key, frm._frame.GetThumbImage(lv.LargeImageList.ImageSize).Bitmap);
        }
      }
      e.DrawDefault = true;
    }

    static public SessionFrameTag nullItemTag = new SessionFrameTag();
    public SessionFrameTag _currItem = nullItemTag;
    public SessionFrameTag _playbackStartItem = new SessionFrameTag();
    public bool _isPlaying = false;
    public Object _playbackLock = new object();
    private void thumbStrip_playControlClicked(object sender, EventArgs e)
    {
      if (lvThumbs.Items.Count == 0)
      {
        return;
      }
      if (sender == pbcPlay || sender == pbcPlayPause)
      {
        lock (_playbackLock)
        {
          if (_isPlaying)
          {
            if (sender == pbcPlayPause)
            {
              _isPlaying = false;
            }
          }
          else
          {
            if (lvThumbs.SelectedItems.Count == 0)
            {
              lvThumbs.Items[0].Selected = true;
              lvThumbs.Items[0].EnsureVisible();
            }
            var selection = lvThumbs.SelectedItems[0];
            if ((sender == pbcPlayPause && !lvThumbs.Items.ContainsKey(_playbackStartItem._key)) || sender == pbcPlay)
            {
              _playbackStartItem = (SessionFrameTag)selection.Tag; //Only set pbStart if we're not using playpause or if playpause was used as first ever playback
            }
            _isPlaying = true;
          }
        }
      }
      else if (sender == pbcStop)
      {
        if (lvThumbs.SelectedItems.Count == 0)
        {
          lock (_playbackLock)
          {
            _isPlaying = false;
          }
        }
      }
      else if (sender == pbcFF)
      {
        lvThumbs.Items[lvThumbs.Items.Count - 1].Selected = true;
        lvThumbs.Items[lvThumbs.Items.Count - 1].EnsureVisible();
      }
      else if (sender == pbcFR)
      {
        lvThumbs.Items[0].Selected = true;
        lvThumbs.Items[0].EnsureVisible();
      }
    }

    private void lvThumbs_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (lvThumbs.Items.Count == 0)
      {
        pbcLabel.Text = "";
        tbCurImage.Text = "";
        _currItem = nullItemTag;
        return;
      }
      else if (lvThumbs.SelectedIndices.Count == 0)
      {
        _currItem = nullItemTag;
        pbcLabel.Text = lvThumbs.Items.Count.ToString("(##)");
        tbCurImage.Text = "";
        //var graphic = pbMain.CreateGraphics();
        //graphic.Clear(Color.White);
        return;
      }
      //else {
      pbcLabel.Text = lvThumbs.SelectedIndices[0].ToString() + "/" + lvThumbs.Items.Count.ToString();
      SessionFrameTag selectedItemTag = (SessionFrameTag)lvThumbs.SelectedItems[0].Tag;
      tbCurImage.Text = selectedItemTag._key;
      {
        var g = thumbToolStrip.CreateGraphics();
        tbCurImage.Size = new Size((int)g.MeasureString(tbCurImage.Text, tbCurImage.Font).Width, thumbToolStrip.Height - (thumbToolStrip.Padding.Vertical + thumbToolStrip.Margin.Vertical));
      }
      if (!selectedItemTag.Equals(_currItem))
      {
        if (_uiMan.SetCurrentFrame(pbMain, pbZoom, _activeSession, selectedItemTag._frame, _activeSession))
        {
          _currItem = selectedItemTag;
        }
      }
    }

    private void lvThumbs_ItemActivate(object sender, EventArgs e)
    {
      if (ModifierKeys == Keys.Control)
      {
        SessionFrameTag selectedItemTag = (SessionFrameTag)lvThumbs.SelectedItems[0].Tag;
        string args = string.Format("/e, /select, \"{0}\"", selectedItemTag._frame._imageFilePath);

        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = "explorer";
        info.Arguments = args;
        Process.Start(info);
      }
    }
    Cursor _tmpCursor = Cursors.Default;
    private void btnCacheAllFrames_Click(object sender, EventArgs e)
    {
      try
      {
        _tmpCursor = Cursor.Current;
        Cursor.Current = Cursors.WaitCursor;
        Application.UseWaitCursor = true;
        foreach (ListViewItem i in lvThumbs.Items)
        {
          if (i.Tag == null)
          {
            continue;
          }
          SessionFrameTag itemTag = (SessionFrameTag)i.Tag;
          itemTag._frame.loadImage(ModifierKeys == Keys.Control);
        }
      }
      catch (Exception)
      {
        throw;
      }
      finally
      {
        Application.UseWaitCursor = false;
        Cursor.Current = _tmpCursor;
      }
    }
    private void btnAutoload_CheckStateChanged(object sender, EventArgs e)
    {
      _autoload = btnAutoload.Checked;
    }

    private void saveToolStripButton_Click(object sender, EventArgs e)
    {
      _activeSession.saveSession(_activeSession._sessionFileName, true);
    }

    private void chkOverlayDiff_CheckedChanged(object sender, EventArgs e)
    {
      _uiMan.setOverlayDiff(pbMain, pbZoom, chkOverlayDiff.Checked, _activeSession);
    }

    private void cbSessioFPS_TextChanged(object sender, EventArgs e)
    {
      if (_activeSession == null || _activeSession._loaded == false)
      {
        return;
      }
      if (cbSessioFPS.Text.Trim().Length == 0)
      {
        return;
      }
      GetFPS(ref _activeSession._options.FPS);
    }
  }
}
