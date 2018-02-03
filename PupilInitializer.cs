using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PupilInitializer : MonoBehaviour {
    [SerializeField]
    private string _device;
    void Start () {
        XRSettings.LoadDeviceByName(_device);
    }
}