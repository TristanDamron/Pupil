using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.PostProcessing;

namespace Pupil {
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
		private Transform _left;
		private Transform _right;
		private GameObject _nearest;

		public PupilCamera() {
			if (GameObject.Find("PupilInitializer") == null) {
				Debug.LogError("Error: No PupilInitializer GameObject found. Please add one to the hierarchy to properly load your VR device.");
			}	

			FindCameraRig();

			if (_camera == null) {
				Debug.LogError("Error: Camera not set. Please ensure that the PupilCameraRig is in the hierarchy and that there is only on camera with the MainCamera tag in the scene.");
			} 

			_behaviourLeft = _camera.GetComponentsInChildren<PostProcessingBehaviour>()[0];

			if (_behaviourLeft == null) {
				Debug.LogError("Error: No post processing behaviour found on the left camera.");			
			}

			_left = _behaviourLeft.gameObject.transform.parent;
		
			_behaviourRight = _camera.GetComponentsInChildren<PostProcessingBehaviour>()[1];

			if (_behaviourRight == null) {
				Debug.LogError("Error: No post processing behaviour found on the right camera.");			
			}

			_right = _behaviourRight.gameObject.transform.parent;
		}

		public void FindCameraRig() {			
			_camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
			_nearest = _camera.gameObject;			
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
			#if UNITY_EDITOR
			rot = _camera.transform.localRotation;
			#endif

			if (Physics.SphereCast(_camera.position, 1f, rot * _camera.forward, out hit)) {
				_nearest = hit.transform.gameObject;
				return hit.transform.gameObject;
			}

			return _camera.gameObject;
		}

		public GameObject FindNearest(int ignoreLayer) {
			RaycastHit hit;

			var rot = InputTracking.GetLocalRotation(XRNode.Head);
			#if UNITY_EDITOR
			rot = _camera.transform.localRotation;
			#endif

			if (Physics.SphereCast(_camera.position, 1f, rot * _camera.forward, out hit)) {
				if (hit.transform.gameObject.layer != ignoreLayer) {
					_nearest = hit.transform.gameObject;
					return hit.transform.gameObject;
				}
			}

			return _camera.gameObject;
		}

		public void DrawViewLines() {
			Debug.DrawLine(_left.position, _left.position + _left.forward * _maxDistance, Color.red);
			Debug.DrawLine(_right.position, _right.position + _right.forward * _maxDistance, Color.green);
		}

		public void AutoAdjustDepthOfField() {
			if (_behaviourLeft.profile == null || _behaviourRight.profile == null) {
				Debug.LogError("Error: Cannot adjust depth of field. PostProcessing profile(s) not found. Please attach a profile to the cameras' PostProcessingBehaviour.");
			} else {	
				_nearest = FindNearest();			
				var distance = GetDistanceToGameObject(_nearest, _camera.gameObject);
				var leftSettings = _behaviourLeft.profile.depthOfField.settings;
				var rightSettings = _behaviourRight.profile.depthOfField.settings;

				var lerp = 0f;
				if (_nearest != _camera.gameObject || distance <= _maxDistanceIPD) {
					lerp = Mathf.Lerp(leftSettings.focusDistance, distance, Time.deltaTime * 30f);	
				} 
				
				if (_nearest == _camera.gameObject) {
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
			
			_nearest = FindNearest();		

			//Default to max distance IPD
			_ipd = _maxDistanceIPD;
			if (_nearest != _camera.gameObject) {
				var distance = GetDistanceToGameObject(_nearest);
				if (distance < _maxDistance) {
					_ipd = _minDistanceIPD;
				}
			}

			//Left
			Quaternion leftSlerp = Quaternion.Euler(_left.localPosition.x, 
													_ipd,
													_left.localPosition.z);

			_left.localRotation = Quaternion.Slerp(_left.localRotation, leftSlerp, Time.deltaTime * 3f);
			_left.localPosition = Vector3.Lerp(_left.localPosition, 
												new Vector3(_ipd, _left.localPosition.y, _left.localPosition.z), 
												Time.deltaTime * 3f);

			//Right
			Quaternion rightSlerp = Quaternion.Euler(_right.localPosition.x, 
													-_ipd, 
													_right.localPosition.z);

			_right.localRotation = Quaternion.Slerp(_right.localRotation, rightSlerp, Time.deltaTime * 3f); 
			_right.localPosition = Vector3.Lerp(_right.localPosition, 
												new Vector3(-_ipd, _right.localPosition.y, _right.localPosition.z), 
												Time.deltaTime * 3f);	
		}
	}
}