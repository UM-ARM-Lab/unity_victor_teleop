using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using OpenCvSharp;

public class SceneLoader : MonoBehaviour
{
    public Text text;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Menu" && text != null)
        {
            Mat mat = new Mat(100, 100, MatType.CV_8UC4, new Scalar(255, 0, 0, 255));

            if(mat != null){
				Vec3b color = mat.Get<Vec3b>(0, 0);
				int red = color[0];
				text.text = "(OpenCV loaded)";
            }
            else 
            {
                text.text = "(OpenCV could not be loaded)";
            }
        }
    }


    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void LoadFaceDetection()
    {
        SceneManager.LoadScene("MasterScene", LoadSceneMode.Single);
        SceneManager.LoadScene("FaceDetection", LoadSceneMode.Additive);
    }

	public void LoadMotionDetection()
	{
		SceneManager.LoadScene("MasterScene", LoadSceneMode.Single);
		SceneManager.LoadScene("MotionDetection", LoadSceneMode.Additive);
	}

    public void LoadLineFollowingRobot()
    {
		SceneManager.LoadScene("MasterScene", LoadSceneMode.Single);
        SceneManager.LoadScene("LineFollowingRobot",LoadSceneMode.Additive);
    }

	public void LoadThresholds()
	{
		SceneManager.LoadScene("MasterScene", LoadSceneMode.Single);
		SceneManager.LoadScene("Thresholds", LoadSceneMode.Additive);
	}

	public void LoadVideoReadWrite()
	{
		SceneManager.LoadScene("MasterScene", LoadSceneMode.Single);
		SceneManager.LoadScene("VideoReadWrite", LoadSceneMode.Additive);
	}

	public void LoadSimpleBlobDetection()
	{
		SceneManager.LoadScene("MasterScene", LoadSceneMode.Single);
		SceneManager.LoadScene("SimpleBlobDetection", LoadSceneMode.Additive);
	}

	public void LoadFAST()
	{
		SceneManager.LoadScene("MasterScene", LoadSceneMode.Single);
		SceneManager.LoadScene("FAST", LoadSceneMode.Additive);
	}

	public void LoadHoughLines()
	{
		SceneManager.LoadScene("MasterScene", LoadSceneMode.Single);
		SceneManager.LoadScene("HoughLines", LoadSceneMode.Additive);
	}

    public void Quit()
    {
        Application.Quit();
    }
}
