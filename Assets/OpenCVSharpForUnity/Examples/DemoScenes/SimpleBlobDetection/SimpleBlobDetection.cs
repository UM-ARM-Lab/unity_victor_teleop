using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using NWH;

public class SimpleBlobDetection : MonoBehaviour
{
	public RawImage rawImage;

	private WebCamTexture webCamTexture;
	private Texture2D tex;
    private Mat mat, greyMat;

	void Start()
	{
		webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
		webCamTexture.Play();

		tex = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
		mat = new Mat(webCamTexture.height, webCamTexture.width, MatType.CV_8UC4);
        greyMat = new Mat(webCamTexture.height, webCamTexture.width, MatType.CV_8UC1);
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
        Cv2.CvtColor(mat, greyMat, ColorConversionCodes.RGBA2GRAY);
        Cv2.Threshold(greyMat, greyMat, 100, 255, ThresholdTypes.Binary);

		var detectorParams = new SimpleBlobDetector.Params
		{
			//MinDistBetweenBlobs = 10, // 10 pixels between blobs
			//MinRepeatability = 1,

			//MinThreshold = 100,
			//MaxThreshold = 255,
			//ThresholdStep = 5,

			FilterByArea = false,
			//FilterByArea = true,
			//MinArea = 0.001f, // 10 pixels squared
			//MaxArea = 500,

			FilterByCircularity = false,
			//FilterByCircularity = true,
			//MinCircularity = 0.001f,

			FilterByConvexity = false,
			//FilterByConvexity = true,
			//MinConvexity = 0.001f,
			//MaxConvexity = 10,

			FilterByInertia = false,
			//FilterByInertia = true,
			//MinInertiaRatio = 0.001f,

			FilterByColor = false
			//FilterByColor = true,
			//BlobColor = 255 // to extract light blobs
		};
		var simpleBlobDetector = SimpleBlobDetector.Create(detectorParams);
        var keyPoints = simpleBlobDetector.Detect(greyMat);

		Cv2.DrawKeypoints(
                image: greyMat,
				keypoints: keyPoints,
				outImage: mat,
				color: Scalar.FromRgb(255, 0, 0),
				flags: DrawMatchesFlags.DrawRichKeypoints);

        CvConvert.MatToTexture2D(mat, ref tex); 
        rawImage.texture = tex;
	}

	private void OnDestroy()
	{
		webCamTexture.Stop();
	}
}