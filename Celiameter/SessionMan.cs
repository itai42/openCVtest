
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Windows.Forms;
using Emgu.CV.Structure;

namespace Celiameter
{
  [Serializable]
  public class RoiItem : IXmlSerializable
  {
    public Rectangle _boundingRect = new Rectangle();
    public Point TL = new Point();
    public Point TR = new Point();
    public Point BL = new Point();
    public Point BR = new Point();
    public SizeF _rectSz = new SizeF();
    public PointF _rectCenter = new PointF();
    public double _rectAngle = 0.0;

    public Mat _mat = null;
    Point[] _points = new Point[4];

    public Point[] Points
    {
      get
      {
        _points[0] = TL;
        _points[1] = TR;
        _points[2] = BR;
        _points[3] = BL;
        return _points;
      }
    }

    public RoiItem(RoiItem src)
    {
    }
    public RoiItem()
    {
    }

    public RoiItem(Point p1, Point p2, Point p3)
    {
      uiMan.calcRectPoints(p1, p2, p3, ref TL, ref TR, ref BR, ref BL, ref _rectCenter, ref _rectSz, ref _rectAngle);
    }

    ~RoiItem()
    {
    }
    public RoiItem Clone(bool cloneData)
    {
      RoiItem ret = new RoiItem(this);
      if (cloneData)
      {
      }
      return ret;
    }

    XmlSchema IXmlSerializable.GetSchema()
    {
      return null;
    }

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
    }
  }
  public class SessionFrame
  {
    public String _imageFilePath = String.Empty;
    public int _iterationAdded = 0;
    public int _iterationMoved = 0;
    public int _iterationRemoved = 0;
    public bool _found = false;
    public SortedList<String, RoiItem> _roiItems = new SortedList<string, RoiItem>();

    public bool _imgLoaded = false;
    public Mat _matOrig = new Mat();
    public SessionFrame()
    {
    }
    public SessionFrame(String imageFilePath, int iteration)
    {
      _imageFilePath = imageFilePath;
      _iterationAdded = iteration;
      _found = File.Exists(imageFilePath);
    }
    public SessionFrame(SessionFrame src)
    {
      _imageFilePath = src._imageFilePath;
      _iterationAdded = src._iterationAdded;
      _iterationRemoved = src._iterationRemoved;
      _iterationMoved = src._iterationMoved;
      var clonedItems = src._roiItems.ToDictionary(x => x.Key, x => x.Value.Clone(false));
      _roiItems = new SortedList<String, RoiItem>(clonedItems); 
      _found = src._found;
      _imgLoaded = false;
    }
    public SessionFrame Clone(bool cloneData)
    {
      SessionFrame ret = new SessionFrame(this);
      if (cloneData)
      {
        ret._imgLoaded = _imgLoaded;
        if (_imgLoaded)
        {
          ret._matOrig = _matOrig.Clone();
        }
      }
      return ret;
    }

      ~SessionFrame()
    {
    }

    internal Mat loadImage(bool force = false)
    {
      if (!force && _imgLoaded && _matOrig != null)
        return _matOrig;
      _matOrig = CvInvoke.Imread(_imageFilePath, Emgu.CV.CvEnum.ImreadModes.AnyColor);
      _imgLoaded = (_matOrig != null);
      return _matOrig;
    }

    internal IImage GetThumbImage(Size imageSize)
    {
      Mat retmat = new Mat(imageSize, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
      CvInvoke.Resize(loadImage(), retmat, imageSize);
      return retmat;
    }
  }
  public class SessionFrameTag
  {
    public SessionFrame _frame = null;
    public String _key = "";
    public SessionFrameTag() { }
    public SessionFrameTag(string key, ref SessionFrame frame)
    {
      _frame = frame;
      _key = key;
    }
    public static int indexInList(ref List<SessionFrameTag> list, String key)
    {
      var items = list.Where(v => v._key == key).Select((v, n) => n).ToList();
      if (items.Count == 1)
        return items.First();
      if (items.Count == 0)
        return -1;
      if (items.Count > 1)
        return -2;
      //if (items.Count < 0)
      return items.Count; //shutup warning
    }
    public static SessionFrameTag itemInList(ref List<SessionFrameTag> list, String key)
    {
      var items = list.Where(v => v._key == key).Select((v, n) => v).ToList();
      if (items.Count == 1)
        return items.First();
      return null;
    }

    internal static void setTagListItem(ref List<SessionFrameTag> frames, String key, ref SessionFrame frame)
    {
      int idx = SessionFrameTag.indexInList(ref frames, key);
      if (idx == -1)
      {
        frames.Add(new SessionFrameTag(key, ref frame));
      }
      else if (idx >= 0)
      {
        frames[idx]._frame = frame;
      }
    }
  }
  public class SessionMan
  {
    public bool _modified;
    public String _sessionFileName;
    public String _path;
    internal string _ext;
    internal int _iteration;
    internal List<SessionFrameTag> _sessionFrames = new List<SessionFrameTag>();
    internal List< SessionFrameTag> _activeFrames = new List<SessionFrameTag>();
    internal static string SessionFileExt = "cels";
    internal static string SessionFileHeaderName = "celiameter";
    internal static string SessionFileHeaderVersion = "1.0";

    public ImageListStreamer imageListStreamer { get; internal set; }

    public SessionMan()
    {
      _modified = false;
      _iteration = 0;
    }

    ~SessionMan()
    {
    }
    public bool Save()
    {
      return true;
    }

    static internal bool createNewSession(out SessionMan newSession, String sessionFilePath, String sessionPath, String imageFileExt)
    {
      newSession = new SessionMan();
      newSession._iteration = 0;
      newSession._modified = false;
      newSession._ext = imageFileExt;
      newSession._path = sessionPath;
      newSession._sessionFileName = sessionFilePath;
      XmlDocument xdoc = new XmlDocument();
      XmlElement celmElement = xdoc.CreateElement("type");
      celmElement.SetAttribute("name", SessionMan.SessionFileHeaderName);
      celmElement.SetAttribute("version", SessionMan.SessionFileHeaderVersion);
      celmElement.SetAttribute("iteration", newSession._iteration.ToString());
      XmlElement sourcefileElement = xdoc.CreateElement("sourceFile");
      sourcefileElement.SetAttribute("path", newSession._path);
      sourcefileElement.SetAttribute("ext", newSession._ext);
      XmlTextWriter writer = new XmlTextWriter(newSession._sessionFileName, Encoding.UTF8);
      writer.Formatting = Formatting.Indented;
      xdoc.AppendChild(xdoc.CreateElement(SessionMan.SessionFileHeaderName));
      xdoc.DocumentElement.AppendChild(celmElement);
      xdoc.DocumentElement.AppendChild(sourcefileElement);
      writer.WriteStartDocument();
      xdoc.WriteContentTo(writer);
      writer.WriteEndDocument();
      writer.Close();
      return newSession.loadSession(newSession._sessionFileName);
    }
    internal bool loadSession(string sessionFileName)
    {
      if (!File.Exists(sessionFileName))
      {
        return false;
      }
      _activeFrames.Clear();
      foreach (var frm in _sessionFrames)
      {
        frm._frame._found = false;
      }
      var files = Directory.GetFiles(_path, "*" + _ext).OrderBy(f => f);  
      foreach (var filePath in files)
      {
        String fileName = Path.GetFileName(filePath);
        var frame = initFrame(fileName, filePath);
        if (frame != null)
        {
          SessionFrameTag.setTagListItem(ref _sessionFrames, fileName, ref frame);
          if (frame._found)
          {
            SessionFrameTag.setTagListItem(ref _activeFrames, fileName, ref frame);
          }
        }
      }
      var missingFrames = _sessionFrames.Where(f => !f._frame._found);
      foreach (var f in missingFrames)
      {
        f._frame._iterationRemoved = _iteration;
      }
      _modified = false;
      return true;
    }
    internal bool saveSession(string fileName, bool force)
    {
      if (!_modified && !force)
      {
        return true;
      }
      // Directory.GetFiles(_path, "*." + _ext).First();
      return true;
    }

    internal SessionFrame initFrame(String name, String imageFile)
    {
      SessionFrame frame = null;
      if (_sessionFrames.Exists(i => i._key == name))
      {
        frame = SessionFrameTag.itemInList(ref _sessionFrames, name)._frame;
        frame._found = true;
        if (!Path.GetFullPath(frame._imageFilePath).Equals(Path.GetFullPath(imageFile), StringComparison.InvariantCultureIgnoreCase))
        {
          frame._iterationMoved = _iteration;
        }
      }
      else
      {
        frame = new SessionFrame();
        frame._found = true;
        frame._imageFilePath = imageFile;
        frame._iterationAdded = _iteration;
        SessionFrameTag.setTagListItem(ref _sessionFrames, name, ref frame);
      }
      return frame;
    }
  }
}