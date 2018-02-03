using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pupil {
	// /*
	// PupilImageBlur

	// Blurs outline shaders at variable rate based on headset movement.
	// */
	public static class PupilImageBlur {
		public static Renderer renderer;
		public static GameObject camera;
		private static int _frames;
		private static Vector3 _lastAngles;


		public static void SetEdgeShader () {
			if (renderer == null) {
				Debug.LogError("Error: Renderer has not been set. Cannot set Edge Shader.");	
			} else {
				renderer.material.shader = Shader.Find("Custom/BlurEdges");
			}
		}

		//Every 10 frames, calculate the percentage movement between two rotation vectors
		public static void AutoBlur() {
			if (camera == null) {
				Debug.LogError("Error: Camera has not been set. Cannot execute blur.\nHint: try 'PupilImageBlur.camera = GameObject.FindGameObjectWithTag('MainCamera');");	
			} else {
				if (camera.transform.localEulerAngles.y != 0f) 
					renderer.material.SetFloat("_Blur", _lastAngles.y / camera.transform.localEulerAngles.y);			
				if (_frames >= 10) {
					_lastAngles = camera.transform.localEulerAngles;
					_frames = 0;
				}
			}
		}
	}
}
