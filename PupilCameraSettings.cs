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

		void Start () {
			_minDistance = PupilDataHolder.minDistance;
			_maxDistance = PupilDataHolder.maxDistance;		
			_maxDistanceIPD = PupilDataHolder.maxIPD;
			_minDistanceIPD = PupilDataHolder.minIPD;

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

		public void SetMaxDistance() {
			PupilDataHolder.minDistance = _camera.GetDistanceToGameObject(_camera.FindNearest());
			_camera.SetMaxDistanceIPD(PupilDataHolder.maxDistance, PupilDataHolder.maxIPD);
		}
		
	}
}