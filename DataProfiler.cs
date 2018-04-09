using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pupil;

public class DataProfiler : MonoBehaviour {
    [SerializeField]
    private Text _profileInfo;
    [SerializeField]
    private float _sampleTime;
    private float _runningTime;
    private Dictionary<float, float> _cpuSample;
    private Dictionary<float, float> _memorySample;
    private Dictionary<float, int> _fpsSample;

    void Start() {
        _cpuSample = new Dictionary<float, float>();
        _memorySample = new Dictionary<float, float>();
        _fpsSample = new Dictionary<float, int>();
        if (_profileInfo == null) 
            Debug.LogWarning("Warning: No Text component attached to script.");
        Invoke("DumpToFile", _sampleTime);
        InvokeRepeating("Sample", 1f, 0.1f);
    }

    public void Sample() {
        PupilProfileData.TakeSample();
        if (_profileInfo != null)
            DisplayData();        
        _runningTime += Time.deltaTime;
        _cpuSample.Add(_runningTime, PupilProfileData.cpu);
        _memorySample.Add(_runningTime, PupilProfileData.memory);
        _fpsSample.Add(_runningTime, PupilProfileData.fps);                
    }

    private void DisplayData() {
        _profileInfo.text = PupilProfileData.cpu + "% CPU, " + PupilProfileData.memory + "MB available, " + PupilProfileData.fps + " FPS";
    }

    public void DumpToFile() {
        var masterPath = Path.Combine(Application.persistentDataPath, "PupilProfileData.txt");
        var cpuData = "CPU Usage" + System.Environment.NewLine;

        foreach (KeyValuePair<float, float> c in _cpuSample) {
            cpuData += c.Key + " " + c.Value + System.Environment.NewLine;
        }

        var memoryData = "Available Memory" + System.Environment.NewLine;

        foreach (KeyValuePair<float, float> m in _memorySample) {
            memoryData += m.Key + " " + m.Value + System.Environment.NewLine;
        }
        
        var fpsData = "Frames Per Second" + System.Environment.NewLine;

        foreach (KeyValuePair<float, int> f in _fpsSample) {
            fpsData += f.Key + " " + f.Value + System.Environment.NewLine;
        }

        var data = cpuData + memoryData + fpsData;
        File.WriteAllText(masterPath, data);
    }
}
