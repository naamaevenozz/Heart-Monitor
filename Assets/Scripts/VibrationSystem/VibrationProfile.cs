using UnityEngine;
namespace VibrationSystem
{
    [CreateAssetMenu(fileName = "VibrationProfile", menuName = "Game/Vibration Profile")]
    public class VibrationProfile : ScriptableObject
    {
        public int durationMs = 40;
        public long[] pattern;
        public int repeat = -1;
    }
}