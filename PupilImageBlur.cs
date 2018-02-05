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
		public static int refresh;
		private static int _frames;
		private static Vector3 _lastAngles;

		public static void SetEdgeShader () {
			if (renderer == null) {
				Debug.LogError("Error: Renderer has not been set. Cannot set Edge Shader.");	
			} else {
				renderer.material.shader = Shader.Find("Custom/BlurEdges");
			}
		}

		public static void AutoBlur() {
			if (camera == null) {
				Debug.LogError("Error: Camera has not been set. Cannot execute blur.\nHint: try 'PupilImageBlur.camera = GameObject.FindGameObjectWithTag('MainCamera');");	
			} else {	
				if (refresh == 0) {
					Debug.LogWarning("Refresh rate not set, defaulting to 10 frames");
					refresh = 10;
				}
				_frames++;
				//Not interested in the camera parent's location, but the location of its children.
				if (_lastAngles.y != 0f) 
					renderer.material.SetFloat("_Blur", camera.transform.GetChild(0).transform.localEulerAngles.y / _lastAngles.y);
				if (_frames >= refresh) {
					_lastAngles = camera.transform.GetChild(0).localEulerAngles;
					_frames = 0;
				}
			}
		}
	}
}
