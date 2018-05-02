using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Pupil;

public class SettingsManager : MonoBehaviour {
	[SerializeField]
	private Slider _ipdSlider;
	[SerializeField]
	private PupilCameraSettings _settings;
	private PupilCamera _camera;
	private bool _minTest;
	private string _testMessage;
	[SerializeField]	
	private PupilInitializer _initializer;
	[SerializeField]
	private Text _text;
	[SerializeField]
	private LerpBetweenPoints _lerp;
	[SerializeField]
	private Button _finishButton;
	[SerializeField]
	private Canvas _canvas;

	void Start() {
		_testMessage = "Please adjust the slider until the ball is clearly visible.";
		_text.text = _testMessage;
		_canvas.worldCamera = GameObject.Find("Left").GetComponent<Camera>();
		_minTest = false;
	}

	void Update() {
		if (_minTest) {
			_settings.SetMinDistance(LayerMask.GetMask("UI"));
			_settings.SetMinDistanceIPD(_ipdSlider.value);
		} else {
			_settings.SetMaxDistance(LayerMask.GetMask("UI"));
			_settings.SetMaxDistanceIPD(_ipdSlider.value);			
		}
	}

	public void NextTest() {
		_minTest = !_minTest;

		if (_minTest == false) {
			_testMessage = "The ball will now move closer to you. Please adjust the slider until it's clearly visible.";
			_text.text = _testMessage;
			_lerp.ChangeTarget(new Vector3(0f, 1f, 4f));
			ShowFinishButton();
		}
	}

	public void ShowFinishButton() {
		_finishButton.gameObject.SetActive(true);
	}

	public void FinishTests() {
		_testMessage = "Calibration complete! Returning to the main scene...";
		_text.text = _testMessage;
		_initializer.SaveDataAsJson();
		Invoke("LoadSceneZero", 5f);
	}

	public void SaveData() {
		_initializer.SaveDataAsJson();
	}

	public void LoadSceneZero() {
		SceneManager.LoadScene(0);
	}	

	public void LoadSceneNext() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}	

}
