using DefaultNamespace;
using ECGSystem;
using Hidden_Points_System;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    // [SerializeField] private Params parameters;
    // private Renderer rend;
    [SerializeField] public Material material;

    void Awake()
    {
        // rend = GetComponent<Renderer>();
        // mpb = new MaterialPropertyBlock();
    }
    private void OnEnable()
    {
        GameEvents.OnWaveStarted += HandleWaveStarted;
    }

    private void HandleWaveStarted(WaveConfig obj)
    {
        Debug.Log("Wave Started Event from MaterialController");
        
        material.SetFloat("_speed", obj.speed);
        material.SetFloat("_waves_Amount",obj.wavesAmount);
        material.SetFloat("_waves_Amp", obj.wavesAmp);
        material.SetFloat("_noise_Amp", obj.noiseAmp);
        material.SetFloat("_noise_Scale", obj.noiseScale);

        // rend.SetPropertyBlock(mpb);
    }

    private void OnDisable()
    {
        GameEvents.OnWaveStarted -= HandleWaveStarted;
    }


    // public void ApplyParams()
    // {
    //     mpb.SetFloat("_speed", parameters.speed);
    //     mpb.SetFloat("_waves_Amount", parameters.wavesAmount);
    //     mpb.SetFloat("_noise_Scale", parameters.noiseScale);
    //     mpb.SetFloat("_noise_Amp", parameters.noiseAmp);
    //     mpb.SetFloat("_waves_Amp", parameters.wavesAmp);
    //     mpb.SetVector("_Color", (Vector4)parameters.color); 
    //
    //     rend.SetPropertyBlock(mpb);
    // }

    // public void UpdateParameter(System.Action<Params> updateAction)
    // {
    //     updateAction(parameters);
    //     // ApplyParams();
    // }
}