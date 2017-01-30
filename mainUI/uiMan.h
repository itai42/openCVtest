#pragma once

#include <iostream>

public ref  class uiMan
{
public:
  System::Drawing::Point p1, p2, p3, p4;
  int dragPhase;
  void drawPreview(Emgu::CV::UI::ImageBox^ pbMain, Emgu::CV::UI::ImageBox^ pbOutView, System::Drawing::Point^ p1, System::Drawing::Point^ p2, System::Drawing::Point^ p3);
public:
    static void setPBImage(Emgu::CV::UI::ImageBox^ pb, Emgu::CV::Mat^ img);
    static bool imgToPbCoord(Emgu::CV::UI::ImageBox^ pb, int ix, int iy, System::Drawing::Point^ pbCoord); //True if coords are within limits, false if outside. when outside - the point as the nearest pb point to coords
    static bool pbToImgCoord(Emgu::CV::UI::ImageBox^ pb, int ix, int iy, System::Drawing::Point^ imgCoord); //True if coords are within limits, false if outside. when outside - the point as the nearest image point to coords
	void setZoomView(Emgu::CV::UI::ImageBox^ pb, System::Drawing::Point^ ipCursor, Emgu::CV::UI::ImageBox^ pbZoom);
public:
  Emgu::CV::Mat^ _img;
  Emgu::CV::Mat^ _imgOrig;
  Emgu::CV::Mat^ _imgDisp;
  Emgu::CV::Mat^ _imgZoom;
  Emgu::CV::Mat^ _ImgPreview;
    uiMan();
    ~uiMan();
    void createNewSet(Emgu::CV::UI::ImageBox^ pb, System::String^ file);
    void updateOverlayAndZoom(Emgu::CV::UI::ImageBox^ pbMain, Emgu::CV::UI::ImageBox^ pbZoom, System::Drawing::Point^ canvasPos, System::Drawing::Point^ imagePos);
private:
  void drawRoiRect(Emgu::CV::Mat^ img, System::Drawing::Point^ p1, System::Drawing::Point^ p2, System::Drawing::Point^ p3, Emgu::CV::Structure::MCvScalar color, int z);
  void calcRectPoints(System::Drawing::Point^ p1, System::Drawing::Point^ p2, System::Drawing::Point^ p3, System::Drawing::Point^ A, System::Drawing::Point^ B, System::Drawing::Point^ C, System::Drawing::Point^ D);
  static System::Drawing::Point rotatePoint(System::Drawing::Point P, System::Drawing::PointF center, double alpha);
public:
	void SURF(Emgu::CV::UI::ImageBox^ pb);
};

