using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pupil {
	[System.Serializable]
	public class PupilData {
		public float left;
		public float right;
		public float minIPD;
		public float maxIPD;
		public float maxDistance;
		public bool colorBlind;
		public string red;
		public string blue;
		public string green;
		public string yellow;
	}
}