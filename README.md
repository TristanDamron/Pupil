# Pupil v0.2 BETA
Unity assets for low-impact image optimizations on low-powered VR hardware. Download on [itch.io](https://tdamron.itch.io/pupil)

![Pupil Logo](https://pbs.twimg.com/media/DYRiylRU8AA6P4u.jpg)
*Pupil is 100% free to use, but it would be really awesome if you could credit it in your VR experience with the Pupil logo!*

# Getting Started
You can start using Pupil in your C# scripts by using the Pupil namespace.

```
using Pupil;
```

## PupilCamera.cs
Dynamically adjusts the camera to account for variable inter-pupillary distance (IPD) based on how far away an object is from the player.
Simply place the Prefabs/PupilCameraSettings prefab into the hierarchy to set adjustment variables and trigger adjustments.

PupilCamera.cs also has functionality for automatically adjusting depth of field for objects close to the player. *This feature requires Unity.PostProcessing.* Attach the PupilCameraProfile.asset to each camera's PostProcessingBehaviour component to trigger adjustments.

### Private fields

```
float _ipd
```

Difference between IPD at variable lengths.

```
Transform _camera
```

Transform data for the PupilCameraRig.prefab in the scene.

```
float _minDistance
```

The minimum distance associated with the user's minimum IPD.

```
float _maxDistance
```

The maximum distance associated with the user's maximum IPD.

```
float _minDistanceIPD
```

User's IPD associated with the recorded minimum distance.

```
float _maxDistanceIPD
```

User's IPD associated with the recorded maximum distance.

```
bool _autoAdjustWarnings
```

Lock for auto adjust warnings.

``` 
PostProcessingBehavior _behaviorLeft
```

Post processing behavior associated with the left camera on the PupilCameraRig.prefab prefab. Used for depth of field adjustments.

``` 
PostProcessingBehavior _behaviorRight
```

Post processing behavior associated with the right camera on the PupilCameraRig.prefab prefab. Used for depth of field adjustments.

```
GameObject _nearest
```

The nearest GameObject that has been detected.

### Public Methods

```
float GetDistanceToGameObject(GameObject obj)
```

Finds the distance between the camera and the closest object to it.

```
float GetDistanceToGameObject(GameObject obj, GameObject from)
```

*Override* Finds the distance between *from* and *obj*

```
void SetMinDistanceIPD(float distance, float ipd)
```

Sets private fields for minimum distance and minimum ipd for the PupilCamera object.

```
void SetMaxDistanceIPD(float distance, float ipd)
```

Sets private fields for maximum distance and maximum ipd for the PupilCamera object.

```
GameObject FindNearest()
```

Raycasts to find the object nearest to the front of the camera.

```
GameObject FindNearest(int ignoreLayer)
```

*Override* Raycasts to find the object nearest to the front of the camera while ignoring the layer at *ignoreLayer.*

```
void DrawViewLines()
```

Draws lines between the two cameras on the PupilCameraRig prefab. Useful for debugging.

```
void AutoAdjustDepthOfField()
```

Automatically adjusts the camera post processing behaviors' depth of field settings depending on the distance of the nearest object from in front of the PupilCameraRig.prefab prefab.

```
void AutoAdjustIPD()
```

Automatically interpolates between the minimum IPD and maximum IPD based on the nearest objects distance to the camera. *Updates every frame in PupilCameraSettings.cs*

## PupilCameraSettings.cs
Script for applying static data values from PupilDataHolder.cs to the PupilCamera. Instantiates a new PupilCamera object and handles its functionality. Simply place a PupilCameraSettings.prefab prefab in your scene to access its features.

### Private fields

```
PupilCamera _camera
```

PupilCamera object associated with the PupilCameraRig.prefab prefab. 

```
float _minDistanceIPD
```

Serialized variable for the current minimum IPD for the user. Initialized from the PupilDataHolder.maxIPD.

```
float _maxDistanceIPD
```

Serialized variable for the current maximum IPD for the user. Initialized from the PupilDataHolder.maxIPD.

```
float _minDistance
```

Serialized variable for the current minimum distance for the user. Initialized from the PupilDataHolder.minDistance.

```
float _maxDistance
```

Serialized variable for the current maximum distance for the user. Initialized from the PupilDataHolder.maxDistance.

```
bool _debug
```

Serialized variable for showing debug info. 

### Public Methods

```
void SetMinDistanceIPD(float ipd)
```

Assigns *ipd* to the static variable PupilDataHolder.minIPD and updates the PupilCamera.cs minimum IPD settings.

```
void SetMaxDistanceIPD(float ipd) 
```

Assigns *ipd* to the static variable PupilDataHolder.maxIPD and updates the PupilCamera.cs maximum IPD settings.

```
void SetMinDistance()
```

Finds the closest GameObject to the camera and sets its distance to the static variable PupilDataHolder.minDistance

```
void SetMinDistance(int ignoreLayer)
```

*Override* Finds the closest GameObject to the camera not in the layer *ignoreLayer* and sets its distance to the static variable PupilDataHolder.minDistance

```
void SetMaxDistance()
```

Finds the closest GameObject to the camera and sets its distance to the static variable PupilDataHolder.maxDistance

```
void SetMaxDistance()
```

*Override* Finds the closest GameObject to the camera not in the layer *ignoreLayer* and sets its distance to the static variable PupilDataHolder.maxDistance

## PupilInitializer.cs
Script for initializing the VR device for Unity as well as loading json data to the PupilDataHolder.cs

### Private fields

```
string _device
```

Serialized variable for the name of the VR device. Used for device loading.

```
string _path
```

Data path for the PlayerData.json file.

```
PupilData _data
```

PupilData object that is saved/loaded to/from PlayerData.json.

### Public Methods

```
void LoadFromJson()
```

Loads PlayerData.json and derives a PupilData object from it. If the file does not exist, PupilData falls back to default values. One the object is loaded, the variables are saved to the PupilDataHolder.cs static variables for faster access.

```
void SaveDataAsJson()
```

Converts PupilData to PlayerData.json.

### Private Methods

```
void CreateRigForDevice()
```

Instantiates a camera rig based on the loaded VR device.

## PupilData.cs
Serializable object for saving and loading user data to/from PlayerData.json

### Public fields

```
float minIPD
```

Minimum recorded IPD.

```
float maxIPD
```

Maximum recorded IPD.

```
float minDistance
```

Minimum recorded distance.

```
float maxDistance
```

Maximum recorded distance.

## PupilDataHolder.cs
Static class that holds the loaded data from PupilData.cs

### Public fields

```
static float minIPD
```

Minimum recorded IPD.

```
static float maxIPD
```

Maximum recorded IPD.

```
static float minDistance
```

Minimum recorded distance.

```
static float maxDistance
```

Maximum recorded distance.

## Prefabs/PupilCameraRig.prefab
VR camera rig that supports dynamic IPD adjustments and dynamic depth of field adjustments from PupilCamear.cs.  

## Prefabs/PupilInitializer.prefab
Loads the VR device for Unity and caches IPD settings from a json file.

## Prefabs/PupilCameraSettings.prefab
Sets camera IPD settings and triggers auto adjustments. *Requires a PupilCameraRig prefab to be in the scene*

## Prefabs/PupilSteamVRCameraRig.prefab
Slightly modified SteamVR camera rig that includes the PupilCameraRig prefab. *Requires the SteamVR Unity plugin* 

# Shaders
Pupil comes with a variety of shaders for image optimization and accessibility.

## Shaders/StandardLowContrastOutline.shader
Renders an outline around the object that is low contrast to the texture.

## Shaders/UnlitLowContrastOutline.shader
Like Shaders/StandardLowContrastOutline.shader, but unlit. 

# Scenes
*New in v0.2!* Pupil now includes scenes to help developers set IPD data for their users.

## Scenes/SettingsMenu.unity
Scene used for setting the user's IPD. *Comes without raycasting controllers for UI elements.*