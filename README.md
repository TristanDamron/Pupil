# Pupil 
Unity assets for low-impact image optimizations on low-powered VR hardware. 

# Getting Started
## PupilCamera.cs
Dynamically adjusts the camera to account for variable inter-pupillary distance (IPD) based on how far away an object is from the player.
Simply place the Prefabs/PupilCameraSettings prefab into the hierarchy to set adjustment variables and trigger adjustments.


## PupilImageBlur.cs
Using the PupilImageBlur class, materials using the Shaders/BlurEdges shader will have the blur effect occur at a variable rate depending on the movement of the HMD.

*Example Script: Blur.cs*
```
using UnityEngine;
using Pupil;

public class Blur : MonoBehaviour {
	void Start () {
		PupilImageBlur.renderer = GetComponent<Renderer>();
		PupilImageBlur.camera = GameObject.FindGameObjectWithTag("MainCamera");
		//Refresh rate in frames. Defaults to 10 frames if not set.
		PupilImageBlur.refresh = 20;
		//Only do this if the BlurEdges shader is not already attached to the object's material.
		PupilImageBlur.SetEdgeShader(); 
	}
	void Update () {
		PupilImageBlur.AutoBlur();
	}
}
```

## Prefabs/PupilCameraRig.prefab
VR camera rig that supports dynamic IPD adjustments.  

## Prefabs/PupilInitializer.prefab
Loads the VR device for Unity

## Prefabs/PupilCameraSettings.prefab
Sets camera IPD settings and triggers auto adjustments. *Requires a PupilCameraRig prefab to be in the scene*
