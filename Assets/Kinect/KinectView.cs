using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using OpenCvSharp;
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
        //colorTexture = new Texture2D(2, 2);
        colorTexture = new Texture2D(width, height, TextureFormat.RGB24, false);



        //InvokeRepeating("UpdateTexture", 0.1f, 0.1f);
    }

    void Update()
    {
        
        if (colorListener.HasNew())
        {
            //Debug.Log("color texture updated");
            UnityEngine.Profiling.Profiler.BeginSample("Apply Color");
            colorTexture.LoadImage(colorListener.GetLast());
            colorTexture.Apply();
            UnityEngine.Profiling.Profiler.EndSample();

            /*
            byte[] compressedImage = colorListener.GetLast();
            Mat mat = Mat.ImDecode(compressedImage);
            Vec3b[] decompressed = new Vec3b[width * height];
            //int[] decompressed = new int[width * height];
            //mat.GetArray(0, 0, decompressed);
            //NativeArray<int> tmparr = new NativeArray<int>(width * height, Allocator.Temp);
            byte[] tmp = new byte[decompressed.Length * 3];
            //byte[] a = mat.ImageDat
            mat.
            //Buffer.BlockCopy(decompressed, 0, tmp, 0, tmp.Length);
            //mat.Total
            //mat.GetArray(0, 0, tmp);
            //tmparr.CopyFrom(decompressed);
            colorTexture.LoadRawTextureData(tmp);
            //colorTexture.LoadRawTextureData(decompressed);
            colorTexture.Apply();
            //tmparr.Dispose();
            Debug.Log("channels: " + mat.Channels() + ", " + mat.Depth() + ", " 
                + mat.Total() + ", " + mat.ElemSize());
                */
            



        }
        if (depthListener.HasNew())
        {
            UnityEngine.Profiling.Profiler.BeginSample("Copy Depth");
            depthTexture.LoadRawTextureData<short>(depthListener.GetLast());
            depthTexture.Apply();
            UnityEngine.Profiling.Profiler.EndSample();
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
        //m = Matrix4x4.TRS(kinect_offset.position,
        //                  kinect_offset.rotation, kinect_offset.localScale);
        m = Matrix4x4.TRS(kinect_offset.position,
                          kinect_offset.rotation, new UnityEngine.Vector3(1,1,1));
        material.SetMatrix("transformationMatrix", m);


        Graphics.DrawProcedural(MeshTopology.Points, width * height, 1);
 
    }

}
