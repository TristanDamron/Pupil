using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Pupil {
    public class PupilInitializer : MonoBehaviour {
        [SerializeField]
        private string _device;
        private string _path;        
        private PupilData _data;
        public static PupilInitializer instance;

        void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        void Start () {
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
                _data.minIPD = 0f;
                _data.maxIPD = 0f;
                _data.maxDistance = 30f;
                _data.minDistance = 0f;
                SaveDataAsJson();
            }		
            
            PupilDataHolder.minIPD = _data.minIPD;                       
            PupilDataHolder.maxIPD = _data.maxIPD;
            PupilDataHolder.maxDistance = _data.maxDistance;
            PupilDataHolder.minDistance = _data.minDistance;
	    }

        public void SaveDataAsJson() {
            _data.minIPD = PupilDataHolder.minIPD;
            _data.maxIPD = PupilDataHolder.maxIPD;
            _data.minDistance = PupilDataHolder.minDistance;
            _data.maxDistance = PupilDataHolder.maxDistance;
            
            var json = JsonUtility.ToJson(_data);
            File.WriteAllText(_path, json);
        }

        private void CreateRigForDevice() {            
            var rig = (GameObject)Resources.Load("PupilCameraRig");

            switch (_device) {
                case "OpenVR":
                    rig = (GameObject)Resources.Load("PupilSteamVRCameraRig");
                    break;
            }

            Instantiate(rig, Vector3.zero, Quaternion.identity);            
        }        
    }
}