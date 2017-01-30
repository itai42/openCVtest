// OpenCVTest.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main(int argc, char** argv)
{
	if (argc != 2)
	{
		cout << " Usage: display_image ImageToLoadAndDisplay" << endl;
		return -1;
	}

	Mat srcImg,dispImg;
	srcImg = imread(argv[1], IMREAD_COLOR); // Read the file
	if (!srcImg.data) // Check for invalid input
	{
		cout << "Could not open or find the image" << std::endl;
		return -1;
	}

	namedWindow("wndMain", WINDOW_AUTOSIZE); // Create a window for display.
	int bright = 128, contrast = 26;
	cvCreateTrackbar("brightness", "wndMain", &bright, 255, NULL);
	cvCreateTrackbar("contrast", "wndMain", &contrast, 50, NULL); 
	while (true)
	{
		//IplImage* frame = 0; frame = cvQueryFrame(capture);
		if (!srcImg.data) break;
		cv::resize(srcImg, dispImg, Size(500, 400), 0, 0, INTER_CUBIC);
		imshow("wndMain", dispImg); // Show our image inside it.
		int c = cvWaitKey(20);
		if ((char)c == 27 || (char)c == 'q') break;
	}
	return 0;
}