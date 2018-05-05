using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pupil;

public class PupilColorBlindCorrection : MonoBehaviour {
	private Renderer _renderer;
	private Shader _colorBlindShader;	
	private Color _red;
	private Color _blue;
	private Color _green;
	private Color _yellow;

	void Start() {
		_renderer = GetComponent<Renderer>();
		_colorBlindShader = Shader.Find("Pupil/ColorblindCorrection");

		ColorUtility.TryParseHtmlString(PupilDataHolder.red, out _red);
		ColorUtility.TryParseHtmlString(PupilDataHolder.blue, out _blue);
		ColorUtility.TryParseHtmlString(PupilDataHolder.green, out _green);
		ColorUtility.TryParseHtmlString(PupilDataHolder.yellow, out _yellow);

		if (_renderer.material.shader != _colorBlindShader && PupilDataHolder.colorBlind) {
			Debug.LogWarning("Warning: Gameobject " + gameObject.name + " does not have the ColorblindCorrection shader. Swapping now.");
			SwapShader();
		}

		if (PupilDataHolder.colorBlind)
			SetTint();
	}

	private void SwapShader() {
		_renderer.material.shader = _colorBlindShader;
	}
	
	private void SetTint() {
		var color = _renderer.material.color;		
		var tex = _renderer.material.mainTexture;
	
		if (color != Color.white) {
			if (color.r >= 0.25f) {
				_renderer.material.SetColor("_Tint", _red / color.r);
			}
			
			if (color.g >= 0.25f) {
				_renderer.material.SetColor("_Tint", _green / color.g);
			} 
			
			if (color.b >= 0.25f) {
				_renderer.material.SetColor("_Tint", _blue / color.b);
			}
			
			// @TODO: Pupil is recognizing everything as yellow... 
			// if (color.g + color.r >= 0.25) {
			// 	_renderer.material.SetColor("_Tint", _yellow / (color.g + color.r));
			// }
		}
		
		if (tex != null){
			var texColor = GetAverageColor();
			
			if (texColor.r >= 0.25f) {
				_renderer.material.SetColor("_Tint", _red * texColor.r);
			}
			
			if (texColor.g >= 0.25f) {
				_renderer.material.SetColor("_Tint", _green * texColor.g);
			} 
			
			if (texColor.b >= 0.25f) {
				_renderer.material.SetColor("_Tint", _blue * texColor.b);
			} 
			
			// if (texColor.g + texColor.r >= 0.25) {
			// 	_renderer.material.SetColor("_Tint", _yellow * (texColor.g + texColor.r));
			// }					
		}
	}

	private Color GetAverageColor() {
		var texture = (Texture2D)_renderer.material.mainTexture;
		var pixels = texture.GetPixels();

		var ret = new Color();

		var avg_r = 0.0f;
		var avg_g = 0.0f;
		var avg_b = 0.0f;

		foreach(Color p in pixels) {
			avg_r += p.r;
			avg_b += p.b;
			avg_g += p.g;
		}

		avg_r = avg_r / pixels.Length;
		avg_g = avg_g / pixels.Length;
		avg_b = avg_b / pixels.Length;

		ret.r = avg_r;
		ret.g = avg_g;
		ret.b = avg_b;

		return ret;
	}

}
