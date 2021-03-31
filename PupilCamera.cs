using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Rendering.PostProcessing;

namespace Pupil
{
    public class PupilCamera : MonoBehaviour
    {
        private float _ipd;
        private Transform _camera;
        [SerializeField]
        private Transform _left;
        [SerializeField]
        private Transform _right;
        private float _maxDistance;
        private float _minDistanceIPD;
        private float _maxDistanceIPD;
        private bool _autoAdjustWarnings;
        private PostProcessVolume _leftVolume;
        private PostProcessVolume _rightVolume;
        private DepthOfField _dof;
        private DepthOfField _rightDOF;
        [SerializeField]
        private GameObject _nearest;

        public void Start()
        {
            if (GameObject.Find("PupilInitializer") == null)
            {
                Debug.LogError("Error: No PupilInitializer GameObject found. Please add one to the hierarchy to properly load your VR device.");
            }

            // FindCameraRig();
            _camera = transform;

            // @todo(tdamron): Cover case for two MainCameras in the scene
            /*
            if (_camera == null)
            {
                Debug.LogError("Error: Camera not set. Please ensure that the PupilCameraRig is in the hierarchy and that there is only on camera with the MainCamera tag in the scene.");
            }
            */

            _leftVolume = _left.GetComponent<PostProcessVolume>();

            if (_leftVolume == null)
            {
                Debug.LogError("Error: No post processing behaviour found on the left camera.");
            }

            _rightVolume = _right.GetComponent<PostProcessVolume>();

            if (_rightVolume == null)
            {
                Debug.LogError("Error: No post processing behaviour found on the right camera.");
            }

            _leftVolume.sharedProfile.TryGetSettings<DepthOfField>(out _dof);
        }

        // DEPRECATED
        public void FindCameraRig()
        {
            _camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
            _nearest = _camera.gameObject;
        }

        public float GetDistanceToGameObject(GameObject obj)
        {
            var distance = Vector3.Distance(_camera.position, obj.transform.position);
            return distance;
        }

        public float GetDistanceToGameObject(GameObject obj, GameObject from)
        {
            var distance = Vector3.Distance(from.transform.localPosition, obj.transform.position);
            return distance;
        }

        public void SetMinDistanceIPD(float ipd)
        {
            _minDistanceIPD = ipd;
        }

        public void SetMaxDistanceIPD(float distance, float ipd)
        {
            _maxDistance = distance;
            _maxDistanceIPD = ipd;
        }

        public GameObject FindNearest()
        {
            RaycastHit hit;

            var rot = InputTracking.GetLocalRotation(XRNode.Head);
#if UNITY_EDITOR
            rot = _camera.transform.localRotation;
#endif

            if (Physics.SphereCast(_camera.position, 1f, rot * _camera.forward, out hit))
            {
                _nearest = hit.transform.gameObject;
                return hit.transform.gameObject;
            }

            return _camera.gameObject;
        }

        public GameObject FindNearest(int ignoreLayer)
        {
            RaycastHit hit;

            var rot = InputTracking.GetLocalRotation(XRNode.Head);
#if UNITY_EDITOR
            rot = _camera.transform.localRotation;
#endif

            if (Physics.SphereCast(_camera.position, 1f, rot * _camera.forward, out hit))
            {
                if (hit.transform.gameObject.layer != ignoreLayer)
                {
                    _nearest = hit.transform.gameObject;
                    return hit.transform.gameObject;
                }
            }

            return _camera.gameObject;
        }

        public void DrawViewLines()
        {
            Debug.DrawLine(_left.position, _left.position + _left.forward * _maxDistance, Color.red);
            Debug.DrawLine(_right.position, _right.position + _right.forward * _maxDistance, Color.green);
        }

        public void AutoAdjustDepthOfField()
        {
            if (_leftVolume == null || _rightVolume == null)
            {
                Debug.LogError("Error: Cannot adjust depth of field. PostProcessing profile(s) not found. Please attach a profile to the cameras' PostProcessingBehaviour.");
            }
            else
            {
                if (_leftVolume.sharedProfile.TryGetSettings<DepthOfField>(out var dof)) {
                    dof.SetAllOverridesTo(true);

                    _nearest = FindNearest();
                    var distance = GetDistanceToGameObject(_nearest, _camera.gameObject);


                    var lerp = 0f;
                    if (_nearest != _camera.gameObject)
                    {
                        lerp = Mathf.Lerp(dof.focusDistance, distance, Time.deltaTime * 30f);
                    }

                    if (_nearest == _camera.gameObject)
                    {
                        lerp = Mathf.Lerp(dof.focusDistance, 100f, Time.deltaTime);
                    }

                    dof.focusDistance.value = lerp;
                }
            }
        }

        public void AutoAdjustIPD()
        {
            if (!_autoAdjustWarnings && _maxDistance == 0f)
            {
                Debug.LogWarning("One or more distance variables are 0. Are you sure you want to do this?");
                _autoAdjustWarnings = true;
            }

            _nearest = FindNearest();

            //Default to max distance IPD
            _ipd = _minDistanceIPD;
            if (_nearest != _camera.gameObject)
            {
                var distance = GetDistanceToGameObject(_nearest);
                if (distance > _maxDistance)
                {
                    _ipd = _maxDistanceIPD;
                }
            }

            _left.parent.localPosition = Vector3.Lerp(_left.parent.localPosition,
                                                new Vector3(-_ipd + PupilDataHolder.left, _left.parent.localPosition.y, _left.parent.localPosition.z),
                                                Time.deltaTime * 3f);

            _right.parent.localPosition = Vector3.Lerp(_right.parent.localPosition,
                                                new Vector3(_ipd + PupilDataHolder.right, _right.parent.localPosition.y, _right.parent.localPosition.z),
                                                Time.deltaTime * 3f);
        }
    }
}