using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ECGWaveDrawer : MonoBehaviour
{
    [Header("Wave Parameters")]
    public ECGPointTester.Params parameters = new ECGPointTester.Params
    {
        speed = 0.2f,
        wavesAmount = 5f,
        wavesAmp = 0.2f,
        noiseAmp = 0.5f,
        noiseScale = 10f,
        baseColor = Color.cyan
    };

    [Header("Graph Settings")]
    public int resolution = 200;
    public Vector2 rangeX = new Vector2(0, 5);
    public float yOffset = 0;

    LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = resolution + 1;
        line.widthMultiplier = 0.05f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = parameters.baseColor;
        line.endColor = parameters.baseColor;
    }

    void Update()
    {
        float time = Time.time;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / resolution;
            float x = Mathf.Lerp(rangeX.x, rangeX.y, t);

            Vector2 uv = new Vector2(x, 0);
            float y = ECGPointTester.WaveValue(uv, time, parameters);

            line.SetPosition(i, new Vector3(x, y + yOffset, 0));
        }
    }
}