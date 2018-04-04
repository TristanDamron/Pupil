using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.PostProcessing;

namespace Pupil {
	/*
	PupilCamera

	Functionality for adjusting the Unity camera settings to account for dynamically changing IPD and depth of field.
	*/
	public class PupilCamera {
		private float _ipd;
		private Transform _camera;
		private float _minDistance;
		private float _maxDistance;
		private float _minDistanceIPD;
		private float _maxDistanceIPD;	
		private bool _autoAdjustWarnings;	
		private PostProcessingBehaviour _behaviourLeft;
		private PostProcessingBehaviour _behaviourRight;
		private GameObject _nearest;

		public PupilCamera() {
			if (GameObject.Find("PupilInitializer") == null) {
				Debug.LogError("Error: No PupilInitializer GameObject found. Please add one to the hierarchy to properly load your VR device.");
			}	

			_camera = GameObject.Find("PupilCameraRig").transform;
			_nearest = _camera.gameObject;

			if (_camera == null) {
				Debug.LogError("Error: Camera not set. Please ensure that the PupilCameraRig is in the hierarchy and that there is only on camera with the MainCamera tag in the scene.");
			} 

			_behaviourLeft = _camera.GetComponentsInChildren<PostProcessingBehaviour>()[0];

			if (_behaviourLeft == null) {
				Debug.LogError("Error: No post processing behaviour found on the left camera.");			
			}
		
			_behaviourRight = _camera.GetComponentsInChildren<PostProcessingBehaviour>()[1];

			if (_behaviourRight == null) {
				Debug.LogError("Error: No post processing behaviour found on the right camera.");			
			}
		}
		
		public float GetDistanceToGameObject(GameObject obj) {
			var distance = Vector3.Distance(_camera.position, obj.transform.position);
			return distance;
		}

		public float GetDistanceToGameObject(GameObject obj, GameObject from) {
			var distance = Vector3.Distance(from.transform.localPosition, obj.transform.position);
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

		public GameObject FindNearest() {
			RaycastHit hit;
			var rot = InputTracking.GetLocalRotation(XRNode.Head);
			if (Physics.Raycast(_camera.position, rot * _camera.forward, out hit)) {
				_nearest = hit.transform.gameObject;
				return hit.transform.gameObject;
			}

			return _nearest;
		}

		public GameObject FindNearest(int ignoreLayer) {
			RaycastHit hit;
			var rot = InputTracking.GetLocalRotation(XRNode.Head);
			if (Physics.Raycast(_camera.position, rot * _camera.forward, out hit)) {
				if (hit.transform.gameObject.layer != ignoreLayer) {
					_nearest = hit.transform.gameObject;
					return hit.transform.gameObject;
				}
			}

			return _nearest;
		}

		public void DrawViewLines() {
			Debug.DrawLine(_camera.GetChild(0).transform.forward * _maxDistance, _camera.GetChild(0).forward, Color.red);
			Debug.DrawLine(_camera.GetChild(1).transform.forward * _maxDistance, _camera.GetChild(1).forward, Color.green);
		}

		public void AutoAdjustDepthOfField() {
			if (_behaviourLeft.profile == null || _behaviourRight.profile == null) {
				Debug.LogError("Error: Cannot adjust depth of field. PostProcessing profile(s) not found. Please attach a profile to the cameras' PostProcessingBehaviour.");
			} else {
				var nearest = FindNearest();
				var distance = GetDistanceToGameObject(nearest, _camera.gameObject);
				var leftSettings = _behaviourLeft.profile.depthOfField.settings;
				var rightSettings = _behaviourRight.profile.depthOfField.settings;

				var lerp = 0f;
				if (nearest != _camera.gameObject || distance <= _maxDistanceIPD) {
					lerp = Mathf.Lerp(leftSettings.focusDistance, distance, Time.deltaTime * 30f);	
				} 
				
				if (nearest == _camera.gameObject) {
					lerp = Mathf.Lerp(leftSettings.focusDistance, 100f, Time.deltaTime);
				}

				leftSettings.focusDistance = lerp;
				rightSettings.focusDistance = lerp;				

				_behaviourLeft.profile.depthOfField.settings = leftSettings;
				_behaviourRight.profile.depthOfField.settings = rightSettings;
			}
			
		}

		public void AutoAdjustIPD() {
			if (!_autoAdjustWarnings && (_maxDistance == 0f || _minDistance == 0f)) {
				Debug.LogWarning("One or more distance variables are 0. Are you sure you want to do this?");
				_autoAdjustWarnings = true;
			}

			var nearest = FindNearest();
			//Default to max distance IPD
			_ipd = _maxDistanceIPD;
			if (nearest != _camera.gameObject) {
				var distance = GetDistanceToGameObject(nearest);
				if (distance < _maxDistance) {
					_ipd = _minDistanceIPD;
				}
			}

			//Left
			Quaternion leftSlerp = Quaternion.Euler(_camera.GetChild(0).transform.localPosition.x, 
													_ipd,
													_camera.GetChild(0).transform.localPosition.z);

			_camera.GetChild(0).transform.localRotation = Quaternion.Slerp(_camera.GetChild(0).transform.localRotation, leftSlerp, Time.deltaTime);
			_camera.GetChild(0).transform.localPosition = Vector3.Lerp(_camera.GetChild(0).transform.localPosition, 
																		new Vector3(_ipd, _camera.GetChild(0).transform.localPosition.y, _camera.GetChild(0).transform.localPosition.z), 
																		Time.deltaTime);

			//Right
			Quaternion rightSlerp = Quaternion.Euler(_camera.GetChild(1).transform.localPosition.x, 
													-_ipd, 
													_camera.GetChild(1).transform.localPosition.z);

			_camera.GetChild(1).transform.localRotation = Quaternion.Slerp(_camera.GetChild(1).transform.localRotation, rightSlerp, Time.deltaTime); 
			_camera.GetChild(1).transform.localPosition = Vector3.Lerp(_camera.GetChild(1).transform.localPosition, 
																		new Vector3(-_ipd, _camera.GetChild(1).transform.localPosition.y, _camera.GetChild(1).transform.localPosition.z), 
																		Time.deltaTime);			

		}
	}
}