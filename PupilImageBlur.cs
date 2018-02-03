using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pupil {
	// /*
	// PupilImageBlur

	// Blurs outline shaders at variable rate based on headset movement.
	// */
	public class PupilImageBlur {
		private static Renderer _renderer;
		private static GameObject _obj;
		private static Camera _camera;
		private static int _frames;
		private static Vector3 _lastAngles;

		public PupilImageBlur() {
			_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		}


		public static void SetEdgeShader (GameObject obj) {
			var rend = obj.GetComponent<Renderer>();
			rend.material.shader = Shader.Find("Custom/BlurEdges");
			_renderer = rend;
			_obj = obj;
		}

		//Every 10 frames, calculate the percentage movement between two rotation vectors
		//TODO: Static fields are uninitialized?! 
		public static void AutoBlur() {
			_renderer.material.SetFloat("_Blur", _lastAngles.y / _camera.transform.localEulerAngles.y);	
			Debug.Log(_renderer.material.GetFloat("_Blur"));		
			if (_frames >= 10) {
				_lastAngles = _camera.transform.localEulerAngles;
				_frames = 0;
			}
		}
	}
}
