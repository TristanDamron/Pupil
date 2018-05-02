using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pupil {
	[System.Serializable]
	public class PupilData {
		public int left;
		public int right;
		public float minIPD;
		public float maxIPD;
		public float minDistance;
		public float maxDistance;
	}
}