using UnityEngine;
using System.Diagnostics;

namespace Pupil {
    public class PupilProfileData {
        public static int fps = 0;
        public static float cpu = 0f;
        public static float memory = 0f;

        private static PerformanceCounter _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private static PerformanceCounter _memoryCounter = new PerformanceCounter("Memory", "Available MBytes");        

        public static void TakeSample() {
            fps = (int) (1f / Time.deltaTime);
            cpu = _cpuCounter.NextValue();
            memory = _memoryCounter.NextValue();
        }
    }
}
