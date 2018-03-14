# Pupil v0.1
Unity assets for low-impact image optimizations on low-powered VR hardware. 

![Pupil Logo] (https://pbs.twimg.com/media/DYRiylRU8AA6P4u.jpg)
*Pupil is 100% free to use, but it would be really awesome if you could credit it in your VR experience with the Pupil logo!*

# Getting Started
## PupilCamera.cs
Dynamically adjusts the camera to account for variable inter-pupillary distance (IPD) based on how far away an object is from the player.
Simply place the Prefabs/PupilCameraSettings prefab into the hierarchy to set adjustment variables and trigger adjustments.

PupilCamera.cs also has functionality for automatically adjusting depth of field for objeccts close to the player. *This feature requires Unity.PostProcessing.* Attach the PupilCameraProfile.asset to each camera's PostProcessingBehaviour component to trigger adjustments.

## Prefabs/PupilCameraRig.prefab
VR camera rig that supports dynamic IPD adjustments.  

## Prefabs/PupilInitializer.prefab
Loads the VR device for Unity.

## Prefabs/PupilCameraSettings.prefab
Sets camera IPD settings and triggers auto adjustments. *Requires a PupilCameraRig prefab to be in the scene*

# Shaders
Pupil comes with a variety of shaders for image optimization and accessibility.

## Shaders/StandardLowContrastOutline.shader
Renders an outline around the object that is low contrast to the texture.

## Shaders/UnlitLowContrastOutline.shader
Like Shaders/StandardLowContrastOutline.shader, but unlit. 

## *(Deprecated)* PupilImageBlur.cs 
Using the PupilImageBlur class, materials using the Shaders/BlurEdges shader will have the blur effect occur at a variable rate depending on the movement of the HMD.

*Example Script: Blur.cs*
```csharp
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
