using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Pupil {
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
		private bool _cameraSet;

		void Start () {
			Invoke("SetCamera", 1f);
		}

		void Update () {
			if (_cameraSet) {
				_camera.AutoAdjustIPD();
				_camera.AutoAdjustDepthOfField();
				if (_debug) 
					_camera.DrawViewLines();
			}
		}

		public void SetCamera() {
			_minDistance = PupilDataHolder.minDistance;
			_maxDistance = PupilDataHolder.maxDistance;		
			_maxDistanceIPD = PupilDataHolder.maxIPD;
			_minDistanceIPD = PupilDataHolder.minIPD;			
			
			_camera = new PupilCamera();		 	

			_camera.SetMinDistanceIPD(_minDistance, _minDistanceIPD);
			_camera.SetMaxDistanceIPD(_maxDistance, _maxDistanceIPD);
			_cameraSet = true;
		}

		public void SetMinDistanceIPD(float ipd) {
			PupilDataHolder.minIPD = ipd;			
			_camera.SetMinDistanceIPD(PupilDataHolder.minDistance, PupilDataHolder.minIPD);			
		}

		public void SetMaxDistanceIPD(float ipd) {
			PupilDataHolder.maxIPD = ipd;
			_camera.SetMaxDistanceIPD(PupilDataHolder.maxDistance, PupilDataHolder.maxIPD);						
		}

		public void SetMinDistance() {
			PupilDataHolder.minDistance = _camera.GetDistanceToGameObject(_camera.FindNearest());			
			_camera.SetMinDistanceIPD(PupilDataHolder.minDistance, PupilDataHolder.minIPD);
		}

		public void SetMinDistance(int ignoreLayer) {
			PupilDataHolder.minDistance = _camera.GetDistanceToGameObject(_camera.FindNearest(ignoreLayer));			
			_camera.SetMinDistanceIPD(PupilDataHolder.minDistance, PupilDataHolder.minIPD);			
		}

		public void SetMaxDistance() {
			PupilDataHolder.maxDistance = _camera.GetDistanceToGameObject(_camera.FindNearest());
			_camera.SetMaxDistanceIPD(PupilDataHolder.maxDistance, PupilDataHolder.maxIPD);
		}

		public void SetMaxDistance(int ignoreLayer) {
			PupilDataHolder.maxDistance = _camera.GetDistanceToGameObject(_camera.FindNearest(ignoreLayer));			
			_camera.SetMaxDistanceIPD(PupilDataHolder.maxDistance, PupilDataHolder.maxIPD);			
		}				
	}
}