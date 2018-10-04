using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using RosSharp.RosBridgeClient;
using ImageMagick;

public class KinectView : MonoBehaviour {

    public RosSharp.RosBridgeClient.ImageListener colorListener;
    public RosSharp.RosBridgeClient.ImageListener depthListener;
    public RosSharp.RosBridgeClient.ImageRawListener depthListenerRaw;

    public string depth_topic;

    //private WebsocketClient wsc;
    string depthTopic;
    string colorTopic;
    //int framerate = 100;
    public string compression = "none"; //"png" is the other option, haven't tried it yet though
    string depthMessage;
    string colorMessage;
    

    public Material Material;
    Texture2D depthTexture;
    Texture2D colorTexture;

    //int width = 512;
    //int height = 424;
    int width = 1920/2;
    int height = 1080/2;

    Matrix4x4 m;

    // Use this for initialization
    void Start()
    {
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
            long loc = 519360;
            int x = 480;
            int y = 270;
            byte[] raw = depthListener.GetLast();
            //Debug.Log("Raw length: " + raw.Length);
            //Debug.Log("Raw: " + raw[0] + ", " + raw[1]);
            //new ImageMagick.MagickReadSettings
            MagickReadSettings settings = new MagickReadSettings();
            //settings.
            ImageMagick.MagickImage image = new ImageMagick.MagickImage(raw, settings);
            
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

            
            image.Format = ImageMagick.MagickFormat.Gray;
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

            depthTexture.LoadRawTextureData(image.ToByteArray());
            
            depthTexture.Apply();
            //Debug.Log(depthTexture.GetPixel(500, 200));
        }
        if(depthListenerRaw.HasNew())
        {
            long loc = 519360;
            int x = 480;
            int y = 270;

            byte[] raw = depthListenerRaw.GetLast();
            Debug.Log("Raw size: " + raw.Length);
            //raw[519361] = 0;
            Debug.Log("Raw Listener Bytes: " +
                Disp(raw[loc]) + ", " + Disp(raw[loc + 1]));
   
            depthTexture.LoadRawTextureData(raw);
            depthTexture.Apply();


  
            MagickReadSettings settings = new MagickReadSettings();
            settings.Width = 960;
            settings.Height = 540;
            settings.Format = MagickFormat.Gray;
            settings.SetDefine("Depth", "16");

           
            ImageMagick.MagickImage image = new ImageMagick.MagickImage(raw, settings);
            //image.Read(raw, settings);
            /*ImageMagick.MagickImage image = new ImageMagick.MagickImage();
            var width = 960;
            var height = 540;
            var storageType = StorageType.Short;
            var mapping = "R";
            var pixelStorageSettings = new PixelStorageSettings(width, height, storageType, mapping);
            image.ReadPixels(raw, pixelStorageSettings);
            */
            Debug.Log("Raw depth: " + image.Depth +
                       ", channels: " + image.ChannelCount +
                       ", datasize: " + image.ToByteArray().Length +
                       ", pix: " + image.GetPixels()[x, y].GetChannel(0) +
                        ", size: " + image.Width + "x" + image.Height);
            Debug.Log("Converted byte: " +
                Disp(image.ToByteArray()[loc]) + ", " +
                Disp(image.ToByteArray()[loc + 1]));

            byte[] tmp = image.ToByteArray();
            tmp[loc] = raw[loc];
            ImageMagick.MagickImage img2 = new ImageMagick.MagickImage(tmp, settings);
            Debug.Log("Reread byte: " +
                Disp(img2.ToByteArray()[loc]) + ", " +
                Disp(img2.ToByteArray()[loc + 1]));

            Debug.Log("Pixel: " + image.GetPixels()[x, y].GetChannel(0));
        }

    }

    string Disp(byte b)
    {
        return Convert.ToString(b, 2).PadLeft(8, '0');
    }


    void OnRenderObject()
    {

        Material.SetTexture("_MainTex", depthTexture);
        Material.SetTexture("_ColorTex", colorTexture);
        Material.SetPass(0);

        m = Matrix4x4.TRS(this.transform.position, this.transform.rotation, this.transform.localScale);
        Material.SetMatrix("transformationMatrix", m);


        Graphics.DrawProcedural(MeshTopology.Points, width * height, 1);
 
    }

}
