using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Text))]
public class FPSCounter : MonoBehaviour
{
    const string display = "{0} FPS";
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (Time.frameCount % 20 != 0) return;

        float FPS = (1.0f / Time.smoothDeltaTime);
        text.text = ((int)FPS).ToString();
    }
}
