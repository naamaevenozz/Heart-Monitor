using UnityEngine;

public class ECGPointTester : MonoBehaviour
{
    public struct Params
    {
        public float speed;        
        public float wavesAmount;  
        public float wavesAmp;     
        public float noiseAmp;     
        public float noiseScale;   
        public Color baseColor;    
    }

    
    public struct Eval
    {
        public float height;        
        public Vector2 vertexAdd;  
        public Vector2 uvShifted;   
        public Color color;         
        public float phaseX;        
        public float rawNoise01;    
        public float noisePhase;    
    }

    static float Remap(float v, float inMin, float inMax, float outMin, float outMax)
    {
        return outMin + (v - inMin) * (outMax - outMin) / (inMax - inMin);
    }

    static float SimpleNoise2D(Vector2 uv, float scale)
    {
        return Mathf.PerlinNoise(uv.x * scale, uv.y * scale);
    }

    public static Eval Evaluate(Vector2 uv, float time, in Params p)
    {
        Eval e = default;

        float offsetX = time * p.speed;                 
        e.uvShifted = uv + new Vector2(offsetX, 0f);    

        float freq = p.wavesAmount * Mathf.PI;

        float baseX = e.uvShifted.x * freq;

        e.rawNoise01 = SimpleNoise2D(e.uvShifted, p.noiseScale);
        e.noisePhase = Remap(e.rawNoise01, 0f, 1f, -p.noiseAmp, +p.noiseAmp);

        e.phaseX = baseX + e.noisePhase;

        float sineX = Mathf.Sin(e.phaseX);

        e.height = sineX * p.wavesAmp;

        e.vertexAdd = new Vector2(0f, e.height);


        float phaseY = e.uvShifted.y * freq;        
        float sineY = Mathf.Sin(phaseY);            

        float sineY01 = Remap(sineY, -1f, 1f, 0f, 1f);

        sineY01 = Mathf.Clamp01(sineY01);

        e.color = p.baseColor * sineY01;
        e.color.a = 1f;

        return e;
    }

    public static float WaveValue(Vector2 uv, float time, in Params p)
    {
        return Evaluate(uv, time, p).height;
    }
    
}
