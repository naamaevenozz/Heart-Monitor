using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;
using Color = UnityEngine.Color;

namespace ECGSystem 
{
    public class Params : MonoBehaviour
    {
        
        public float speed = 1f;
        public float wavesAmount = 2f;
        public float noiseScale = 1f;
        public float noiseAmp = 0.5f;
        public float wavesAmp = 1f;
        public Color color = Color.white;
    }
}