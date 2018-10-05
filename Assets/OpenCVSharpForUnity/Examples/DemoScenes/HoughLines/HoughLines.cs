using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using NWH;

public class HoughLines : MonoBehaviour
{
	private RawImage rawImage;
	private WebCamTexture webCamTexture;
	private Texture2D tex;
	private Mat mat, gray;
    private Scalar colorScalar;

	void Start()
	{
        rawImage = GameObject.Find("Canvas/RawImage").GetComponent<RawImage>();

		webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
		webCamTexture.Play();

		tex = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGB24, false);
		mat = new Mat(webCamTexture.height, webCamTexture.width, MatType.CV_8UC4);
        gray = new Mat(webCamTexture.height, webCamTexture.width, MatType.CV_8UC1);
        colorScalar = new Scalar(255, 255, 0, 255);
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
        Cv2.CvtColor(mat, mat, ColorConversionCodes.RGBA2RGB);
        Cv2.CvtColor(mat, gray, ColorConversionCodes.RGB2GRAY);
		Cv2.Canny(gray, gray, 90, 100);

        LineSegmentPoint[] segHoughP = Cv2.HoughLinesP(gray, 1, Mathf.PI / 180, 90, 30, 50);

        foreach(LineSegmentPoint p in segHoughP)
        {
            Cv2.Line(mat, p.P1, p.P2, colorScalar, 1, LineTypes.Link4);
        }

        CvConvert.MatToTexture2D(mat, ref tex);
		rawImage.texture = tex;
	}

	private void OnDestroy()
	{
		webCamTexture.Stop();
	}
}