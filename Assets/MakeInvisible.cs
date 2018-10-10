using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeInvisible : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SetVisibility(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetVisibility(bool vis)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = vis;
        }
    }
}
