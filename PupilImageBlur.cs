using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Pupil {
	// /*
	// PupilImageBlur

	// Blurs outline shaders at variable rate based on headset movement.
	// DEPRECATED IN v0.1
	// */
	public static class PupilImageBlur {
		public static Renderer renderer;
		public static GameObject camera;
		public static int refresh;
		private static int _frames;
		private static Quaternion _lastRotation;

		public static void SetEdgeShader () {
			if (renderer == null) {
				Debug.LogError("Error: Renderer has not been set. Cannot set Edge Shader.");	
			} else {
				renderer.material.shader = Shader.Find("Pupil/Unlit/BlurredOutline");
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

			
				if (_lastRotation != InputTracking.GetLocalRotation(XRNode.Head)) 
					renderer.material.SetFloat("_Alpha", InputTracking.GetLocalRotation(XRNode.Head).y / _lastRotation.y);
				if (_frames >= refresh) {
					_lastRotation = InputTracking.GetLocalRotation(XRNode.Head);
					_frames = 0;
				}
			}
		}
	}
}
