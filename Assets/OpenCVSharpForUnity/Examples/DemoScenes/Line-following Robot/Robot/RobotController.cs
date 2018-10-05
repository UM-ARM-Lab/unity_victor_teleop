using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour {

    public float rotationSpeed = 60f;
    public float linearSpeed = 0.8f;

    public void MoveForward()
    {
		transform.position += transform.forward * linearSpeed * Time.deltaTime;
    }

	public void MoveBackward()
	{
		transform.position += -transform.forward * linearSpeed * Time.deltaTime;
	}

    public void RotateLeft()
    {
        transform.RotateAround(transform.position + transform.forward * 0.1f, transform.up, -rotationSpeed * Time.deltaTime);
    }

    public void RotateRight()
    {
        transform.RotateAround(transform.position + transform.forward * 0.1f, transform.up, rotationSpeed * Time.deltaTime);
    }
}
