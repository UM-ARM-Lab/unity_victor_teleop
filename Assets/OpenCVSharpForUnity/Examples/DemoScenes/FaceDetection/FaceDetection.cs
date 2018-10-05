using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using NWH;

public class FaceDetection : MonoBehaviour
{
    public RawImage rawImage;
    private WebCamTexture webCamTexture;
    private CascadeClassifier haarCascade;
    private Texture2D tex;
    private Mat mat;
    public int cameraIndex = 0;

    void Start()
    {
        // Initialize WebCamTexture
        CamInit();

        // Initialize mat and tex. Avoid doing this in Update() due to high cost of GC.
        tex = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
        mat = new Mat(webCamTexture.height, webCamTexture.width, MatType.CV_8UC4);

        // Load file needed for face detection.
        haarCascade = new CascadeClassifier(
            CvUtil.GetStreamingAssetsPath("haarcascade_frontalface_alt2.xml"));
    }

    void Update()
    {
        // Only call webcam update if needed
		if (webCamTexture != null && webCamTexture.didUpdateThisFrame && webCamTexture.isPlaying)
		{
			CamUpdate();
		}
    }

    void CamUpdate()
    {
        // Get Mat from WebCamTexture
        CvUtil.GetWebCamMat(webCamTexture, ref mat);

        // Run face detection
        mat = DetectFace(haarCascade, mat);

        // Convert Mat to Texture2D for display
        CvConvert.MatToTexture2D(mat, ref tex);

        // Assign Texture2D to GUI element
        rawImage.texture = tex;
    }

    /// <summary>
    /// Initialize new WebCamTexture
    /// </summary>
    public void CamInit()
    {
        // Do not try to stop WebCamTexture if it does not exist
        if (webCamTexture != null)
            webCamTexture.Stop();

        // Initialize new texture with requested device
		webCamTexture = new WebCamTexture(WebCamTexture.devices[cameraIndex].name);

        // Start playback
		webCamTexture.Play();
	}

    /// <summary>
    /// Switches between multiple cameras if they exist.
    /// </summary>
    public void ChangeCamera()
    {
        cameraIndex++;

        if (cameraIndex >= WebCamTexture.devices.Length)
            cameraIndex = 0;
        
        CamInit();
    }

    /// <summary>
    /// Run face detection using Haar Cascades.
    /// </summary>
    private Mat DetectFace(CascadeClassifier cascade, Mat src)
    {
        Mat result;

        using (var gray = new Mat())
        {          
            result = src.Clone();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // Detect faces
            OpenCvSharp.Rect[] faces = cascade.DetectMultiScale(
                gray, 1.08, 3, HaarDetectionType.ScaleImage, new Size(124, 124));

            // Render all detected faces
            foreach (OpenCvSharp.Rect face in faces)
            {
                var center = new Point
                {
                    X = (int)(face.X + face.Width * 0.5),
                    Y = (int)(face.Y + face.Height * 0.5)
                };
                var axes = new Size
                {
                    Width = (int)(face.Width * 0.5),
                    Height = (int)(face.Height * 0.5)
                };
                Cv2.Ellipse(result, center, axes, 0, 0, 360, new Scalar(255, 255, 255, 128), 4);
            }
        }
        return result;
    }

    private void OnDestroy()
    {
        webCamTexture.Stop();
    }
}
