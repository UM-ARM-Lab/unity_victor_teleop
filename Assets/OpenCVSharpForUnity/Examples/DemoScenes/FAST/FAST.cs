using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using NWH;

public class FAST : MonoBehaviour
{
	public RawImage rawImage;
	private WebCamTexture webCamTexture;
	private Texture2D tex;
	private Mat mat, gray;

	void Start()
	{
		webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
		webCamTexture.Play();

		tex = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
		mat = new Mat(webCamTexture.height, webCamTexture.width, MatType.CV_8UC4);
        gray = new Mat(webCamTexture.height, webCamTexture.width, MatType.CV_8UC1);

        mat = Cv2.ImRead(CvUtil.GetStreamingAssetsPath("lena.jpg"), ImreadModes.GrayScale);
	}

	void Update()
	{
        if (webCamTexture.didUpdateThisFrame && webCamTexture.isPlaying && rawImage != null)
		{
			CamUpdate();
		}
	}

	void CamUpdate()
	{
		CvUtil.GetWebCamMat(webCamTexture, ref mat);

        Cv2.CvtColor(mat, gray, ColorConversionCodes.RGBA2GRAY);

		KeyPoint[] keypoints = Cv2.FAST(gray, 50, true);

		foreach (KeyPoint kp in keypoints)
		{
            mat.Circle(kp.Pt, 3, new Scalar(255, 0, 0, 255), -1, LineTypes.AntiAlias, 0);
		}

		CvConvert.MatToTexture2D(mat, ref tex);
		rawImage.texture = tex;
	}

	private void OnDestroy()
	{
		webCamTexture.Stop();
	}
}