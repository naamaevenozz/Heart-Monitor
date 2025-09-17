using UnityEngine;
using Utils;

namespace VibrationSystem
{
    public class VibrationManager : MonoSingleton<VibrationManager>
    {
        public VibrationProfile defaultProfile;
        
        private void Awake()
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    Vibration.Init();
    Debug.Log("[HAPTIC] Vibration initialized");
#endif
        }
        
        public void Play(VibrationProfile profile = null)
        {
            if (profile == null) profile = defaultProfile;
            if (profile == null) return;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            if (profile.pattern != null && profile.pattern.Length > 0)
            {
                Vibration.Vibrate(profile.pattern, profile.repeat);
            }
            else
            {
                Vibration.Vibrate(profile.durationMs);
            }
#else
            Debug.Log($"[HAPTIC] Vibrate {profile.durationMs}ms (Editor simulation)");
#endif
        }
        public void Cancel()
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            Vibration.Cancel();
#else
            Debug.Log("[HAPTIC] Cancel vibration (Editor simulation)");
#endif
        }
    }
    
}