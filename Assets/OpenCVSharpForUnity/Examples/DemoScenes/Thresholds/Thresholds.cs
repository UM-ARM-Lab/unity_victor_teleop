using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;
using NWH;

public class Thresholds : MonoBehaviour
{
	public RawImage rawImage;

	private Texture2D tex;
    private Mat mat, matRGBA;
	private float timeElapsed = 0;
	private int mode = 0;

    private Mat binaryInvMat, toZeroMat, toZeroInvMat, gaussianMat, subtractMat;

	void Start()
	{
		// Avoid using new keyword in Update(), esp. with Mat and Texture2D
		mat = Cv2.ImRead(CvUtil.GetStreamingAssetsPath("lena.png"), ImreadModes.GrayScale);
        matRGBA = new Mat(mat.Width, mat.Height, MatType.CV_8UC4);
        tex = new Texture2D(mat.Height, mat.Width, TextureFormat.RGBA32, false);

        binaryInvMat = new Mat();
        toZeroMat = new Mat();
        toZeroInvMat = new Mat();
        gaussianMat = new Mat();
        subtractMat = new Mat();
    }

	void Update()
	{

        // Performance measuring purposes only, avoid reading data in Update()
        mat = Cv2.ImRead(CvUtil.GetStreamingAssetsPath("lena.png"), ImreadModes.GrayScale);

		// Timer to swithch between different thresholds
		timeElapsed += Time.deltaTime;
		if (timeElapsed > 1.5f)
		{
			timeElapsed = 0;
			mode++;
			if (mode > 4) mode = 0;
		}

		Cv2.Threshold(mat, binaryInvMat, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
		Cv2.Threshold(mat, toZeroMat, 0, 255, ThresholdTypes.Tozero | ThresholdTypes.Otsu);
		Cv2.Threshold(mat, toZeroInvMat, 0, 255, ThresholdTypes.TozeroInv | ThresholdTypes.Otsu);
		Cv2.AdaptiveThreshold(mat, gaussianMat, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 7, 8);
        Cv2.Subtract(gaussianMat, toZeroMat, subtractMat);

        switch(mode)
        {
            case 0:
                mat = subtractMat;
                break;
            case 1:
                mat = binaryInvMat;
                break;
            case 2:
                mat = toZeroMat;
                break;
            case 3:
                mat = gaussianMat;
                break;
            case 4:
                mat = toZeroInvMat;
                break;
            default:
                break;
        }


		Cv2.CvtColor(mat, matRGBA, ColorConversionCodes.GRAY2RGBA);
		CvConvert.MatToTexture2D(matRGBA, ref tex);
		rawImage.texture = tex;
	}
}
