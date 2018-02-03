using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Pupil;

public class StereoScaling : MonoBehaviour {
	private PupilCamera _camera;

	void Start () {
		_camera = new PupilCamera();		 	

		//Set Pupil Settings
		_camera.SetMinDistanceIPD(1f, -2f);
		_camera.SetMaxDistanceIPD(10f, 0f);
	}

	void Update () {
		_camera.AutoAdjustIPD();
		_camera.DrawViewLines();
	}
}
