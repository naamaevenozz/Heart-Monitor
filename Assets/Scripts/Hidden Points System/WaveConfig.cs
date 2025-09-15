using System.Drawing;

namespace Hidden_Points_System
{
    [System.Serializable]
    public class WaveConfig {
        public float speed;
        public int waveAmount;
        public float noiseAmp;
        public float noiseScale;
        public Color color;

        public int targetAmount;
        public float spawnDelay;
        public float lifetime;
    }

}