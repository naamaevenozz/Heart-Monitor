using UnityEngine;

namespace Hidden_Points_System
{
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "Game/WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        public float targetAmount;
        public float spawnDelay;
        public float minBound;
        public float maxBound;
        public float lifetime;
        
        public float wavesAmount;
        public float wavesAmp;
        public float speed;
        public float noiseAmp;
        public float noiseScale;
        // public Vector4 color;
    
        
    }
}