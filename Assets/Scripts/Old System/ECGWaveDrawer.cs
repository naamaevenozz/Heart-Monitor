using DefaultNamespace;
using Hidden_Points_System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ECGWaveDrawer : MonoBehaviour
{
    [Header("Shader Material")]
    [SerializeField] public Material material;

    // [Header("Wave Parameters (Serialized)")]
    // [SerializeField] public ECGPointTester.Params parameters;
    //
    // [Header("Graph Settings")]
    // [SerializeField] private int resolution = 200;
    // [SerializeField] private Vector2 rangeX = new Vector2(0, 5);
    // [SerializeField] private float yOffset = 0;

    private LineRenderer line;

    void Start()
    {
        // line = GetComponent<LineRenderer>();
        // line.positionCount = resolution + 1;
        // line.widthMultiplier = 0.05f;
        // line.material = new Material(Shader.Find("Sprites/Default"));
        // line.startColor = parameters.baseColor;
    }
    private void OnEnable()
    {
        GameEvents.OnWaveStarted += HandleWaveStarted;
    }

    private void HandleWaveStarted(WaveConfig obj)
    {
        material.SetFloat("_speed", obj.speed);
        material.SetFloat("_waves_Amount", obj.wavesAmount);
        material.SetFloat("_waves_Amp", obj.wavesAmp);
        material.SetFloat("_noise_Amp", obj.noiseAmp);
        material.SetFloat("_noise_Scale", obj.noiseScale);
    }

    void Update()
    {
        // if (material != null)
        // {
        //     material.SetFloat("_speed", parameters.speed);
        //     material.SetFloat("_waves_Amount", parameters.wavesAmount);
        //     material.SetFloat("_waves_Amp", parameters.wavesAmp);
        //     material.SetFloat("_noise_Amp", parameters.noiseAmp);
        //     material.SetFloat("_noise_Scale", parameters.noiseScale);
        // }

        // float time = Time.time;
        // for (int i = 0; i <= resolution; i++)
        // {
        //     float t = (float)i / resolution;
        //     float x = Mathf.Lerp(rangeX.x, rangeX.y, t);
        //
        //     Vector2 uv = new Vector2(x, 0);
        //     float y = ECGPointTester.WaveValue(uv, time, parameters);
        //
        //     line.SetPosition(i, new Vector3(x, y + yOffset, 0));
        }
    // }
}