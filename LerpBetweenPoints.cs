using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpBetweenPoints : MonoBehaviour {
	[SerializeField]
	private Vector3 _target;
	private bool _start;

	void Start() {
		StartCoroutine(SetStartPosition());
		_target = new Vector3(0f, 1f, 55f);
	}

	void Update () {
		if (_start)
			transform.position = Vector3.Lerp(transform.position, _target, Time.deltaTime / 3f);		
	}

	public void ChangeTarget(Vector3 newTarget) {
		_target = newTarget;
	}

	IEnumerator SetStartPosition() {
		yield return new WaitForSeconds(1f);
		transform.position = new Vector3 (0, 1, 100);
		_start = true;
	}
}
