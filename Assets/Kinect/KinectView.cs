using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using RosSharp.RosBridgeClient;
using Unity.Collections;


public class KinectView : MonoBehaviour {

    public RosSharp.RosBridgeClient.ImageListener colorListener;
    public RosSharp.RosBridgeClient.DepthImageListener depthListener;
    //public RosSharp.RosBridgeClient.ImageRawListener depthListenerRaw;
    public Transform kinect_offset;

    //public string depth_topic;

    //private WebsocketClient wsc;

    //int framerate = 100;
    public string compression = "none"; //"png" is the other option, haven't tried it yet though
    string depthMessage;
    string colorMessage;
    

    public Material material;
    Texture2D depthTexture;
    Texture2D colorTexture;

    //int width = 512;
    //int height = 424;
    int width;
    int height;

    Matrix4x4 m;

    // Use this for initialization
    void Start()
    {
        width = depthListener.width;
        height = depthListener.height;
        // Create a texture for the depth image and color image
        depthTexture = new Texture2D(width, height, TextureFormat.R16, false);
        //depthTexture = new Texture2D(width, height, TextureFormat.RG16, false);
        colorTexture = new Texture2D(2, 2);


        //InvokeRepeating("UpdateTexture", 0.1f, 0.1f);
    }

    void Update()
    {
        if(colorListener.HasNew())
        {
            //Debug.Log("color texture updated");
            colorTexture.LoadImage(colorListener.GetLast());
            colorTexture.Apply();
        }
        if(depthListener.HasNew())
        {
            //long loc = 519360;
            //int x = 480;
            //int y = 270;
            //byte[] raw = depthListener.GetLast();
            //Debug.Log("Raw length: " + raw.Length);
            //Debug.Log("Raw: " + raw[0] + ", " + raw[1]);
            //new ImageMagick.MagickReadSettings
            //MagickReadSettings settings = new MagickReadSettings();
            //settings.
            //ImageMagick.MagickImage image = new ImageMagick.MagickImage(raw, settings);

            //Mat mat = new Mat(width, height, MatType.CV_16U, raw);
            //Mat mat = Mat.FromImageData(raw, ImreadModes.AnyDepth);

            //Mat mat = Mat.ImDecode(depthListener.GetLast(), ImreadModes.AnyDepth);

            //Debug.Log("Loaded mat");
            //Debug.Log("Properties: channels " + mat.Channels() + 
            //    ", depth " + mat.Depth() + 
            //    ", type " + mat.Type() + 
            //    ", size " + mat.Size());
            //mat.ImEncode();

            //Mat mat2 = new Mat();
            //mat.ConvertTo(mat2, MatType.CV_16U);
            //Debug.Log("opencv size: " + mat.ToBytes().Length);
            //Debug.Log("mat2 size: " + mat.ToBytes(".raw").Length);
            //short[] data = new short[960 * 540];
            //mat.GetArray(0, 0, data);
            //mat.GetArray()
            //byte[] a = data;
            // NativeArray<short> a = new NativeArray<short>(data, Allocator.Temp);



            //depthTexture.LoadRawTextureData<short>(a);
            depthTexture.LoadRawTextureData<short>(depthListener.GetLast());

            //Debug.Log("data size: " + data.Length);
            //Debug.Log("sizeof(short): " + sizeof(short));

            /*
            //System.Drawing.Bitmap bmp = image.ToBitmap(System.Drawing.Imaging.ImageFormat.Bmp);
            Debug.Log("initial      depth: " + image.Depth +
                ", channels: " + image.ChannelCount +
                ", datasize: " + image.ToByteArray().Length +
                ", pix: " + image.GetPixels()[x, y].GetChannel(0) +
                ", size: " + image.Width + "x" + image.Height);
            Debug.Log("settings: " + image.Settings);
            */

            //image.ToBitmap();
            //depthTexture = depthListener.GetLast();
            //depthTexture.LoadRawTextureData(depthListener.GetLast());
            //ImageConversion.
            //byte[] data = depthListener.GetLast();
            //int offset = 12;
            //byte[] data_with_offset = new byte[data.Length-offset];
            //Array.Copy(data, offset, data_with_offset, 0, data.Length-offset);
            //depthTexture.LoadImage(depthListener.GetLast());
            //depthTexture.LoadRawTextureData(image.ToBitmap().);
            //depthTexture.Apply();


            //image.Format = ImageMagick.MagickFormat.Gray;
            //ImageMagick.MagickFormat.

            //Debug.Log(image.FormatInfo);
            /*
            Debug.Log("Formatted depth: " + image.Depth +
                ", channels: " + image.ChannelCount + 
                ", datasize: " + image.ToByteArray().Length +
                ", pix: " + image.GetPixels()[x, y].GetChannel(0) +
                ", size: " + image.Width + "x" + image.Height);

            Debug.Log("Converted byte: " + 
                Disp(image.ToByteArray()[loc]) + ", " + 
                Disp(image.ToByteArray()[loc+1]));
            Debug.Log("Pixel: " + image.GetPixels()[480, 270].GetChannel(0));
            */

            //depthTexture.LoadRawTextureData(image.ToByteArray());

            depthTexture.Apply();
            //a.Dispose();
            //Debug.Log(depthTexture.GetPixel(500, 200));
        }

    }

    string Disp(byte b)
    {
        return Convert.ToString(b, 2).PadLeft(8, '0');
    }


    void OnRenderObject()
    {

        material.SetTexture("_MainTex", depthTexture);
        material.SetTexture("_ColorTex", colorTexture);
        material.SetPass(0);
        //Transform t = this.transform * kinect_offset;
        //m = Matrix4x4.TRS(this.transform.position + kinect_offset.position, 
        //    this.transform.rotation, this.transform.localScale);
        m = Matrix4x4.TRS(kinect_offset.position,
                          kinect_offset.rotation, kinect_offset.localScale);
        material.SetMatrix("transformationMatrix", m);


        Graphics.DrawProcedural(MeshTopology.Points, width * height, 1);
 
    }

}
