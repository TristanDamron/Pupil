using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Pupil;

public class PupilCameraSettings : MonoBehaviour {
	private PupilCamera _camera;
	[SerializeField]
	private float _minDistanceIPD;
	[SerializeField]
	private float _minDistance;
	[SerializeField]
	private float _maxDistanceIPD;
	[SerializeField]
	private float _maxDistance;
	[SerializeField]
	private bool _debug;

	void Start () {
		_camera = new PupilCamera();		 	

		//Set Pupil Settings
		_camera.SetMinDistanceIPD(_minDistance, _minDistanceIPD);
		_camera.SetMaxDistanceIPD(_maxDistance, _maxDistanceIPD);
	}

	void Update () {
		_camera.AutoAdjustIPD();
		_camera.AutoAdjustDepthOfField();
		if (_debug) 
			_camera.DrawViewLines();
	}
}
