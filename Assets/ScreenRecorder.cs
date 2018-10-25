using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProMovieCapture;

public class ScreenRecorder : MonoBehaviour {

    public CaptureBase movieCapture;
    public RosSharp.RosBridgeClient.RecordButtonPublisher recordButtonPublisher;

    private bool recording;
  

	// Use this for initialization
	void Start () {
        createMovieCapture();
        recording = false;
	}

    void createMovieCapture()
    {
    }

    void startRecording()
    {
        movieCapture.StartCapture();
        recording = true;
    }

    void stopRecording()
    {
        movieCapture.StopCapture();
        recording = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(!recording && recordButtonPublisher.recording)
        {
            startRecording();
        }
        
        if(recording && !recordButtonPublisher.recording)
        {
            stopRecording();
        }
	}
}
