using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Pupil
{
    [RequireComponent(typeof(PopupWarning))]
    public class PupilCameraSettings : MonoBehaviour
    {
        [SerializeField]
        private PupilCamera _camera;
        [SerializeField]
        private float _left;
        [SerializeField]
        private float _right;
        [SerializeField]
        private float _minDistanceIPD;
        [SerializeField]
        private float _maxDistanceIPD;
        [SerializeField]
        private float _maxDistance;
        [SerializeField]
        private string _red;
        [SerializeField]
        private string _blue;
        [SerializeField]
        private string _green;
        [SerializeField]
        private string _yellow;
        [SerializeField]
        private bool _debug;
        private bool _cameraSet;

        void Update()
        {
			SetCamera();

            if (_cameraSet)
            {
                _camera.AutoAdjustIPD();
                _camera.AutoAdjustDepthOfField();
                if (_debug)
                    _camera.DrawViewLines();
            }
        }

        public void SetCamera()
        {
            if (!_cameraSet)
            {
                _left = PupilDataHolder.left;
                _right = PupilDataHolder.right;
                _maxDistance = PupilDataHolder.maxDistance;
                _maxDistanceIPD = PupilDataHolder.maxIPD;
                _minDistanceIPD = PupilDataHolder.minIPD;
                _red = PupilDataHolder.red;
                _blue = PupilDataHolder.blue;
                _green = PupilDataHolder.green;
                _yellow = PupilDataHolder.yellow;

                _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PupilCamera>();

				if (_camera) {
	                _camera.SetMinDistanceIPD(_minDistanceIPD);
	                _camera.SetMaxDistanceIPD(_maxDistance, _maxDistanceIPD);
	                _cameraSet = true; 
				}
           }
        }

        public void SetMinDistanceIPD(float ipd)
        {
            PupilDataHolder.minIPD = ipd;
            _camera.SetMinDistanceIPD(PupilDataHolder.minIPD);
        }

        public void SetMaxDistanceIPD(float ipd)
        {
            PupilDataHolder.maxIPD = ipd;
            _camera.SetMaxDistanceIPD(PupilDataHolder.maxDistance, PupilDataHolder.maxIPD);
        }

        public void SetMaxDistance()
        {
            PupilDataHolder.maxDistance = _camera.GetDistanceToGameObject(_camera.FindNearest());
            _camera.SetMaxDistanceIPD(PupilDataHolder.maxDistance, PupilDataHolder.maxIPD);
        }

        public void SetMaxDistance(int ignoreLayer)
        {
            PupilDataHolder.maxDistance = _camera.GetDistanceToGameObject(_camera.FindNearest(ignoreLayer));
            _camera.SetMaxDistanceIPD(PupilDataHolder.maxDistance, PupilDataHolder.maxIPD);
        }
    }
}