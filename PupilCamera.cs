using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pupil {
	/*
	PupilCamera

	Functionality for adjusting the Unity camera settings to account for dynamically changing IPD.
	*/ 
	public class PupilCamera {
		//Difference between IPD at variable lengths 		
		private float _ipd;
		private Transform _camera;
		private Camera[] _children;
		private float _minDistance;
		private float _maxDistance;
		private float _minDistanceIPD;
		private float _maxDistanceIPD;	
		private bool _autoAdjustWarnings;	

		
		public PupilCamera() {
			if (GameObject.Find("PupilInitializer") == null) {
				Debug.LogError("Error: No PupilInitializer GameObject found. Please add one to the hierarchy to properly load your VR device.");
			}	

			_camera = GameObject.FindGameObjectWithTag("MainCamera").transform;	

			if (_camera == null) {
				Debug.LogError("Error: Camera not set. Please ensure that the PupilCameraRig is in the hierarchy and that there is only on camera with the MainCamera tag in the scene.");
			}

			_children = _camera.GetComponentsInChildren<Camera>();
		}
		
		public void ChangeIpd(float update) {
			_ipd += update;
		}

		public float GetIPD() {
			return _ipd;
		}

		public float GetDistanceToGameObject(GameObject obj) {
			var distance = Vector3.Distance(_camera.position, obj.transform.position);
			return distance;
		}

		public void SetMinDistanceIPD(float distance, float ipd) {
			_minDistance = distance;
			_minDistanceIPD = ipd;
		}

		public void SetMaxDistanceIPD(float distance, float ipd) {
			_maxDistance = distance;
			_maxDistanceIPD = ipd;
		}

		private GameObject FindNearest() {
			RaycastHit hit;
			if (Physics.SphereCast(_camera.localPosition, 10f, Vector3.forward, out hit, _maxDistance)) {
				return hit.transform.gameObject;
			}
			return _camera.gameObject;
		}

		//TODO: Lines do not follow rotation vectors
		public void DrawViewLines() {
			Debug.DrawLine(_children[0].transform.localPosition, Vector3.forward, Color.red);
			Debug.DrawLine(_children[1].transform.localPosition, Vector3.forward, Color.green);
		}

		public void AutoAdjustIPD() {
			if (!_autoAdjustWarnings && (_maxDistance == 0f || _minDistance == 0f)) {
				Debug.LogWarning("One or more distance variables are 0. Are you sure you want to do this?");
				_autoAdjustWarnings = true;
			}

			var nearest = FindNearest();
			//Default to max distance IPD
			_ipd = _maxDistanceIPD;
			var distance = GetDistanceToGameObject(nearest);
			if (distance >= _minDistance) {
				_ipd = _minDistanceIPD;
			}

			//Left
			_children[0].transform.localRotation = Quaternion.Euler(_children[0].transform.localPosition.x, 
																	_ipd,
																	_children[0].transform.localPosition.z);
			
			//Right
			_children[1].transform.localRotation = Quaternion.Euler(_children[0].transform.localPosition.x, 
																	_ipd, 
																	_children[0].transform.localPosition.z);
		}
	}

}