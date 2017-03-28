using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

using System.Diagnostics;

using Emgu;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.Structure;

using Emgu.CV.Util;
using Emgu.CV.Features2D;
using Emgu.CV.XFeatures2D;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Celiameter
{
  [XmlRoot("Options")]
  public class CMOptions
  {
    [XmlElement("DrawingParams")]
    public CMDrawingParameters DrawingParams = new CMDrawingParameters();
    [XmlElement("FPS")]
    public double FPS = 60.0;

    [XmlRoot("DrawingParams")]
    public class CMDrawingParameters
    {
      [XmlElement("ShowDiffOverlay")]
      public bool ShowDiffOverlay = true;
      [XmlElement("CorrectMotion")]
      public bool CorrectMotion = true;
      [XmlElement("ShowImage")]
      internal bool ShowImage = true;
      [XmlElement("DiffMethod")]
      public String DiffMethod = "Lucas - Kanade";
    }
  }

  public class uiMan
  {

    public Point _p1, _p2, _p3, _p4;
    public int _dragPhase;

    public Mat _img;
    public Mat _imgOrig;
    public Mat _imgDisp;
    public Mat _imgZoom;
    public Mat _ImgPreview;
    public uiMan()
    {
      _img = new Mat();
      _imgOrig = new Mat();
      _dragPhase = 0;
    }


    ~uiMan()
    {
    }

    static public bool imgToPbCoord(ref ImageBox pb, int ix, int iy, ref Point pbCoord) //True if coords are within limits, false if outside. when outside - the point as the nearest pb point to coords
    {
      if (pb == null || pbCoord == null || pb.Image == null)
        return false;

      double x = (double)ix;
      double y = (double)iy;
      double pic_hgt = (double)pb.ClientSize.Height;
      double pic_wid = (double)pb.ClientSize.Width;
      double img_hgt = (double)pb.Image.Size.Height;
      double img_wid = (double)pb.Image.Size.Width;
      double img_zoom = (double)pb.ZoomScale;

      int offsetX = (int)(x * img_zoom);
      int offsetY = (int)(y * img_zoom);
      int horizontalScrollBarValue = (pb.HorizontalScrollBar.Visible) ? (int)(pb.HorizontalScrollBar.Value) : 0;
      int verticalScrollBarValue = (pb.VerticalScrollBar.Visible) ? (int)(pb.VerticalScrollBar.Value) : 0;
      int X0 = offsetX - (int)((double)horizontalScrollBarValue * img_zoom);
      int Y0 = offsetY - (int)((double)verticalScrollBarValue * img_zoom);
      pbCoord.X = Math.Min(pb.ClientSize.Width - 1, Math.Max(0, X0));
      pbCoord.Y = Math.Min(pb.ClientSize.Height - 1, Math.Max(0, Y0));
      return (X0 >= 0 && X0 < pb.ClientSize.Width && Y0 >= 0 && Y0 < pb.ClientSize.Height);
    }

    static public bool pbToImgCoord(ref ImageBox pb, int ix, int iy, ref Point imgCoord) //True if coords are within limits, false if outside. when outside - the point as the nearest image point to coords
    {
      if (pb == null || imgCoord == null || pb.Image == null)
        return false;

      double x = (double)ix;
      double y = (double)iy;
      double pic_hgt = (double)pb.ClientSize.Height;
      double pic_wid = (double)pb.ClientSize.Width;
      double img_hgt = (double)pb.Image.Size.Height;
      double img_wid = (double)pb.Image.Size.Width;
      double img_zoom = (double)pb.ZoomScale;

      int offsetX = (int)(x / img_zoom);
      int offsetY = (int)(y / img_zoom);
      int horizontalScrollBarValue = (pb.HorizontalScrollBar.Visible) ? (int)(pb.HorizontalScrollBar.Value) : 0;
      int verticalScrollBarValue = (pb.VerticalScrollBar.Visible) ? (int)(pb.VerticalScrollBar.Value) : 0;
      int X0 = offsetX + horizontalScrollBarValue;
      int Y0 = offsetY + verticalScrollBarValue;
      imgCoord.X = Math.Min(pb.Image.Size.Width - 1, Math.Max(0, X0));
      imgCoord.Y = Math.Min(pb.Image.Size.Height - 1, Math.Max(0, Y0));
      return (X0 >= 0 && X0 < pb.Image.Size.Width && Y0 >= 0 && Y0 < pb.Image.Size.Height);
    }

    internal void PopulateSessionThumbs(ref ListView lvThumbs, ref SessionMan session)
    {
      lvThumbs.Clear();
      lvThumbs.Tag = session;
      int tileHeight = (int)((double)lvThumbs.ClientSize.Height * 0.8);
      lvThumbs.TileSize = new Size(tileHeight, tileHeight);
      ImageList lil = new ImageList();
      lil.ImageSize = new Size(tileHeight, tileHeight);
      lvThumbs.LargeImageList = lil;

      for (int i=0; i< session._activeFrames.Count; i++)
      {
        var frm = session._activeFrames[i];
        ListViewItem lvItem = new ListViewItem(frm._key, frm._key);
        lvItem.Tag = new SessionFrameTag(frm._key, frm._frame);
        lvThumbs.Items.Add(lvItem);
      }
    }

    bool _hasLastCursor = false;
    Point _lastCursor = new Point();
    public static readonly MCvScalar RoisColor = new MCvScalar(50,255,50);

    public void setZoomView(ImageBox pb, Point ipCursor, ImageBox pbZoom)
    {
      if (pb == null || pbZoom == null || _img == null)
        return;
      Size zoomSz = pbZoom.ClientSize;
      Point centerLeft = new Point(zoomSz.Width / 2, zoomSz.Height / 2);
      Point centerRight = new Point(zoomSz.Width - (centerLeft.X + 1), zoomSz.Height - (centerLeft.Y + 1));
      //int imX = Math.Min(pb.Image.Size.Width-1, Math.Max(0, ipCursor.X));
      //int imY = Math.Min(pb.Image.Size.Height-1, Math.Max(0, ipCursor.Y));
      //Rectangle roi = new Rectangle(new Point.Subtract(*ipCursor, center), zoomSz);
      int imW = _img.Size.Width;
      int imH = _img.Size.Height;
      int roiLeft = ipCursor.X - centerLeft.X;
      int roiRight = ipCursor.X + centerRight.X;
      int roiTop = ipCursor.Y - centerLeft.Y;
      int roiBottom = ipCursor.Y + centerRight.Y;
      int padLeft = 0;
      int padRight = 0;
      int padTop = 0;
      int padBottom = 0;
      if (roiLeft < 0)
      {
        padLeft = -roiLeft;
        roiLeft = 0;
      }
      if (roiRight >= imW)
      {
        padRight = 1 + (roiRight - imW);
        roiRight = imW - 1;
      }
      if (roiTop < 0)
      {
        padTop = -roiTop;
        roiTop = 0;
      }
      if (roiBottom >= imH)
      {
        padBottom = 1 + (roiBottom - imH);
        roiBottom = imH - 1;
      }
      int roiW = roiRight - roiLeft;
      int roiH = roiBottom - roiTop;
      Mat zoomCrop = new Mat(_img, new Rectangle(roiLeft, roiTop, roiW, roiH));
      if (_imgZoom == null || _imgZoom.Width != zoomSz.Width || _imgZoom.Height != zoomSz.Height)
      {
        _imgZoom = new Mat(zoomSz.Height, zoomSz.Width, _img.Depth, 3);
      }
      Rectangle zoomRoi = new Rectangle(padLeft, padTop, roiW, roiH);
      Mat zoomImgROI = new Mat(_imgZoom, zoomRoi);
      _imgZoom.SetTo(new MCvScalar(255, 255, 255), null);
      zoomCrop.CopyTo(zoomImgROI, null);
      int z = 6;
      CvInvoke.Line(_imgZoom, new Point(centerLeft.X - 25, centerLeft.Y - 1), new Point(centerLeft.X - z, centerLeft.Y - 1), new MCvScalar(55, 55, 255), 1, Emgu.CV.CvEnum.LineType.FourConnected, 0);
      CvInvoke.Line(_imgZoom, new Point(centerLeft.X + z, centerLeft.Y - 1), new Point(centerLeft.X + 25, centerLeft.Y - 1), new MCvScalar(55, 55, 255), 1, Emgu.CV.CvEnum.LineType.FourConnected, 0);
      CvInvoke.Line(_imgZoom, new Point(centerLeft.X - 25, centerLeft.Y + 1), new Point(centerLeft.X - z, centerLeft.Y + 1), new MCvScalar(55, 55, 255), 1, Emgu.CV.CvEnum.LineType.FourConnected, 0);
      CvInvoke.Line(_imgZoom, new Point(centerLeft.X + z, centerLeft.Y + 1), new Point(centerLeft.X + 25, centerLeft.Y + 1), new MCvScalar(55, 55, 255), 1, Emgu.CV.CvEnum.LineType.FourConnected, 0);
      CvInvoke.Line(_imgZoom, new Point(centerLeft.X - 25, centerLeft.Y), new Point(centerLeft.X + 25, centerLeft.Y), new MCvScalar(0, 0, 0), 1, Emgu.CV.CvEnum.LineType.FourConnected, 0);
      CvInvoke.Line(_imgZoom, new Point(centerLeft.X - 1, centerLeft.Y - 25), new Point(centerLeft.X - 1, centerLeft.Y - z), new MCvScalar(55, 55, 255), 1, Emgu.CV.CvEnum.LineType.FourConnected, 0);
      CvInvoke.Line(_imgZoom, new Point(centerLeft.X - 1, centerLeft.Y + z), new Point(centerLeft.X - 1, centerLeft.Y + 25), new MCvScalar(55, 55, 255), 1, Emgu.CV.CvEnum.LineType.FourConnected, 0);
      CvInvoke.Line(_imgZoom, new Point(centerLeft.X + 1, centerLeft.Y - 25), new Point(centerLeft.X + 1, centerLeft.Y - z), new MCvScalar(55, 55, 255), 1, Emgu.CV.CvEnum.LineType.FourConnected, 0);
      CvInvoke.Line(_imgZoom, new Point(centerLeft.X + 1, centerLeft.Y + z), new Point(centerLeft.X + 1, centerLeft.Y + 25), new MCvScalar(55, 55, 255), 1, Emgu.CV.CvEnum.LineType.FourConnected, 0);
      CvInvoke.Line(_imgZoom, new Point(centerLeft.X, centerLeft.Y - 25), new Point(centerLeft.X, centerLeft.Y + 25), new MCvScalar(0, 0, 0), 1, Emgu.CV.CvEnum.LineType.FourConnected, 0);
      pbZoom.Image = _imgZoom;
      _hasLastCursor = true;
      _lastCursor = ipCursor;
    }
    public MCvScalar SineLineCol = new MCvScalar(0, 255, 0);
    internal void ShowVals(List<double> vals, ImageBox pb)
    {
      
      double maxVal = vals.Max();
      double minVal = vals.Min();
      Mat dst = new Mat(new Size(Math.Max(pb.Width, 4 + vals.Count), Math.Max(pb.Height, (int)(maxVal-minVal))), DepthType.Cv8U, 3);
      double vPitch = ((double)(pb.Height - 4)) / (maxVal - minVal);
      dst.SetTo(new MCvScalar(255, 255, 255), null);
      for (int i=0; i<(vals.Count-1); i++)
      {

        Point a = new Point(2 + i, (int)(vPitch * (vals[i]- minVal)));
        Point b = new Point(3 + i, (int)(vPitch * (vals[i + 1]- minVal)));
        CvInvoke.Line(dst, a, b, SineLineCol);
      }
      pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
      pb.FunctionalMode = ImageBox.FunctionalModeOption.PanAndZoom;
      _ImgPreview = dst;
      pb.Image = _ImgPreview;
    }

    //     public void loadImageFile(ref ImageBox pb, String file)
    //     {
    //       _imgOrig = CvInvoke.Imread(file, Emgu.CV.CvEnum.ImreadModes.AnyColor);
    //       _imgDisp = new Mat(_imgOrig.Height, _imgOrig.Width, _imgOrig.Depth, 3);
    //       CvInvoke.CvtColor(_imgOrig, _imgDisp, Emgu.CV.CvEnum.ColorConversion.BayerGr2Rgb, 3);
    //       _img = _imgDisp.Clone();
    //       pb.Image = _img;
    //       //setPBImage(pb, img);
    //     }

    public void updateOverlayAndZoom(ref ImageBox pbMain, ref ImageBox pbZoom, Point canvasPos, Point imagePos)
    {
      //Mat buff = _imgDisp.Clone();
      _imgDisp.CopyTo(_img);
      switch (_dragPhase)
      {
        default:
        case 0:

          break;
        case 1:
          drawRoiRect(ref _img, _p1, imagePos, new MCvScalar(0, 0, 255), -1);
          break;
        case 2:
          drawRoiRect(ref _img, _p1, _p2, imagePos, new MCvScalar(0, 0, 255), -1);
          break;
      }
      //_img = buff;
      pbMain.Image = _img;
      setZoomView(pbMain, imagePos, pbZoom);
    }

    static public void drawRoiRect(ref Mat img, Point p1, Point p2, MCvScalar color, int z)
    {
      Point A = new Point();
      Point B = new Point();
      Point C = new Point();
      Point D = new Point();
      PointF rectCenter = new PointF();
      SizeF rectSz = new SizeF();
      double rectAngle = 0.0;
      calcRectPoints(p1, p2, p2, ref A, ref B, ref C, ref D, ref rectCenter, ref rectSz, ref rectAngle);
      CvInvoke.Line(img, A, B, color, 1, Emgu.CV.CvEnum.LineType.EightConnected, 0);


      CvInvoke.PutText(img, "A", A, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      CvInvoke.PutText(img, "B", B, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      Point MidAB = new Point(-5 + (A.X + B.X) / 2, -12 + (A.Y + B.Y) / 2);
      CvInvoke.PutText(img, rectSz.Width.ToString("0.0"), MidAB, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      CvInvoke.Circle(img, Point.Round(rectCenter), 2, color, 2, LineType.EightConnected, 0);
      CvInvoke.PutText(img, "p1", p1, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, true);
      CvInvoke.PutText(img, "p2", p2, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, true);
      CvInvoke.PutText(img, "angle = " + rectAngle.ToString("0.0"), new Point(p1.X + 5, p1.Y + 12), Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);    
    }
    static public void drawRoiRect(ref Mat img, Point p1, Point p2, Point p3, MCvScalar color, int z)
    {
      Point A = new Point();
      Point B = new Point();
      Point C = new Point();
      Point D = new Point();
      PointF rectCenter = new PointF();
      SizeF rectSz = new SizeF();
      double rectAngle = 0.0;
      calcRectPoints(p1, p2, p3, ref A, ref B, ref C, ref D, ref rectCenter, ref rectSz, ref rectAngle);
      CvInvoke.Line(img, A, B, color, 1, Emgu.CV.CvEnum.LineType.EightConnected, 0);
      CvInvoke.Line(img, B, C, color, 1, Emgu.CV.CvEnum.LineType.EightConnected, 0);
      CvInvoke.Line(img, C, D, color, 1, Emgu.CV.CvEnum.LineType.EightConnected, 0);
      CvInvoke.Line(img, D, A, color, 1, Emgu.CV.CvEnum.LineType.EightConnected, 0);

      
      CvInvoke.PutText(img, "A", A, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      CvInvoke.PutText(img, "B", B, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      CvInvoke.PutText(img, "C", C, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      CvInvoke.PutText(img, "D", D, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      Point MidAB = new Point(-5 + (A.X + B.X) / 2, -12 + (A.Y + B.Y) / 2);
      Point MidAD = new Point(-22 + (A.X + D.X) / 2, 5 + (A.Y + D.Y) / 2);
      CvInvoke.PutText(img, rectSz.Width.ToString("0.0"), MidAB, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      CvInvoke.PutText(img, rectSz.Height.ToString("0.0"), MidAD, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      CvInvoke.Circle(img, Point.Round(rectCenter), 4, color, 2, LineType.EightConnected, 0);
      CvInvoke.PutText(img, "p1", p1, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, true);
      CvInvoke.PutText(img, "p2", p2, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, true);
      CvInvoke.PutText(img, "angle = " + rectAngle.ToString("0.0"), new Point(p1.X+5, p1.Y+12), Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      //         CvInvoke.PutText(img, "p3", *p3, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      //       
    }

    internal bool SetCurrentFrame(ImageBox pbMain, ImageBox pbZoom, SessionMan session, SessionFrame frame, object lockObject)
    {
      if (session == null || session._loaded == false)
      {
        return false;
      }
      if (frame == null)
      {
        return false;
      }
      lock (lockObject)
      {
        session._currentFrame = frame;
         _imgOrig = frame.loadImage();
        CvInvoke.GaussianBlur(_imgOrig, _imgOrig, new Size(3, 3), 1.6);
        if (_imgDisp == null
            || _imgDisp.Height != _imgOrig.Height
            || _imgDisp.Width != _imgOrig.Width
            || _imgDisp.Depth != _imgOrig.Depth)
        {
          _imgDisp = new Mat(_imgOrig.Height, _imgOrig.Width, _imgOrig.Depth, 3);
        }

        //imgDisp is RGB conversion of imgOrig
        CvInvoke.CvtColor(_imgOrig, _imgDisp, Emgu.CV.CvEnum.ColorConversion.Gray2Rgb, 3);

        //Draw everything that needs drawing

        //Multi frame drawings
        SessionFrame diffframe = null;
        int idx = SessionFrameTag.indexInList(ref session._activeFrames, session._currentFrame.Key);
        bool correctMotion = false;
        PointF globalMotion = new PointF(0f, 0f);
        if ((session._options.DrawingParams.ShowDiffOverlay || session._options.DrawingParams.CorrectMotion) && session._activeFrames.Count > 0)
        {
          if (idx == 0)
          {
            diffframe = session._activeFrames[1]._frame;
          }
          else
          {
            diffframe = session._activeFrames[idx-1]._frame;
          }

          //Diff overlay
          Mat frmImgMat;// = frame.loadImage();
          Mat diffFrmMat;// = diffframe.loadImage();
          if (idx == 0)
          {
            frmImgMat = frame.loadImage();
            diffFrmMat = diffframe.loadImage();
          }
          else
          {
            frmImgMat = diffframe.loadImage();
            diffFrmMat = frame.loadImage(); 
          }

          //OpticalFlow
          int xgrid = 120;
          int ygrid = 120;

          List<PointF> ptsDiff = new List<PointF>();
          PointF[] pts = new PointF[xgrid * ygrid];
          PointF[] ptsO = new PointF[xgrid * ygrid];
          byte[] ptsStatus = new byte[xgrid * ygrid];
          SizeF gridWinSize = new SizeF((float)frmImgMat.Width/ (float)xgrid, (float)frmImgMat.Height / (float)ygrid);
          Size procWinSize = new Size(51, 51);
          for (int x = 0; x < xgrid; x++)
          {
            for (int y = 0; y < ygrid; y++)
            {
              int n = x + y * xgrid;
              pts[n] = new PointF((float)x * gridWinSize.Width, (float)y * gridWinSize.Height);
              ptsO[n] = new PointF((float)x * gridWinSize.Width, (float)y * gridWinSize.Height);
              ptsStatus[n] = 1;
            }
          }
          VectorOfPointF ptsIn = new VectorOfPointF(pts);
          VectorOfPointF ptsOut = new VectorOfPointF(ptsO);
          bool drawFlowArrows = false;
          if (CudaInvoke.HasCuda)
          {
            CudaSparsePyrLKOpticalFlow of = new CudaSparsePyrLKOpticalFlow(procWinSize);
            of.Calc(frmImgMat, diffFrmMat, ptsIn, ptsOut);
            drawFlowArrows = session._options.DrawingParams.ShowDiffOverlay;
            correctMotion = session._options.DrawingParams.CorrectMotion;
          }
          else
          {
            if (session._options.DrawingParams.DiffMethod == "Farneback")
            {
//             var lengths = ptsDiff.Select(p => (float)Math.Sqrt(Math.Pow((double)p.X, 2.0) + Math.Pow((double)p.Y, 2.0)));
//             DenseHistogram HLen = new DenseHistogram(lengths.Count(), new RangeF(lengths.Min(), lengths.Max()));
//             var angles = ptsDiff.Select(p => (float)Math.Atan((double)p.Y/(double)p.X));
//             DenseHistogram HAngle = new DenseHistogram(angles.Count(), new RangeF(angles.Min(), angles.Max()));
              Image<Gray, float> flowX = new Image<Gray, float>(frmImgMat.Size);
              Image<Gray, float> flowY = new Image<Gray, float>(frmImgMat.Size);
              CvInvoke.CalcOpticalFlowFarneback(frmImgMat.ToImage<Gray,byte>(), diffFrmMat.ToImage<Gray, byte>(), flowX, flowY, 0.5, 1, 16, 16, 7, 1.5, OpticalflowFarnebackFlag.FarnebackGaussian);
              for (int x = 0; x < xgrid; x++)
              {
                for (int y = 0; y < ygrid; y++)
                {
                  int n = x + y * xgrid;
                  ptsStatus[n] = 1;
                  float dx = (float)flowX[(int)((float)x * gridWinSize.Width), (int)((float)y * gridWinSize.Height)].Intensity;
                  float dy = (float)flowY[(int)((float)x * gridWinSize.Width), (int)((float)y * gridWinSize.Height)].Intensity;
                  ptsDiff.Add(new PointF(dx,dy));
                  ptsO[n] = new PointF(pts[n].X +dx, pts[n].Y + dy);
                }
              }
              ptsOut = new VectorOfPointF(ptsO);
            }
            else //if (diffMethod == "Lucas - Kanade")  / default
            { 
              VectorOfByte ptsStatusOut = new VectorOfByte(xgrid * ygrid);
              Mat ptsErrOut = new Mat();
              MCvTermCriteria tc = new MCvTermCriteria(32, 0.001);
              Mat outMat = new Mat();
              CvInvoke.CalcOpticalFlowPyrLK(frmImgMat, diffFrmMat, ptsIn, ptsOut, ptsStatusOut, ptsErrOut, procWinSize, 1, tc);
              for (int x = 0; x < xgrid; x++)
              {
                for (int y = 0; y < ygrid; y++)
                {
                  int n = x + y * xgrid;
                  ptsStatus[n] = ptsStatusOut[n];
                  float dx = (float)ptsOut[n].X - (float)ptsIn[n].X;
                  float dy = (float)ptsOut[n].Y - (float)ptsIn[n].Y;
                  ptsDiff.Add(new PointF(dx, dy));
                }
              }
            }
            
            drawFlowArrows = session._options.DrawingParams.ShowDiffOverlay;
            correctMotion = session._options.DrawingParams.CorrectMotion;
          }

          var lengthsNangles = ptsDiff.Select((p, indx) => new {
            Indx = indx,
            status = ptsStatus[indx],
            X = p.X,
            Y = p.Y,
            length = (float)Math.Sqrt(Math.Pow((double)p.X, 2.0) + Math.Pow((double)p.Y, 2.0)),
            angle = (float)Math.Atan2((double)p.Y, (double)p.X) }).Where(l => (l.length >= 1)).Where(i => i.status == 1);
          float lengthAvg = 0;
          float angleAvg = 0;
          try
          {
//             SortedList<int, int> lengthHistogram = new SortedList<int, int>();
//             foreach (var i in lengthsNangles)
//             {
//               if (i.status == 1)
//               {
//                 int key = (int)Math.Round(i.length);
//                 int val = (lengthHistogram.ContainsKey(key) ? lengthHistogram[key] + 1 : 1);
//                 lengthHistogram[key] = val;
//               }
//             }
//             double lengthsTotal = (double)lengthHistogram.Values.Sum();
//             int acc = 0;
//             for (int i=0; i< lengthHistogram.Count; i++)
//             {
//               acc += lengthHistogram.ElementAt(i).Value;
//               lengthHistogram[i] = acc;
//             }
//             var percentileByLen = lengthHistogram.Select(i => new KeyValuePair<int, double>(i.Key, (double)i.Value / lengthsTotal)).ToDictionary(k => k.Key, v => v.Value);
            var filteredLNA = lengthsNangles;//.Where(u => Inside(percentileByLen[(int)Math.Round(u.length)], 0.25, 0.75));
            lengthAvg = filteredLNA.Average(i => i.length);
            var unitXAvg = filteredLNA.Where(i => (i.length != 0f)).Average(i => i.X / i.length);
            var unitYAvg = filteredLNA.Where(i => (i.length != 0f)).Average(i => i.Y / i.length);
            angleAvg = (float)Math.Atan2((double)unitYAvg, (double)unitXAvg);
          }
          catch
          {
            drawFlowArrows = false;
            correctMotion = false;
          }
          PointF center = new PointF((float)_imgDisp.Width / 2f, (float)_imgDisp.Height / 2f);
          Arrow a = new Arrow(lengthAvg, angleAvg, new PointF(center.X, center.Y));
          globalMotion = new PointF(a.dX, a.dY);
          if (drawFlowArrows)
          {
            if (!session._options.DrawingParams.ShowImage)
            {
              _imgDisp.SetTo(new MCvScalar(255, 255, 255), null);
            }

            for (int x = 0; x < xgrid; x++)
            {
              for (int y = 0; y < ygrid; y++)
              {
                int n = x + y * xgrid;
                if (ptsStatus[n] == 0)
                {
                  CvInvoke.Circle(_imgDisp, Point.Round(ptsIn[n]), 3, new MCvScalar(128, 128, 255));
                }
                else
                {
                  PointF ptOut;
                  if (session._options.DrawingParams.CorrectMotion)
                  {
                    ptOut = new PointF(ptsOut[n].X - globalMotion.X, ptsOut[n].Y - globalMotion.Y);
                  }
                  else
                  {
                    ptOut = ptsOut[n];
                  }
                  CvInvoke.ArrowedLine(_imgDisp, Point.Round(ptsIn[n]), Point.Round(ptOut), new MCvScalar(0, 0, 255), 1, LineType.EightConnected, 0, 0.3);
                }
              }
            }
            //draw center arrow
            a._length *= 2;
            CvInvoke.ArrowedLine(_imgDisp, Point.Round(a.Origin), Point.Round(a.Tip), new MCvScalar(0, 255, 0), 1, LineType.EightConnected, 0, 0.3);
          }



          //AbsDiff
          //double Threshold = 25.0; //stores threshold for thread access
          //Mat DifferenceMat = new Mat(frmImgMat.Size, frmImgMat.Depth, frmImgMat.NumberOfChannels);
          //CvInvoke.AbsDiff(frmImgMat, diffFrmMat, DifferenceMat); //find the absolute difference 
          //var diffMask = DifferenceMat.ToImage<Rgb, byte>().ThresholdBinary(new Rgb(Threshold, Threshold, Threshold), new Rgb(0, 255, 0)); //if value > threshold set to 255, 0 otherwise 
          //DifferenceMat = diffMask.Mat;
          //CvInvoke.AddWeighted(_imgDisp, 1.0, DifferenceMat, 0.05, 0.0, _imgDisp);
        }
        //ROIs
        foreach (var roi in frame._roiItems)
        {
          CvInvoke.Polylines(_imgDisp, roi.Value.Points, true, RoisColor);
        }


        //we clone/copy because we'll draw roi creation lines over imgDisp and will want it to avoid repeating color conversion
        bool wasCloned = false;
        if (_img == null
            || _imgDisp.Height != _img.Height
            || _imgDisp.Width != _img.Width
            || _imgDisp.NumberOfChannels != _img.NumberOfChannels
            || _imgDisp.Depth != _img.Depth)
        {
           _img = _imgDisp.Clone();
          wasCloned = true;
        }
        if (correctMotion)
        {
          int Sleft = (int)Math.Max(globalMotion.X, 0);
          int Stop = (int)Math.Max(globalMotion.Y, 0);
          int Dleft = (int)Math.Max(-globalMotion.X, 0);
          int Dtop = (int)Math.Max(-globalMotion.Y, 0);
          int W = _imgDisp.Width - (int)Math.Abs(globalMotion.X);
          int H = _imgDisp.Height - (int)Math.Abs(globalMotion.Y);
          Rectangle SRect = new Rectangle(Sleft, Stop, W, H);
          Rectangle DRect = new Rectangle(Dleft, Dtop, W, H);
          Mat Simg = new Mat(_imgDisp, SRect);
          Mat Dimg = new Mat(_img, DRect);
          Simg.CopyTo(Dimg);
        }
        else if(!wasCloned)
        {
          _imgDisp.CopyTo(_img);
        }

        pbMain.Image = _img;
        if (_hasLastCursor)
        {
          setZoomView(pbMain, _lastCursor, pbZoom);
        }
        //setPBImage(pb, img);
      }
      return true;
    }

    public static bool Inside<T>(T x, T min, T max)
    {
      return ((Comparer<T>.Default.Compare(x, min) >= 0) && (Comparer<T>.Default.Compare(x, max) <= 0));
    }

    public void drawPreview(ref ImageBox pbMain, ref ImageBox pbOutView, Point p1, Point p2, Point p3)
    {
      Point A = new Point();
      Point B = new Point();
      Point C = new Point();
      Point D = new Point();
      PointF rectCenter = new PointF();
      SizeF rectSz = new SizeF();
      double rectRotation = 0.0;
      calcRectPoints(p1, p2, p3, ref A, ref B, ref C, ref D, ref rectCenter, ref rectSz, ref rectRotation);
      Mat src = _imgOrig;
      Mat dst = new Mat(pbOutView.Size, src.Depth, src.NumberOfChannels);
      //PointF rectCenter = new PointF((((float)(A.X + B.X + C.X + D.X)) / 4.0f), (((float)(A.Y + B.Y + C.Y + D.Y)) / 4.0f));
      //SizeF rectSz = new SizeF((float)Math.Sqrt(Math.Pow(B.X - A.X, 2) + Math.Pow(B.Y - A.Y, 2)), (float)Math.Sqrt(Math.Pow(D.X - A.X, 2) + Math.Pow(D.Y - A.Y, 2)));
      //double alpha = -Math.Atan(((double)(B.Y - A.Y)) / ((double)(B.X - A.X))) * 180.0 / Math.PI;
      Mat rotMat = new Mat();
      RotatedRect recCrop = new RotatedRect(rectCenter, rectSz, (float)rectRotation);

      double angle = rectRotation * Math.PI / 180.0;
      Point Ar = rotatePoint(A, rectCenter, angle);
      //Point Br = rotatePoint(B, rectCenter, angle);
      Point Cr = rotatePoint(C, rectCenter, angle); ;
      //Point Dr = rotatePoint(D, rectCenter, angle);
      CvInvoke.GetRotationMatrix2D(rectCenter, -rectRotation, 1, rotMat);
      CvInvoke.WarpAffine(src, dst, rotMat, src.Size, Emgu.CV.CvEnum.Inter.Cubic, Emgu.CV.CvEnum.Warp.Default, Emgu.CV.CvEnum.BorderType.Wrap, new MCvScalar(255, 255, 255));
      /*
        new MCvScalar color = new MCvScalar(255, 0, 0);
      CvInvoke.Line(dst, Ar, Br, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, 0);
      CvInvoke.Line(dst, Br, Cr, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, 0);
      CvInvoke.Line(dst, Cr, Dr, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, 0);
      CvInvoke.Line(dst, Dr, Ar, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, 0);
      CvInvoke.PutText(dst, "A'", Ar, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      CvInvoke.PutText(dst, "B'", Br, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      CvInvoke.PutText(dst, "C'", Cr, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
      CvInvoke.PutText(dst, "D'", Dr, Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, color, 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
    */
      Point topLeft = new Point(Math.Min(Cr.X, Ar.X), Math.Min(Cr.Y, Ar.Y));
      Size Sz = new Size(Math.Abs(Cr.X - Ar.X), Math.Abs(Cr.Y - Ar.Y));
      pbOutView.Image = _ImgPreview = new Mat(dst, new Rectangle(Ar, Sz));
      pbOutView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      pbOutView.FunctionalMode = ImageBox.FunctionalModeOption.Minimum;
    }

    internal void setOverlayDiff(ImageBox pbMain, ImageBox pbZoom, bool showDiffOverlay, SessionMan session)
    {
      if (session == null || session._loaded == false)
      {
        return;
      }
      session._options.DrawingParams.ShowDiffOverlay = showDiffOverlay;
      SetCurrentFrame(pbMain, pbZoom, session, session._currentFrame, session);
    }
    internal void setCorrectMotion(ImageBox pbMain, ImageBox pbZoom, bool correctMotion, SessionMan session)
    {
      if (session == null || session._loaded == false)
      {
        return;
      }
      session._options.DrawingParams.CorrectMotion = correctMotion;
      SetCurrentFrame(pbMain, pbZoom, session, session._currentFrame, session);
    }
    internal void setShowImage(ImageBox pbMain, ImageBox pbZoom, bool showImage, SessionMan session)
    {
      if (session == null || session._loaded == false)
      {
        return;
      }
      session._options.DrawingParams.ShowImage = showImage;
      SetCurrentFrame(pbMain, pbZoom, session, session._currentFrame, session);
    }

    internal void setDiffMethod(ImageBox pbMain, ImageBox pbZoom, String diffMethod, SessionMan session)
    {
      if (session == null || session._loaded == false)
      {
        return;
      }
      session._options.DrawingParams.DiffMethod = diffMethod;
      SetCurrentFrame(pbMain, pbZoom, session, session._currentFrame, session);
    }

    public void setPBImage(ref ImageBox pb, ref Mat img)
    {
      //Graphics graphics = pb.CreateGraphics();
      //System.IntPtr ptr(img.Ptr);
      //Bitmap b = new Bitmap(img.Cols, img.Rows, img.Step, Imaging.PixelFormat.Format24bppRgb, ptr);
      //RectangleF rect(0, 0, pb.Width, pb.Height);
      //pb.FunctionalMode = ImageBox.FunctionalModeOption.Everything;
      //Image<Bgr, unsigned char> rstImg = new Image<Bgr, unsigned char>(*b);
      //.Clone();

      //graphics.DrawImage(b, rect);
    }


    static public Point rotatePoint(Point P, PointF center, double alpha)
    {
      Point rc = new Point();
      rc.X = (int)((((double)P.X - center.X) * Math.Cos(alpha)) - (((double)P.Y - center.Y) * Math.Sin(alpha)) + center.X);
      rc.Y = (int)((((double)P.X - center.X) * Math.Sin(alpha)) + (((double)P.Y - center.Y) * Math.Cos(alpha)) + center.Y);
      return rc;
    }

    // 
    //     public void FindMatchSURF(ref Mat modelImage, ref Mat observedImage/*, long matchTime*/, ref VectorOfKeyPoint modelKeyPoints, ref VectorOfKeyPoint observedKeyPoints, ref VectorOfVectorOfDMatch matches, ref Mat mask, ref Mat homography)
    //     {
    // 
    //       int k = 2;
    //       double uniquenessThreshold = 0.8;
    //       double hessianThresh = 300;
    // 
    //       CudaSURF surfCuda = new CudaSURF((float)hessianThresh, 4, 2, true, (float)0.01, false);
    //       GpuMat gpuModelImage = new GpuMat(modelImage);
    // 
    //       GpuMat gpuModelKeyPoints = surfCuda.DetectKeyPointsRaw(gpuModelImage, null);
    //       GpuMat gpuModelDescriptors = surfCuda.ComputeDescriptorsRaw(gpuModelImage, null, gpuModelKeyPoints);
    //       CudaBFMatcher matcher = new CudaBFMatcher(DistanceType.L2);
    // 
    //       surfCuda.DownloadKeypoints(gpuModelKeyPoints, modelKeyPoints);
    //       //Stopwatch watch = Stopwatch.StartNew();
    // 
    //       // extract features from the observed image
    //       GpuMat gpuObservedImage = new GpuMat(observedImage);
    //       GpuMat gpuObservedKeyPoints = surfCuda.DetectKeyPointsRaw(gpuObservedImage, null);
    //       GpuMat gpuObservedDescriptors = surfCuda.ComputeDescriptorsRaw(gpuObservedImage, null, gpuObservedKeyPoints);
    //       //GpuMat tmp = new GpuMat())
    //       //Stream stream = new Stream())
    //       matcher.KnnMatch(gpuObservedDescriptors, gpuModelDescriptors, matches, k, null, false);
    // 
    //       surfCuda.DownloadKeypoints(gpuObservedKeyPoints, observedKeyPoints);
    // 
    //       mask = new Mat(matches.Size, 1, Emgu.CV.CvEnum.DepthType.Cv8U, 1);
    //       mask.SetTo(new MCvScalar(255), null);
    //       Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);
    // 
    //       int nonZeroCount = CvInvoke.CountNonZero(mask);
    //       if (nonZeroCount >= 4)
    //       {
    //         nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,
    //             matches, mask, 1.5, 20);
    //         if (nonZeroCount >= 4)
    //           homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, matches, mask, 2);
    //       }
    //       //watch.Stop();
    //       //matchTime = watch.ElapsedMilliseconds;
    //       /*
    //       //Extract SURF points by initializing parameters
    //       Emgu.CV.CvSURFParams params = CvInvoke.cvSURFParams(500, 1);
    //       cvExtractSURF(image, 0, &imageKeypoints, &imageDescriptors, storage, params);
    //       printf("Image Descriptors: %d\n", imageDescriptors.total);
    // 
    //       //draw the keypoints on the captured frame
    //       for (i = 0; i < imageKeypoints.total; i++)
    //       {
    //       CvSURFPoint* r = (CvSURFPoint*)cvGetSeqElem(imageKeypoints, i);
    //       CvPoint center;
    //       int radius;
    //       center.x = cvRound(r.pt.x);
    //       center.y = cvRound(r.pt.y);
    //       radius = cvRound(r.size*1.2 / 9. * 2);
    //       cvCircle(frame, center, radius, red_color[0], 1, 8, 0);
    //       }
    //       */
    //     }
    // 
    //     public Mat DrawSURF(ref Mat modelImage, ref Mat observedImage)//, long matchTime)
    //     {
    //       Mat mask = new Mat();
    //       Mat homography = new Mat();
    //       VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();
    //       VectorOfKeyPoint modelKeyPoints = new VectorOfKeyPoint();
    //       VectorOfKeyPoint observedKeyPoints = new VectorOfKeyPoint();
    //       FindMatchSURF(ref modelImage, ref observedImage, /*matchTime, */ref modelKeyPoints, ref observedKeyPoints, ref matches, ref mask, ref homography);
    // 
    //       //Draw the matched keypoints
    //       Mat result = new Mat();
    //       Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
    //           matches, result, new MCvScalar(255, 255, 255), new MCvScalar(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.Default);
    // 
    //       //region draw the projected region on the image
    // 
    //       if (homography != null)
    //       {/*
    //     //draw a rectangle along the projected model
    //     Rectangle rect = new Rectangle(new Point(0, 0), modelImage.Size);
    //     array<int> ptci = new array<int>(4);
    //     array < PointF ,1 > ptcv = new array<PointF,1 >(4);
    //     CV.Util.VectorOfPointF ptsT = new CV.Util.VectorOfPointF();
    //     ptsT.Push(&PointF(rect.Left, rect.Bottom));
    //     ptsT.Push(new PointF(rect.Right, rect.Bottom));
    //     ptsT.Push(new PointF(rect.Right, rect.Top));
    //     ptsT.Push(new PointF(rect.Left, rect.Top));
    //     CV.Util.VectorOfPointF pts = new CV.Util.VectorOfPointF();
    //     CvInvoke.PerspectiveTransform(pts, ptsT, homography);
    // 
    //     CV.Util.VectorOfPoint points = Array.ConvertAll < PointF, new Point> (ptsT, new System.Converter<PointF, new Point> (new Point.Round));
    //     VectorOfPoint vp = new VectorOfPoint(new Points))
    // 		CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255), 5);*/
    //       }
    //       return result;
    //     }
    //     
    public static void FindMatchSURF(Mat modelImage, Mat observedImage, out long matchTime, out VectorOfKeyPoint modelKeyPoints, out VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out Mat homography)
    {
      int k = 2;
      double uniquenessThreshold = 0.8;
      double hessianThresh = 300;

      Stopwatch watch;
      homography = null;

      modelKeyPoints = new VectorOfKeyPoint();
      observedKeyPoints = new VectorOfKeyPoint();

      if (CudaInvoke.HasCuda)
      {
        CudaSURF surfCuda = new CudaSURF((float)hessianThresh);
        using (GpuMat gpuModelImage = new GpuMat(modelImage))
        //extract features from the object image
        using (GpuMat gpuModelKeyPoints = surfCuda.DetectKeyPointsRaw(gpuModelImage, null))
        using (GpuMat gpuModelDescriptors = surfCuda.ComputeDescriptorsRaw(gpuModelImage, null, gpuModelKeyPoints))
        using (CudaBFMatcher matcher = new CudaBFMatcher(DistanceType.L2))
        {
          surfCuda.DownloadKeypoints(gpuModelKeyPoints, modelKeyPoints);
          watch = Stopwatch.StartNew();

          // extract features from the observed image
          using (GpuMat gpuObservedImage = new GpuMat(observedImage))
          using (GpuMat gpuObservedKeyPoints = surfCuda.DetectKeyPointsRaw(gpuObservedImage, null))
          using (GpuMat gpuObservedDescriptors = surfCuda.ComputeDescriptorsRaw(gpuObservedImage, null, gpuObservedKeyPoints))
          //using (GpuMat tmp = new GpuMat())
          //using (Stream stream = new Stream())
          {
            matcher.KnnMatch(gpuObservedDescriptors, gpuModelDescriptors, matches, k);

            surfCuda.DownloadKeypoints(gpuObservedKeyPoints, observedKeyPoints);

            mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(255));
            Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);

            int nonZeroCount = CvInvoke.CountNonZero(mask);
            if (nonZeroCount >= 4)
            {
              nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,
                 matches, mask, 1.5, 20);
              if (nonZeroCount >= 4)
                homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints,
                   observedKeyPoints, matches, mask, 2);
            }
          }
          watch.Stop();
        }
      }
      else
      {
        using (UMat uModelImage = modelImage.GetUMat(AccessType.Read))
        using (UMat uObservedImage = observedImage.GetUMat(AccessType.Read))
        {
          SURF surfCPU = new SURF(hessianThresh);
          //SIFT surfCPU = new SIFT();
          //extract features from the object image
          UMat modelDescriptors = new UMat();
          surfCPU.DetectAndCompute(uModelImage, null, modelKeyPoints, modelDescriptors, false);

          watch = Stopwatch.StartNew();

          // extract features from the observed image
          UMat observedDescriptors = new UMat();
          surfCPU.DetectAndCompute(uObservedImage, null, observedKeyPoints, observedDescriptors, false);
          BFMatcher matcher = new BFMatcher(DistanceType.L2);
          matcher.Add(modelDescriptors);

          matcher.KnnMatch(observedDescriptors, matches, k, null);
          mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
          mask.SetTo(new MCvScalar(255));
          Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);

          int nonZeroCount = CvInvoke.CountNonZero(mask);
          if (nonZeroCount >= 4)
          {
            nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,
               matches, mask, 1.5, 20);
            if (nonZeroCount >= 4)
              homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints,
                 observedKeyPoints, matches, mask, 2);
          }

          watch.Stop();
        }
      }
      matchTime = watch.ElapsedMilliseconds;
    }

    /// <summary>
    /// Draw the model image and observed image, the matched features and homography projection.
    /// </summary>
    /// <param name="modelImage">The model image</param>
    /// <param name="observedImage">The observed image</param>
    /// <param name="matchTime">The output total time for computing the homography matrix.</param>
    /// <returns>The model image and observed image, the matched features and homography projection.</returns>
    public static Mat DrawSURF(ref Mat modelImage, ref Mat observedImage, out long matchTime)
    {
      Mat homography;
      VectorOfKeyPoint modelKeyPoints;
      VectorOfKeyPoint observedKeyPoints;
      using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
      {
        Mat mask;
        FindMatchSURF(modelImage, observedImage, out matchTime, out modelKeyPoints, out observedKeyPoints, matches,
           out mask, out homography);

        //Draw the matched keypoints
        Mat result = new Mat();
        Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
           matches, result, new MCvScalar(255, 255, 255), new MCvScalar(255, 255, 255), mask);

        #region draw the projected region on the image

        if (homography != null)
        {
          //draw a rectangle along the projected model
          Rectangle rect = new Rectangle(Point.Empty, modelImage.Size);
          PointF[] pts = new PointF[]
          {
                  new PointF(rect.Left, rect.Bottom),
                  new PointF(rect.Right, rect.Bottom),
                  new PointF(rect.Right, rect.Top),
                  new PointF(rect.Left, rect.Top)
          };
          pts = CvInvoke.PerspectiveTransform(pts, homography);

          Point[] points = Array.ConvertAll<PointF, Point>(pts, Point.Round);
          using (VectorOfPoint vp = new VectorOfPoint(points))
          {
            CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255), 5);
          }

        }

        #endregion

        return result;

      }
    }
    public void SURF(ref ImageBox pb)
    {
      long matchTime = 0;
      _imgDisp = DrawSURF(ref _ImgPreview, ref _imgDisp, out matchTime);
      pb.Image = _imgDisp;
    }
    public void drawCanny(ref ImageBox pb)
    {
      CvInvoke.Canny(_imgOrig, _imgDisp, 50, 50, 3, true);
      pb.Image = _imgDisp;
    }

    static public void calcRectPoints(Point p1, Point p2, Point p3, ref Point TL, ref Point TR, ref Point BR, ref Point BL, ref PointF rectCenter, ref SizeF rectSz, ref double rectRotation)
    {
      double dx1, dx2, dx3, dx4, dx5, dy1, dy2, dy3, dy4, dy5;
      double alpha, beta;
      dx1 = p1.X;
      dx2 = p2.X;
      dx3 = p3.X;
      dy1 = p1.Y;
      dy2 = p2.Y;
      dy3 = p3.Y;
      alpha = (dy2 - dy1) / (dx2 - dx1);
      beta = -1 / alpha;
      Point A = new Point();
      Point B = new Point();
      Point C = new Point();
      Point D = new Point();
      A.X = p1.X;
      A.Y = p1.Y;
      B.X = p2.X;
      B.Y = p2.Y;
      if (p1.X == p2.X)
      {
        C.X = p3.X;
        C.Y = p2.Y;
        D.X = p3.X;
        D.Y = p1.Y;
      }
      else if (p1.Y == p2.Y)
      {
        C.X = p2.X;
        C.Y = p3.Y;
        D.X = p1.X;
        D.Y = p3.Y;
      }
      else
      {
        double b, c, d;
        b = dy3 - alpha * dx3;
        c = dy1 - beta * dx1;
        d = dy2 - beta * dx2;
        dx4 = (c - b) / (alpha - beta);
        dy4 = c + beta * dx4;
        dx5 = (d - b) / (alpha - beta);
        dy5 = d + beta * dx5;
        C.X = (int)dx5;
        C.Y = (int)dy5;
        D.X = (int)dx4;
        D.Y = (int)dy4;
      }

      bool orientationFromImage = false;
      if (orientationFromImage)
      {

        ///now we want to set ABCD where A is top-left and going clockwise
        List<Point> ll = new List<Point>();
        ll.Add(A);
        ll.Add(B);
        ll.Add(C);
        ll.Add(D);
        int PtAIdx = ll.Select((item, indx) => new { Item = item, Index = indx }).OrderByDescending(x => x.Item.X).ThenBy(x => x.Item.Y).Select(x => x.Index).First();
        int nextIdx = (PtAIdx < 3) ? PtAIdx + 1 : 0;
        int prevIdx = (PtAIdx > 0) ? PtAIdx - 1 : 3;
        int farIdx = (nextIdx < 3) ? nextIdx + 1 : 0;
        if (ll[nextIdx].Y <= ll[prevIdx].Y)
        {
          int tmp = prevIdx;
          prevIdx = nextIdx;
          nextIdx = tmp;
        }
        TL = ll[farIdx];
        TR = ll[prevIdx];
        BR = ll[PtAIdx];
        BL = ll[nextIdx];
        rectRotation = -Math.Atan(((double)(TR.Y - TL.Y)) / ((double)(TR.X - TL.X))) * 180.0 / Math.PI;
      }
      else //base is set and initial 2 points and 3rd point determines direction
      {
        if (p1.X == p2.X)
        {
          int top = Math.Min(p1.Y, p2.Y);
          int bottom = Math.Max(p1.Y, p2.Y);
          if (p3.X > p1.X)
          {
            TL.X = p3.X;
            TL.Y = top;
            TR.X = p3.X;
            TR.Y = bottom;
            BR.X = p1.X;
            BR.Y = bottom;
            BL.X = p1.X;
            BL.Y = top;
            rectRotation = 270;
          }
          else
          {
            TL.X = p3.X;
            TL.Y = bottom;
            TR.X = p3.X;
            TR.Y = top;
            BR.X = p1.X;
            BR.Y = top;
            BL.X = p1.X;
            BL.Y = bottom;
            rectRotation = 90;
          }
        }
        else if (p1.Y == p2.Y)
        {
          int left = Math.Min(p1.X, p2.X);
          int right = Math.Max(p1.X, p2.X);
          if (p3.Y > p1.Y)
          {
            TL.X = right;
            TL.Y = p3.Y;
            TR.X = left;
            TR.Y = p3.Y;
            BR.X = left;
            BR.Y = p1.Y;
            BL.X = right;
            BL.Y = p1.Y;
            rectRotation = 180;
          }
          else
          {
            TL.X = left;
            TL.Y = p3.Y;
            TR.X = right;
            TR.Y = p3.Y;
            BR.X = right;
            BR.Y = p1.Y;
            BL.X = left;
            BL.Y = p1.Y;
            rectRotation = 0;
          }
        }
        else
        {
          double abLineSlope;
          if (p1.X < p2.X)
          {
            abLineSlope = (dy2 - dy1) / (dx2 - dx1);
          }
          else
          {
            abLineSlope = (dy1 - dy2) / (dx1 - dx2);
          }
          double abLineYAtCX = dy1 + ((double)C.X - dx1) * abLineSlope;
          bool isP3AboveP12Line = (abLineYAtCX < (double)C.Y);

          int top12 = Math.Max(p1.Y, p2.Y);
          int bottom12 = Math.Min(p1.Y, p2.Y);
          int left12 = Math.Min(p1.X, p2.X);
          int right12 = Math.Max(p1.X, p2.X);

          int top45 = Math.Max(C.Y, D.Y);
          int bottom45 = Math.Min(C.Y, D.Y);
          int left45 = Math.Min(C.X, D.X);
          int right45 = Math.Max(C.X, D.X);

          if (abLineSlope > 0)
          {
            if (isP3AboveP12Line)
            {
              TL.X = right45;
              TL.Y = top45;
              TR.X = left45;
              TR.Y = bottom45;
              BL.X = right12;
              BL.Y = top12;
              BR.X = left12;
              BR.Y = bottom12;
              rectRotation = 180 + (Math.Atan(-abLineSlope) * 180.0 / Math.PI);
            }
            else
            {
              TL.X = left45;
              TL.Y = bottom45;
              TR.X = right45;
              TR.Y = top45;
              BL.X = left12;
              BL.Y = bottom12;
              BR.X = right12;
              BR.Y = top12;
              rectRotation = (Math.Atan(-abLineSlope) * 180.0 / Math.PI);
            }
          }
          else
          {
            if (isP3AboveP12Line)
            {
              TL.X = right45;
              TL.Y = bottom45;
              TR.X = left45;
              TR.Y = top45;
              BL.X = right12;
              BL.Y = bottom12;
              BR.X = left12;
              BR.Y = top12;
              rectRotation = 180 - (Math.Atan(abLineSlope) * 180.0 / Math.PI);
            }
            else
            {
              TL.X = left45;
              TL.Y = top45;
              TR.X = right45;
              TR.Y = bottom45;
              BL.X = left12;
              BL.Y = top12;
              BR.X = right12;
              BR.Y = bottom12;
              rectRotation = -(Math.Atan(abLineSlope) * 180.0 / Math.PI);
            }
          }
        }
      }
      rectCenter = new PointF((((float)(TL.X + TR.X + BR.X + BL.X)) / 4.0f), (((float)(TL.Y + TR.Y + BR.Y + BL.Y)) / 4.0f));
      rectSz = new SizeF((float)Math.Sqrt(Math.Pow(TR.X - TL.X, 2) + Math.Pow(TR.Y - TL.Y, 2)), (float)Math.Sqrt(Math.Pow(BL.X - TL.X, 2) + Math.Pow(BL.Y - TL.Y, 2)));

    }

  }

  public class Arrow
  {
    public float _angle;
    public float _length;
    public PointF _origin;

    public Arrow(float length, float angle, PointF origin)
    {
      _length = length;
      _angle = angle;
      _origin = origin;
    }
    public PointF Origin
    {
      get
      {
        return _origin;
      }
      set
      {
        _origin = value;
      }
    }
    public PointF Tip
    {
      get
      {
        return new PointF(dX + _origin.X, dY + _origin.Y);
      }
      set
      {
        float dx = value.X - _origin.X;
        float dy = value.Y - _origin.Y;
        _angle = (float)Math.Atan2(dy, dx);
        _length = (float)Math.Sqrt(dx * dx + dy * dy);
      }
    }
    public float dX
    {
      get
      {
        return _length * (float)Math.Cos(_angle);
      }
    }
    public float dY
    {
      get
      {
        return _length * (float)Math.Sin(_angle);
      }
    }
  }
}
