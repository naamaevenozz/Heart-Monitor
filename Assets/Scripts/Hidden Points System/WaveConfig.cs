using UnityEngine;

namespace Hidden_Points_System
{
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "Game/WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        public float speed;
        public int waveAmount;
        public float noiseAmp;
        public float noiseScale;
        public Vector4 color;

        public int targetAmount;
        public float spawnDelay;
        public float minBound;
        public float maxBound;
        public float lifetime;
    }
}