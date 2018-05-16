using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using Pupil;

public class PopupWarning : MonoBehaviour {	
	IEnumerator Start () {		
		var www = UnityWebRequest.Get("https://pupil-vr.herokuapp.com/redeem/api/" + PupilDataHolder.username + "/" + PupilDataHolder.password);
		yield return www.SendWebRequest();
		var response = www.downloadHandler.text;

		if (!response.Contains("buildkey")) {
			EditorUtility.DisplayDialog("Warning", "Your copy of Pupil is unvalidated! Pupil needs to be validated before it can be used in your" +
			" VR experience. Please go to https://www.pupiltechnologies.xyz/redeem to redeem a build key.", "Okay");		
		}	
	}
}
