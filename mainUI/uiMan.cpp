#include "uiMan.h"
#include <vector>

using namespace std;
using namespace System::Drawing;
using namespace System;
using namespace Emgu;
using namespace Emgu::CV;
using namespace Emgu::Util;
using namespace Emgu::CV::UI;
using namespace Emgu::CV::Structure;
uiMan::uiMan()
{
  _img = gcnew Mat();
  _imgOrig = gcnew Mat();
  dragPhase = 0;
}


uiMan::~uiMan()
{
}

bool uiMan::imgToPbCoord(Emgu::CV::UI::ImageBox^ pb, int ix, int iy, System::Drawing::Point^ pbCoord) //True if coords are within limits, false if outside. when outside - the point as the nearest pb point to coords
{
  if (pb == nullptr || pbCoord == nullptr || pb->Image == nullptr)
    return false;

  double x = (double)ix;
  double y = (double)iy;
  double pic_hgt = (double)pb->ClientSize.Height;
  double pic_wid = (double)pb->ClientSize.Width;
  double img_hgt = (double)pb->Image->Size.Height;
  double img_wid = (double)pb->Image->Size.Width;
  double img_zoom = (double)pb->ZoomScale;

  int offsetX = (int)(x * img_zoom);
  int offsetY = (int)(y * img_zoom);
  int horizontalScrollBarValue = (pb->HorizontalScrollBar->Visible) ? (int)(pb->HorizontalScrollBar->Value) : 0;
  int verticalScrollBarValue = (pb->VerticalScrollBar->Visible) ? (int)(pb->VerticalScrollBar->Value) : 0;
  int X0 = offsetX - (int)((double)horizontalScrollBarValue * img_zoom);
  int Y0 = offsetY - (int)((double)verticalScrollBarValue * img_zoom);
  pbCoord->X = System::Math::Min(pb->ClientSize.Width - 1, System::Math::Max(0, X0));
  pbCoord->Y = System::Math::Min(pb->ClientSize.Height - 1, System::Math::Max(0, Y0));
  return (X0 >= 0 && X0 < pb->ClientSize.Width && Y0 >= 0 && Y0 < pb->ClientSize.Height);
}

bool uiMan::pbToImgCoord(Emgu::CV::UI::ImageBox^ pb, int ix, int iy, System::Drawing::Point^ imgCoord) //True if coords are within limits, false if outside. when outside - the point as the nearest image point to coords
{
  if (pb == nullptr || imgCoord == nullptr || pb->Image == nullptr)
    return false;

  double x = (double)ix;
  double y = (double)iy;
  double pic_hgt = (double)pb->ClientSize.Height;
  double pic_wid = (double)pb->ClientSize.Width;
  double img_hgt = (double)pb->Image->Size.Height;
  double img_wid = (double)pb->Image->Size.Width;
  double img_zoom = (double)pb->ZoomScale;

  int offsetX = (int)(x / img_zoom);
  int offsetY = (int)(y / img_zoom);
  int horizontalScrollBarValue = (pb->HorizontalScrollBar->Visible) ? (int)(pb->HorizontalScrollBar->Value) : 0;
  int verticalScrollBarValue = (pb->VerticalScrollBar->Visible) ? (int)(pb->VerticalScrollBar->Value) : 0;
  int X0 = offsetX + horizontalScrollBarValue;
  int Y0 = offsetY + verticalScrollBarValue;
  imgCoord->X = System::Math::Min(pb->Image->Size.Width - 1, System::Math::Max(0, X0));
  imgCoord->Y = System::Math::Min(pb->Image->Size.Height - 1, System::Math::Max(0, Y0));
  return (X0 >= 0 && X0 < pb->Image->Size.Width && Y0 >= 0 && Y0 < pb->Image->Size.Height);
}

void uiMan::setZoomView(Emgu::CV::UI::ImageBox^ pb, Point^ ipCursor, Emgu::CV::UI::ImageBox^ pbZoom)
{
  if (pb == nullptr || pbZoom == nullptr || _img == nullptr)
    return;
  Size zoomSz = pbZoom->ClientSize;
  Point centerLeft = Point(zoomSz.Width / 2, zoomSz.Height / 2);
  Point centerRight = Point(zoomSz.Width - (centerLeft.X + 1), zoomSz.Height - (centerLeft.Y + 1));
  //int imX = System::Math::Min(pb->Image->Size.Width-1, System::Math::Max(0, ipCursor->X));
  //int imY = System::Math::Min(pb->Image->Size.Height-1, System::Math::Max(0, ipCursor->Y));
  //Rectangle roi = Rectangle(Point::Subtract(*ipCursor, center), zoomSz);
  int imW = _img->Size.Width;
  int imH = _img->Size.Height;
  int roiLeft = ipCursor->X - centerLeft.X;
  int roiRight = ipCursor->X + centerRight.X;
  int roiTop = ipCursor->Y - centerLeft.Y;
  int roiBottom = ipCursor->Y + centerRight.Y;
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
  Mat^ zoomCrop = gcnew Mat(_img, Rectangle(roiLeft, roiTop, roiW, roiH));
  Mat^ zoomImg = gcnew Mat(zoomSz.Height, zoomSz.Width, _img->Depth, 3);
  Rectangle zoomRoi = Rectangle(padLeft, padTop, roiW, roiH);
  Mat^ zoomImgROI = gcnew Mat(zoomImg, zoomRoi);
  zoomImg->SetTo(MCvScalar(255, 255, 255), nullptr);
  zoomCrop->CopyTo(zoomImgROI, nullptr);
  int z = 6;
  CvInvoke::Line(zoomImg, Point(centerLeft.X - 25, centerLeft.Y - 1), Point(centerLeft.X - z, centerLeft.Y - 1), MCvScalar(55, 55, 255), 1, CvEnum::LineType::FourConnected, 0);
  CvInvoke::Line(zoomImg, Point(centerLeft.X + z, centerLeft.Y - 1), Point(centerLeft.X + 25, centerLeft.Y - 1), MCvScalar(55, 55, 255), 1, CvEnum::LineType::FourConnected, 0);
  CvInvoke::Line(zoomImg, Point(centerLeft.X - 25, centerLeft.Y + 1), Point(centerLeft.X - z, centerLeft.Y + 1), MCvScalar(55, 55, 255), 1, CvEnum::LineType::FourConnected, 0);
  CvInvoke::Line(zoomImg, Point(centerLeft.X + z, centerLeft.Y + 1), Point(centerLeft.X + 25, centerLeft.Y + 1), MCvScalar(55, 55, 255), 1, CvEnum::LineType::FourConnected, 0);
  CvInvoke::Line(zoomImg, Point(centerLeft.X - 25, centerLeft.Y), Point(centerLeft.X + 25, centerLeft.Y), MCvScalar(0, 0, 0), 1, CvEnum::LineType::FourConnected, 0);
  CvInvoke::Line(zoomImg, Point(centerLeft.X - 1, centerLeft.Y - 25), Point(centerLeft.X - 1, centerLeft.Y - z), MCvScalar(55, 55, 255), 1, CvEnum::LineType::FourConnected, 0);
  CvInvoke::Line(zoomImg, Point(centerLeft.X - 1, centerLeft.Y + z), Point(centerLeft.X - 1, centerLeft.Y + 25), MCvScalar(55, 55, 255), 1, CvEnum::LineType::FourConnected, 0);
  CvInvoke::Line(zoomImg, Point(centerLeft.X + 1, centerLeft.Y - 25), Point(centerLeft.X + 1, centerLeft.Y - z), MCvScalar(55, 55, 255), 1, CvEnum::LineType::FourConnected, 0);
  CvInvoke::Line(zoomImg, Point(centerLeft.X + 1, centerLeft.Y + z), Point(centerLeft.X + 1, centerLeft.Y + 25), MCvScalar(55, 55, 255), 1, CvEnum::LineType::FourConnected, 0);
  CvInvoke::Line(zoomImg, Point(centerLeft.X, centerLeft.Y - 25), Point(centerLeft.X, centerLeft.Y + 25), MCvScalar(0, 0, 0), 1, CvEnum::LineType::FourConnected, 0);
  pbZoom->Image = zoomImg;
  _imgZoom = zoomImg;
}

void uiMan::createNewSet(Emgu::CV::UI::ImageBox^ pb, System::String^ file)
{
  _imgOrig = CvInvoke::Imread(file, CvEnum::ImreadModes::AnyColor);
  _imgDisp = gcnew Mat(_imgOrig->Height, _imgOrig->Width, _imgOrig->Depth, 3);
  CvInvoke::CvtColor(_imgOrig, _imgDisp, CvEnum::ColorConversion::BayerGr2Rgb, 3);
  _img = _imgDisp->Clone();
  pb->Image = _img;
  //setPBImage(pb, img);
}

void uiMan::updateOverlayAndZoom(Emgu::CV::UI::ImageBox^ pbMain, Emgu::CV::UI::ImageBox^ pbZoom, Point^ canvasPos, Point^ imagePos)
{
  Mat^ buff = _imgDisp->Clone();
  switch (dragPhase)
  {
  default:
  case 0:

    break;
  case 1:
    drawRoiRect(buff, p1, imagePos, nullptr, MCvScalar(0,0,255), -1);
    break;
  case 2:
    drawRoiRect(buff, p1, p2, imagePos, MCvScalar(0, 0, 255), -1);
    break;
  }
  _img = buff;
  pbMain->Image = _img;
  setZoomView(pbMain, imagePos, pbZoom);
}

void uiMan::drawRoiRect(Mat^ img, Point^ p1, Point^ p2, Point^ p3, Emgu::CV::Structure::MCvScalar color, int z)
{
  if (p1 == nullptr || p2 == nullptr)
  {
    return;
  }
  CvInvoke::Line(img, *p1, *p2, color, 1, CvEnum::LineType::AntiAlias, 0);
/*
  double alpha = System::Math::Atan(((double)(p2->Y - p1->Y)) / ((double)(p2->X - p1->X)))*180.0/ System::Math::PI; //System::Math::Asin(((double)(B->Y - A->Y)) / ((double)(B->X - A->X)));
  double beta = 90.0 - alpha;//System::Math::Acos(alpha);
  CvInvoke::PutText(img, alpha.ToString(), *p1, Emgu::CV::CvEnum::FontFace::HersheyPlain, 2, color, 1, CvEnum::LineType::AntiAlias, false);
  CvInvoke::PutText(img, beta.ToString(), *p2, Emgu::CV::CvEnum::FontFace::HersheyPlain, 2, color, 1, CvEnum::LineType::AntiAlias, false);
*/

  if (p3 == nullptr)
  {
    return;
  }
  Point^ A = gcnew Point();
  Point^ B = gcnew Point();
  Point^ C = gcnew Point();
  Point^ D = gcnew Point();
  calcRectPoints(p1, p2, p3, A, B, C, D);
  CvInvoke::Line(img, *A, *B, color, 1, CvEnum::LineType::AntiAlias, 0);
  CvInvoke::Line(img, *B, *C, color, 1, CvEnum::LineType::AntiAlias, 0);
  CvInvoke::Line(img, *C, *D, color, 1, CvEnum::LineType::AntiAlias, 0);
  CvInvoke::Line(img, *D, *A, color, 1, CvEnum::LineType::AntiAlias, 0);

/*
  CvInvoke::PutText(img, "A", *A, Emgu::CV::CvEnum::FontFace::HersheyPlain, 1, color, 1, CvEnum::LineType::AntiAlias, true);
  CvInvoke::PutText(img, "B", *B, Emgu::CV::CvEnum::FontFace::HersheyPlain, 1, color, 1, CvEnum::LineType::AntiAlias, true);
  CvInvoke::PutText(img, "C", *C, Emgu::CV::CvEnum::FontFace::HersheyPlain, 1, color, 1, CvEnum::LineType::AntiAlias, true);
  CvInvoke::PutText(img, "D", *D, Emgu::CV::CvEnum::FontFace::HersheyPlain, 1, color, 1, CvEnum::LineType::AntiAlias, true);
  CvInvoke::PutText(img, "p1", *p1, Emgu::CV::CvEnum::FontFace::HersheyPlain, 1, color, 1, CvEnum::LineType::AntiAlias, false);
  CvInvoke::PutText(img, "p2", *p2, Emgu::CV::CvEnum::FontFace::HersheyPlain, 1, color, 1, CvEnum::LineType::AntiAlias, false);
  CvInvoke::PutText(img, "p3", *p3, Emgu::CV::CvEnum::FontFace::HersheyPlain, 1, color, 1, CvEnum::LineType::AntiAlias, false);

*/
}

void uiMan::drawPreview(Emgu::CV::UI::ImageBox^ pbMain, Emgu::CV::UI::ImageBox^ pbOutView, Point^ p1, Point^ p2, Point^ p3)
{
  Point^ A = gcnew Point();
  Point^ B = gcnew Point();
  Point^ C = gcnew Point();
  Point^ D = gcnew Point();
  calcRectPoints(p1, p2, p3, A, B, C, D);
  Mat^ src = _imgOrig;
  Mat^ dst = gcnew Mat(pbOutView->Size, src->Depth, src->NumberOfChannels);
  PointF rectCenter = PointF((((double)(A->X + B->X + C->X + D->X)) / 4.0), (((double)(A->Y + B->Y + C->Y + D->Y)) / 4.0));
  SizeF rectSz = SizeF(System::Math::Sqrt(System::Math::Pow(B->X - A->X, 2) + System::Math::Pow(B->Y - A->Y, 2)), System::Math::Sqrt(System::Math::Pow(D->X - A->X, 2) + System::Math::Pow(D->Y - A->Y, 2)));
  double alpha = -System::Math::Atan(((double)(p2->Y - p1->Y)) / ((double)(p2->X - p1->X)))*180.0 / System::Math::PI;
  double beta = 90.0 - alpha;
  Mat^ rotMat = gcnew Mat();
  RotatedRect recCrop = Emgu::CV::Structure::RotatedRect(rectCenter, rectSz, (float)alpha);

  double angle = alpha * System::Math::PI / 180.0;
  Point Ar = rotatePoint(*A, rectCenter, angle);
  //Point Br = rotatePoint(*B, rectCenter, angle);
  Point Cr = rotatePoint(*C, rectCenter, angle);;
  //Point Dr = rotatePoint(*D, rectCenter, angle);
  CvInvoke::GetRotationMatrix2D(rectCenter, -alpha, 1, rotMat);
  CvInvoke::WarpAffine(src, dst, rotMat, src->Size, Emgu::CV::CvEnum::Inter::Cubic, Emgu::CV::CvEnum::Warp::Default, Emgu::CV::CvEnum::BorderType::Wrap, MCvScalar(255, 255, 255));
  /*
    MCvScalar color = MCvScalar(255, 0, 0);
  CvInvoke::Line(dst, Ar, Br, color, 1, CvEnum::LineType::AntiAlias, 0);
  CvInvoke::Line(dst, Br, Cr, color, 1, CvEnum::LineType::AntiAlias, 0);
  CvInvoke::Line(dst, Cr, Dr, color, 1, CvEnum::LineType::AntiAlias, 0);
  CvInvoke::Line(dst, Dr, Ar, color, 1, CvEnum::LineType::AntiAlias, 0);
  CvInvoke::PutText(dst, "A'", Ar, Emgu::CV::CvEnum::FontFace::HersheyPlain, 1, color, 1, CvEnum::LineType::AntiAlias, false);
  CvInvoke::PutText(dst, "B'", Br, Emgu::CV::CvEnum::FontFace::HersheyPlain, 1, color, 1, CvEnum::LineType::AntiAlias, false);
  CvInvoke::PutText(dst, "C'", Cr, Emgu::CV::CvEnum::FontFace::HersheyPlain, 1, color, 1, CvEnum::LineType::AntiAlias, false);
  CvInvoke::PutText(dst, "D'", Dr, Emgu::CV::CvEnum::FontFace::HersheyPlain, 1, color, 1, CvEnum::LineType::AntiAlias, false);
*/
  Point topLeft = Point(Math::Min(Cr.X, Ar.X), Math::Min(Cr.Y, Ar.Y));
  Size Sz = Size(Math::Abs(Cr.X-Ar.X), Math::Abs(Cr.Y-Ar.Y));
  pbOutView->Image = _ImgPreview = gcnew Mat(dst, System::Drawing::Rectangle(Ar, Sz));
}


void uiMan::setPBImage(Emgu::CV::UI::ImageBox^ pb, Mat^ img)
{
  //System::Drawing::Graphics^ graphics = pb->CreateGraphics();
  //System::IntPtr ptr(img->Ptr);
  //System::Drawing::Bitmap^ b = gcnew System::Drawing::Bitmap(img->Cols, img->Rows, img->Step, System::Drawing::Imaging::PixelFormat::Format24bppRgb, ptr);
  //System::Drawing::RectangleF rect(0, 0, pb->Width, pb->Height);
  //pb->FunctionalMode = ImageBox::FunctionalModeOption::Everything;
  //Image<Bgr, unsigned char> rstImg = gcnew Image<Bgr, unsigned char>(*b);
  //->Clone();

  //graphics->DrawImage(b, rect);
}
     

System::Drawing::Point uiMan::rotatePoint(System::Drawing::Point P, System::Drawing::PointF center, double alpha)
{
	Point rc;
	rc.X = (int)((((double)P.X - center.X) * Math::Cos(alpha)) - (((double)P.Y - center.Y) * Math::Sin(alpha)) + center.X);
	rc.Y = (int)((((double)P.X - center.X) * Math::Sin(alpha)) + (((double)P.Y - center.Y) * Math::Cos(alpha)) + center.Y);
	return rc;
}

using namespace Emgu::CV::Util;
using namespace Emgu::CV::Features2D;
using namespace Emgu::CV::XFeatures2D;
using namespace Emgu::CV::Cuda;

void FindMatchSURF(Mat^ modelImage, Mat^ observedImage/*, long matchTime*/, VectorOfKeyPoint^ modelKeyPoints, VectorOfKeyPoint^ observedKeyPoints, VectorOfVectorOfDMatch^ matches, Mat^ mask, Mat^ homography)
{

	int k = 2;
	double uniquenessThreshold = 0.8;
	double hessianThresh = 300;

	CudaSURF^ surfCuda = gcnew CudaSURF((float)hessianThresh, 4, 2, true, (float)0.01, false);
	GpuMat^ gpuModelImage = gcnew GpuMat(modelImage);

	GpuMat^ gpuModelKeyPoints = surfCuda->DetectKeyPointsRaw(gpuModelImage, nullptr);
	GpuMat^ gpuModelDescriptors = surfCuda->ComputeDescriptorsRaw(gpuModelImage, nullptr, gpuModelKeyPoints);
	CudaBFMatcher^ matcher = gcnew CudaBFMatcher(DistanceType::L2);

	surfCuda->DownloadKeypoints(gpuModelKeyPoints, modelKeyPoints);
	//Stopwatch watch = Stopwatch.StartNew();

	// extract features from the observed image
	GpuMat^ gpuObservedImage = gcnew GpuMat(observedImage);
	GpuMat^ gpuObservedKeyPoints = surfCuda->DetectKeyPointsRaw(gpuObservedImage, nullptr);
	GpuMat^ gpuObservedDescriptors = surfCuda->ComputeDescriptorsRaw(gpuObservedImage, nullptr, gpuObservedKeyPoints);
	//GpuMat tmp = new GpuMat())
	//Stream stream = new Stream())
	matcher->KnnMatch(gpuObservedDescriptors, gpuModelDescriptors, matches, k, nullptr, false);

	surfCuda->DownloadKeypoints(gpuObservedKeyPoints, observedKeyPoints);

	mask = gcnew Mat(matches->Size, 1, CvEnum::DepthType::Cv8U, 1);
	mask->SetTo(MCvScalar(255), nullptr);
	Features2DToolbox::VoteForUniqueness(matches, uniquenessThreshold, mask);

	int nonZeroCount = CvInvoke::CountNonZero(mask);
	if (nonZeroCount >= 4)
	{
		nonZeroCount = Features2DToolbox::VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,
			matches, mask, 1.5, 20);
		if (nonZeroCount >= 4)
			homography = Features2DToolbox::GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, matches, mask, 2);
	}
	//watch.Stop();
	//matchTime = watch.ElapsedMilliseconds;
	/*
	//Extract SURF points by initializing parameters
	Emgu::CV::CvSURFParams params = CvInvoke::cvSURFParams(500, 1);
	cvExtractSURF(image, 0, &imageKeypoints, &imageDescriptors, storage, params);
	printf("Image Descriptors: %d\n", imageDescriptors->total);

	//draw the keypoints on the captured frame
	for (i = 0; i < imageKeypoints->total; i++)
	{
	CvSURFPoint* r = (CvSURFPoint*)cvGetSeqElem(imageKeypoints, i);
	CvPoint center;
	int radius;
	center.x = cvRound(r->pt.x);
	center.y = cvRound(r->pt.y);
	radius = cvRound(r->size*1.2 / 9. * 2);
	cvCircle(frame, center, radius, red_color[0], 1, 8, 0);
	}
	*/
}

using namespace System;
Mat DrawSURF(Mat^ modelImage, Mat^ observedImage)//, long matchTime)
{
	Mat^ mask = gcnew Mat();
	Mat^ homography = gcnew Mat();
	VectorOfVectorOfDMatch^ matches = gcnew VectorOfVectorOfDMatch();
	VectorOfKeyPoint^ modelKeyPoints = gcnew VectorOfKeyPoint();
	VectorOfKeyPoint^ observedKeyPoints = gcnew VectorOfKeyPoint();
	FindMatchSURF(modelImage, observedImage, /*matchTime, */modelKeyPoints, observedKeyPoints, matches, mask, homography);

	//Draw the matched keypoints
	Mat^ result = gcnew Mat();
	Features2DToolbox::DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
		matches, result, MCvScalar(255, 255, 255), MCvScalar(255, 255, 255), mask, Features2DToolbox::KeypointDrawType::Default);

	//region draw the projected region on the image

	if (homography != nullptr)
	{
		//draw a rectangle along the projected model
		Rectangle rect = Rectangle(Point(0, 0), modelImage->Size);
		array<int>^ ptci = gcnew array< int>(4);
		array<PointF^,1>^ ptcv = gcnew array< PointF^ ,1>(4);
		CV::Util::VectorOfPointF^ ptsT = gcnew CV::Util::VectorOfPointF();
		ptsT->Push(&PointF(rect.Left, rect.Bottom));
		ptsT->Push(gcnew PointF(rect.Right, rect.Bottom));
		ptsT->Push(gcnew PointF(rect.Right, rect.Top));
		ptsT->Push(gcnew PointF(rect.Left, rect.Top));
		CV::Util::VectorOfPointF^ pts = gcnew CV::Util::VectorOfPointF();
		CvInvoke::PerspectiveTransform(pts, ptsT, homography);

		CV::Util::VectorOfPoint^ points = Array::ConvertAll<PointF, Point>(ptsT, gcnew System::Converter<PointF, Point>(Point::Round));
		VectorOfPoint^ vp = gcnew VectorOfPoint(points))
		CvInvoke::Polylines(result, vp, true, MCvScalar(255, 0, 0, 255), 5);
	}
	return result;
}

void uiMan::SURF(Emgu::CV::UI::ImageBox^ pb)
{
	DrawSURF(_ImgPreview, _imgDisp);
	pb->Image = _imgDisp;
}

void uiMan::calcRectPoints(Point^ p1, Point^ p2, Point^ p3, Point^ A, Point^ B, Point^ C, Point^ D)
{
	A->X = p1->X;
	A->Y = p1->Y;
	B->X = p2->X;
	B->Y = p2->Y;
	if (p1->X == p2->X)
	{
		C->X = p3->X;
		C->Y = p2->Y;
		D->X = p3->X;
		D->Y = p1->Y;
	}
	else if (p1->Y == p2->Y)
	{
		C->X = p2->X;
		C->Y = p3->Y;
		D->X = p1->X;
		D->Y = p3->Y;
	}
	else
	{
		double dx1, dx2, dx3, dx4, dx5, dy1, dy2, dy3, dy4, dy5;
		dx1 = p1->X;
		dx2 = p2->X;
		dx3 = p3->X;
		dy1 = p1->Y;
		dy2 = p2->Y;
		dy3 = p3->Y;
		double alpha, beta, a, b, c, d;
		alpha = (dy2 - dy1) / (dx2 - dx1);
		beta = -1 / alpha;
		b = dy3 - alpha*dx3;
		c = dy1 - beta*dx1;
		d = dy2 - beta*dx2;
		dx4 = (c - b) / (alpha - beta);
		dy4 = c + beta*dx4;
		dx5 = (d - b) / (alpha - beta);
		dy5 = d + beta*dx5;
		C->X = (int)dx5;
		C->Y = (int)dy5;
		D->X = (int)dx4;
		D->Y = (int)dy4;
	}
}
