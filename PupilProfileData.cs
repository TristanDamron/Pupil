using UnityEngine;
using System.Diagnostics;
using UnityEngine.XR;

namespace Pupil {
    public class PupilProfileData {
        public static int fps = 0;
        public static float cpu = 0f;
        public static float memory = 0f;
        public static Vector3 position = Vector3.zero;
        public static Quaternion rotation = Quaternion.identity;

        //private static PerformanceCounter _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        //private static PerformanceCounter _memoryCounter = new PerformanceCounter("Memory", "Available MBytes");        

        public static void TakeSample() {
            fps = (int) (1f / Time.deltaTime);
            //cpu = _cpuCounter.NextValue();
            //memory = _memoryCounter.NextValue();
            position = InputTracking.GetLocalPosition(XRNode.Head);
            rotation = InputTracking.GetLocalRotation(XRNode.Head);
        }
    }
}
