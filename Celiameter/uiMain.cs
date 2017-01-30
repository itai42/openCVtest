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

namespace Celiameter
{
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
        lvItem.Tag = new SessionFrameTag(frm._key, ref frm._frame);
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

    internal bool SetActiveFrame(ImageBox pbMain, ImageBox pbZoom, ImageBox pbOutView, ref SessionFrameTag frameItemTag, object lockObject)
    {
      lock(lockObject)
      {
       
        _imgOrig = frameItemTag._frame.loadImage();
        _imgDisp = new Mat(_imgOrig.Height, _imgOrig.Width, _imgOrig.Depth, 3);
        CvInvoke.CvtColor(_imgOrig, _imgDisp, Emgu.CV.CvEnum.ColorConversion.BayerGr2Rgb, 3);
        foreach (var roi in frameItemTag._frame._roiItems)
        {
          CvInvoke.Polylines(_imgDisp, roi.Value.Points, true, RoisColor);
        }
        _img = _imgDisp.Clone();
        pbMain.Image = _img;
        if (_hasLastCursor)
        {
          setZoomView(pbMain, _lastCursor, pbZoom);
        }
        //setPBImage(pb, img);
      }
      return true;
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
}
