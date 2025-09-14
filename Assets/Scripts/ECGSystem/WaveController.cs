using ECGSystem;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class WaveController : MonoBehaviour
{
    [SerializeField] private Params parameters;
    private Renderer rend;
    private MaterialPropertyBlock mpb;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
        ApplyParams(); 
    }

    public void ApplyParams()
    {
        mpb.SetFloat("_speed", parameters.speed);
        mpb.SetFloat("_waves_Amount", parameters.wavesAmount);
        mpb.SetFloat("_noise_Scale", parameters.noiseScale);
        mpb.SetFloat("_noise_Amp", parameters.noiseAmp);
        mpb.SetFloat("_waves_Amp", parameters.wavesAmp);
        mpb.SetVector("_Color", (Vector4)parameters.color); 

        rend.SetPropertyBlock(mpb);
    }

    public void UpdateParameter(System.Action<Params> updateAction)
    {
        updateAction(parameters);
        ApplyParams();
    }
}