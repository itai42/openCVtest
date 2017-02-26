
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
using System.Runtime.Serialization;
using System.Windows.Forms;
using Emgu.CV.Structure;

namespace Celiameter
{
  [XmlRoot("ROI")]
  public class RoiItem
  {
    [XmlElement("BoundingRect")]
    public Rectangle _boundingRect = new Rectangle();

    [XmlElement("TopLeft")]
    public Point TL = new Point();

    [XmlElement("TopRight")]
    public Point TR = new Point();

    [XmlElement("BottomLeft")]
    public Point BL = new Point();

    [XmlElement("BottomRight")]
    public Point BR = new Point();

    [XmlElement("RectSize")]
    public SizeF _rectSz = new SizeF();

    [XmlElement("RectCenter")]
    public PointF _rectCenter = new PointF();

    [XmlElement("RectAngle")]
    public double _rectAngle = 0.0;

    [XmlIgnore]
    public Mat _mat = null;
    [XmlIgnore]
    Point[] _points = new Point[4];

    [XmlIgnore]
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
  }


  [XmlRoot("SessionFrameHolder")]
  public class SessionFrame
  {
    [XmlAttribute("ImageFile")]
    public String _imageFilePath = String.Empty;

    [XmlAttribute("IterationAdded")]
    public int _iterationAdded = 0;

    [XmlAttribute("IterationMoved")]
    public int _iterationMoved = 0;

    [XmlAttribute("IterationRemoved")]
    public int _iterationRemoved = 0;

    [XmlIgnore]
    public bool _found = false;
    [XmlIgnore]
    public SortedList<String, RoiItem> _roiItems = new SortedList<string, RoiItem>();

    public class RoiSerializerItem
    {
      [XmlAttribute("Name")]
      public string _key = "";

      [XmlElement("Rectangle")]
      public RoiItem _val = new RoiItem();

      public RoiSerializerItem()
      {

      }
      public RoiSerializerItem(string key, RoiItem val)
      {
        _key = key;
        _val = val;
      }
    }

    [XmlIgnore]
    List<RoiSerializerItem> _roisForSerialization = null;

    [XmlArray("ROIs")]
    public List<RoiSerializerItem> ROIsForSerialization
    {
      get
      {
        if (_roisForSerialization == null)
        {
          _roisForSerialization = _roiItems.Select(v => new RoiSerializerItem(v.Key, v.Value)).ToList();
        }
        return _roisForSerialization;
      }
      set
      {
        _roiItems = new SortedList<String, RoiItem>(value.ToDictionary(v => v._key, v => v._val));
      }
    }

    [XmlIgnore]
    public string Key { get; set; }

    //[XmlArray("ROIsD")]
    //[XmlArrayItem("PairD")]
    //     [XmlIgnore]
    //     public Dictionary<String, RoiItem> ROIsD
    //     {
    //       get
    //       {
    //         return _roiItems.ToDictionary(v => v.Key, v => v.Value);
    //       }
    //       set
    //       {
    //         _roiItems = new SortedList<String, RoiItem>(value);
    //       }
    //     }

    [XmlIgnore]
    public bool _imgLoaded = false;
    [XmlIgnore]
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
      _matOrig = CvInvoke.Imread(_imageFilePath, Emgu.CV.CvEnum.ImreadModes.AnyColor);// | Emgu.CV.CvEnum.ImreadModes.AnyDepth);
      _imgLoaded = (_matOrig != null);
      return _matOrig;
    }

    internal IImage GetThumbImage(Size imageSize)
    {
      Mat retmat = new Mat(imageSize, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
      CvInvoke.Resize(loadImage(), retmat, imageSize);
      return retmat;
    }

    internal SessionFrame GetSerializationFrame()
    {
      ROIsForSerialization = _roisForSerialization;
      _roisForSerialization = null;
      return this;
    }
  }

  [XmlRoot("SessionFrameHolder")]
  public class SessionFrameTag
  {
    [XmlElement("Frame")]
    public SessionFrame _frame = null;
    [XmlAttribute("name")]
    public String _key = "";
    public SessionFrameTag() { }
    public SessionFrameTag(string key, SessionFrame frame)
    {
      _frame = frame;
      _key = key;
      _frame.Key = _key;
    }
    public static int indexInList(ref List<SessionFrameTag> list, String key)
    {
      var items = list.Select((v, n) => new { Value = v, Index = n }).Where(v => v.Value._key == key).ToList();
      if (items.Count == 1)
        return items.First().Index;
      if (items.Count == 0)
        return -1;
      if (items.Count > 1)
      {
        list.RemoveAll(v => v._key == key);
        return -2;
      }
      //if (items.Count < 0)
      return items.Count; //shutup warning
    }
    public static SessionFrameTag itemInList(ref List<SessionFrameTag> list, String key)
    {
      var items = list.Where(v => v._key == key).ToList();
      if (items.Count == 1)
        return items.First();
      return null;
    }

    internal static void setTagListItem(ref List<SessionFrameTag> frames, String key, ref SessionFrame frame, out bool existed)
    {
      int idx = SessionFrameTag.indexInList(ref frames, key);
      if (idx < 0)
      {
        frames.Add(new SessionFrameTag(key, frame));
        existed = false;
      }
      else //if (idx >= 0)
      {
        frames[idx]._frame = frame;
        existed = true;
      }
    }

  }

  [XmlRoot("Session")]
  public class SessionMan
  {
    [XmlIgnore]
    public bool _loaded = false;

    [XmlIgnore]
    private bool _modified = false;

    [XmlElement("SessionFilename")]
    public String _sessionFileName ="";

    [XmlElement("SessionPath")]
    public String _path = "";

    [XmlAttribute("Ext")]
    public string _ext = ".tif";

    [XmlAttribute("Iteration")]
    public int _iteration = 0;

    [XmlAttribute("Version")]
    public string _version = SessionFileHeaderVersion;

    [XmlIgnore]
    public List<SessionFrameTag> _sessionFrames = new List<SessionFrameTag>();

    [XmlIgnore]
    List<SessionFrame> _sessionFramesForSerialization = null;

    [XmlArray("SessionFrames")]
    public List<SessionFrame> SessionFramesForSerialization
    {
      get
      {
        if (_sessionFramesForSerialization == null)
        {
          _sessionFramesForSerialization = _sessionFrames.Select(v => v._frame).ToList();
        }
        return _sessionFramesForSerialization;
      }
      set
      {
        var frameTags = value.Select(v => new SessionFrameTag(Path.GetFileName(v._imageFilePath), v.GetSerializationFrame())).ToList();
        _sessionFrames = frameTags;
        _sessionFramesForSerialization = null;
      }
    }



    [XmlIgnore]
    internal List< SessionFrameTag> _activeFrames = new List<SessionFrameTag>();

    internal static string SessionFileExt = ".cels";
    internal static string SessionFileHeaderName = "celiameter";
    internal static string SessionFileHeaderVersion = "1.0";
    internal SessionFrame _currentFrame;

    [XmlElement("SessionOptions")]
    internal CMOptions _options = new CMOptions();

    [XmlIgnore]
    public ImageListStreamer imageListStreamer { get; internal set; }
    [XmlIgnore]
    public bool Modified
    {
      get { return (_loaded & _modified); }
      internal set { _modified = (_loaded & value); }
    }

    public SessionMan()
    {
      _iteration = 0;
    }

    ~SessionMan()
    {
    }
    public bool Save()
    {
      return true;
    }

    static internal bool createNewSession(out SessionMan newSession, String sessionFilePath, String imageFileExt)
    {
      String sessionPath = Path.GetDirectoryName(sessionFilePath);
      newSession = new SessionMan();
      newSession._iteration = 0;
      newSession._modified = false;
      newSession._ext = imageFileExt;
      newSession._path = sessionPath;
      newSession._sessionFileName = sessionFilePath;
      if (!newSession.saveSession(sessionFilePath, true))
      {
        return false;
      }
      newSession = SessionMan.loadSession(sessionFilePath);
      return (newSession != null);
    }
    static public SessionMan loadSession(string sessionFileName)
    {
      if (!File.Exists(sessionFileName))
      {
        return null;
      }
      SessionMan session = null;
      XmlTextReader reader = new XmlTextReader(sessionFileName);
      try
      {

        var ser = new XmlSerializer(typeof(SessionMan));
        object loadedSessionObj = ser.Deserialize(reader);
        if (loadedSessionObj == null)
        {
          return null;
        }
        session = (SessionMan)loadedSessionObj;
        session.prepAfterLoad(sessionFileName);

      }
      catch (Exception ex)
      {
        string msg = ex.Message;
        MessageBox.Show(msg);
        return null;
      }
      finally
      {
        reader.Close();
      }
      return session;
    }

    private void prepAfterLoad(string sessionFileName)
    {
      SessionFramesForSerialization = _sessionFramesForSerialization;
      _sessionFramesForSerialization = null;
      _path = Path.GetDirectoryName(sessionFileName);
      _sessionFileName = sessionFileName;
      _activeFrames.Clear();
      foreach (var frm in _sessionFrames)
      {
        frm._frame._found = false;
      }
      bool existed;
      var files = Directory.GetFiles(_path, "*" + _ext).OrderBy(f => f);
      foreach (var filePath in files)
      {
        String fileName = Path.GetFileName(filePath);
        var frame = initFrame(fileName, filePath);
        if (frame != null)
        {
          SessionFrameTag.setTagListItem(ref _sessionFrames, fileName, ref frame, out existed);
          if (!existed)
          {
            Modified = true;
          }
          if (frame._found)
          {
            SessionFrameTag.setTagListItem(ref _activeFrames, fileName, ref frame, out existed);
          }
        }
      }
      var missingFrames = _sessionFrames.Where(f => !f._frame._found);
      foreach (var f in missingFrames)
      {
        f._frame._iterationRemoved = _iteration;
      }
      _modified = false;
      _loaded = true;
    }

    internal bool saveSession(string fileName, bool force = false)
    {
      if (!_modified && !force)
      {
        return true;
      }
      // Directory.GetFiles(_path, "*." + _ext).First();

      //Prep for save
      if (!prepForSave(fileName))
      {
        return false;
      }

      //Save
      bool ret = true;
      XmlTextWriter writer = new XmlTextWriter(_sessionFileName, Encoding.UTF8);
      try
      {

        var ser = new XmlSerializer(typeof(SessionMan));
        writer.Indentation = 2;
        writer.IndentChar = ' ';
        writer.Formatting = Formatting.Indented;
        writer.WriteStartDocument();
        ser.Serialize(writer, this);
        writer.WriteEndDocument();
        //Set state after save
        _modified = false;
      }
      catch (Exception ex)
      {
        string msg = ex.Message;
        MessageBox.Show(msg);
        ret = false;
      }
      finally
      {
        writer.Close();
      }
      return ret;
    }

    private bool prepForSave(string fileName)
    {
      _path = Path.GetDirectoryName(fileName);
      _sessionFramesForSerialization = null;
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
        bool existed;
        SessionFrameTag.setTagListItem(ref _sessionFrames, name, ref frame, out existed);
      }
      return frame;
    }
  }
}