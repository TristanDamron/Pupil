using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Pupil {
    public class PupilInitializer : MonoBehaviour {
        [SerializeField]
        private string _device;
        [SerializeField]
        private Vector3 _rigPosition;
        [SerializeField]
        private Quaternion _rigRotation;        
        private string _path;        
        private PupilData _data;
        public static PupilInitializer instance;

        void Awake() {
            XRSettings.LoadDeviceByName(_device);
            var persistentPath = Application.persistentDataPath;
            _path = Path.Combine(persistentPath, "PupilData.json");  
            LoadFromJson();      
            CreateRigForDevice();                
        }

        public void LoadFromJson() {
            if (File.Exists(_path)) {
                var json = File.ReadAllText(_path);
                _data = JsonUtility.FromJson<PupilData>(json); 
            } else {
                Debug.LogWarning("Warning: PupilData.json not found, applying default settings.");
                _data = new PupilData();
                _data.left = 0;
                _data.right = 0;                
                _data.minIPD = 0f;
                _data.maxIPD = 0f;
                _data.maxDistance = 30f;
                _data.minDistance = 0f;
                _data.red = "#000000";
                _data.blue = "#000000";
                _data.green = "#000000";
                _data.yellow = "#000000";
            }		
            
            PupilDataHolder.left = _data.left;
            PupilDataHolder.right = _data.right;
            PupilDataHolder.minIPD = _data.minIPD;                       
            PupilDataHolder.maxIPD = _data.maxIPD;
            PupilDataHolder.maxDistance = _data.maxDistance;
            PupilDataHolder.minDistance = _data.minDistance;            
            PupilDataHolder.red = _data.red;
            PupilDataHolder.blue = _data.blue;
            PupilDataHolder.green = _data.green;
            PupilDataHolder.yellow = _data.yellow;

            SaveDataAsJson();            
	    }

        public void SaveDataAsJson() {
            _data.left = PupilDataHolder.left;
            _data.right = PupilDataHolder.right;
            _data.minIPD = PupilDataHolder.minIPD;
            _data.maxIPD = PupilDataHolder.maxIPD;
            _data.minDistance = PupilDataHolder.minDistance;
            _data.maxDistance = PupilDataHolder.maxDistance;
            _data.red = PupilDataHolder.red;
            _data.blue = PupilDataHolder.blue;
            _data.green = PupilDataHolder.green;
            _data.yellow = PupilDataHolder.yellow;
            
            var json = JsonUtility.ToJson(_data);
            File.WriteAllText(_path, json);
        }

        private void CreateRigForDevice() {            
            var rig = (GameObject)Resources.Load("PupilCameraRig");

            switch (_device) {
                case "OpenVR":
                    rig = (GameObject)Resources.Load("PupilSteamVRCameraRig");
                    if (GameObject.Find("[SteamVR]") == null) 
                        Debug.LogWarning("Error: No SteamVR prefab found in scene. Controller tracking will be disabled.");
                    break;
            }

            Instantiate(rig, _rigPosition, _rigRotation);            
        }        
    }
}