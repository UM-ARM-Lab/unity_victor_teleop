using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using NWH;

public class MotionDetection : MonoBehaviour
{
	public RawImage rawImage;

	private WebCamTexture webCamTexture;
	private Texture2D tex;
	private Mat mat, fg, res, nm;
	private BackgroundSubtractorMOG2 mog2;
	private Point[][] points;
	private HierarchyIndex[] hIndex;

	void Start()
	{
		webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
		webCamTexture.Play();

		// Avoid using new keyword in Update(), esp. with Mat and Texture2D
		tex = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
		mat = fg = new Mat(webCamTexture.height, webCamTexture.width, MatType.CV_8UC4);
		nm = new Mat();
		mog2 = BackgroundSubtractorMOG2.Create(200, 16, true);
	}

	void Update()
	{
		if (webCamTexture.didUpdateThisFrame && webCamTexture.isPlaying)
		{
			CamUpdate();
		}
	}

	void CamUpdate()
	{
		CvUtil.GetWebCamMat(webCamTexture, ref mat);

		mog2.Apply(mat, fg, 0.05f);
		Cv2.GaussianBlur(fg, fg, new Size(21, 21), 0);
		Cv2.Threshold(fg, fg, 30, 255, ThresholdTypes.Binary);
		Cv2.Dilate(fg, fg, nm, default(Point?), 2);
		Cv2.CvtColor(fg, fg, ColorConversionCodes.GRAY2BGRA);
		Cv2.Add(mat, fg, fg);

		CvConvert.MatToTexture2D(fg, ref tex);
		rawImage.texture = tex;
	}

	private void OnDestroy()
	{
		webCamTexture.Stop();
	}
}
