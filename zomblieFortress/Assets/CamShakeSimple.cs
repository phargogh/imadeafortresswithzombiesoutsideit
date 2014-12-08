//from http://newbquest.com/2014/06/the-art-of-screenshake-with-unity-2d-script/
using UnityEngine;
using System.Collections;

public class CamShakeSimple : MonoBehaviour 
{
	
	Vector3 originalCameraPosition;
	
	float shakeAmt = 0;
	
	public Camera mainCamera;

	public void CallShake(float x) 
	{
		this.originalCameraPosition = mainCamera.transform.position;
		shakeAmt = x * .0025f;
		InvokeRepeating("CameraShake", 0, .01f);
		Invoke("StopShaking", 1f);
		
	}

	public void CameraShake(float shakeAmt)
	{

		if(shakeAmt>0) 
		{
			float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
			Vector3 pp = mainCamera.transform.position;
			pp.y+= quakeAmt; // can also add to x and/or z
			mainCamera.transform.position = pp;
		}

	}
	
	void  StopShaking()
	{
		CancelInvoke("CameraShake");
		mainCamera.transform.position = this.originalCameraPosition;
	}
	
}