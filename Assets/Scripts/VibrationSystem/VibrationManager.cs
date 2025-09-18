using UnityEngine;
using Utils;

namespace VibrationSystem
{
    public class VibrationManager : MonoSingleton<VibrationManager>
    {
        public VibrationProfile defaultProfile;
        
        public void Play(VibrationProfile profile = null)
        {
            if (profile == null) profile = defaultProfile;
            if (profile == null) return;

    #if UNITY_ANDROID && !UNITY_EDITOR
        if (profile.pattern != null && profile.pattern.Length > 0)
            Vibration.VibrateAndroid(profile.pattern, profile.repeat);
        else
            Vibration.VibrateAndroid(profile.durationMs);
    #else
            Debug.Log($"[HAPTIC] Vibrate {profile.durationMs}ms");
    #endif
        }
    }
}