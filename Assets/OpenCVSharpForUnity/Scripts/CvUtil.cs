using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using System.Runtime.InteropServices;
using System.IO;

namespace NWH
{
	public class CvUtil
	{
		/// <summary>
		/// Gets single camera frame in Texture2D format. 
		/// Texture2D will have WebCamTexture's dimension and TextureFormat of RGBA32.
		/// </summary>
		/// <param name="webTex">WebCamTexture from which Texture2D will be created.</param>
		/// <param name="tex">Texture2D into which WebCamTexture contents will be copied. Will be resized to fit.</param>
		public static void GetWebCamTexture2D(WebCamTexture webTex, ref Texture2D tex)
		{
			if (webTex.isPlaying)
			{
				if (tex.width != webTex.width || tex.height != webTex.height
					|| tex.format != TextureFormat.RGBA32)
				{
					tex.Resize(webTex.width, webTex.height, TextureFormat.RGBA32, false);
				}

				CvConvert.WebCamTextureToTexture2D(webTex, ref tex);
			}
		}


		/// <summary>
		/// Get single camera frame in OpenCV Mat format.
		/// Mat will have WebCamTexture's dimensions and MatType of CV_8UC4.
		/// </summary>
		/// <param name="webTex">WebCamTexture from which Texture2D will be created.</param>
		/// <param name="tex">Mat into which WebCamTexture contents will be copied. Will be resized to fit.</param>
		public static void GetWebCamMat(WebCamTexture webTex, ref Mat mat)
		{
			if (webTex.isPlaying)
			{
				CvConvert.WebCamTextureToMat(webTex, ref mat);
			}
		}


		/// <summary>
		/// File must be placed into Assets/StreamingAssets folder at root level. Multiplatform.
		/// </summary>
		/// <returns>Absolute path to the file.</returns>
		/// <param name="fileName">File name of the requested file with extension.</param>
		public static string GetStreamingAssetsPath(string fileName)
		{
			string path = "";
#if UNITY_EDITOR
			path = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
#elif UNITY_ANDROID
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, fileName);
            var loadFile = new WWW("jar:file://" + Application.dataPath + "!/assets/" + fileName);  
            while (!loadFile.isDone) { }
            File.WriteAllBytes(filepath, loadFile.bytes);
            path = filepath;
#elif UNITY_IOS
            path = System.IO.Path.Combine(Application.dataPath, "Raw"); 
            path = System.IO.Path.Combine(path, fileName);
#else
        path = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
#endif
            return path;
		}

	}
}
