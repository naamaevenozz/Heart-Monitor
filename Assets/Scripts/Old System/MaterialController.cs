using System.Collections;
using DefaultNamespace;
using Hidden_Points_System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MaterialController : MonoBehaviour
{
    [Header("Shader Material")]
    [SerializeField] public Material material;
    [SerializeField] public WaveConfig introWaveConfig;
    [SerializeField] private float resetDuration = 1f; 

    private Coroutine resetCoroutine;

    private void ResetFunc()
    {
        Debug.Log("Final wave from material controller");

        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(FadeWaveAmpToZero());
    }

    private IEnumerator FadeWaveAmpToZero()
    {
        float startValue = material.GetFloat("_waves_Amp");
        float time = 0f;

        while (time < resetDuration)
        {
            time += Time.deltaTime;
            float t = time / resetDuration;

            float newValue = Mathf.Lerp(startValue, 0f, t);
            material.SetFloat("_waves_Amp", newValue);

            yield return null;
        }

        material.SetFloat("_waves_Amp", 0f);
        resetCoroutine = null;
    }


    private LineRenderer line;

    void Start()
    {

    }
    private void OnEnable()
    {
        GameEvents.OnWaveStarted += HandleWaveStarted;
        GameEvents.GameOver += ResetFunc;
        GameEvents.Intro += ShowIntro;

    }

    private void ShowIntro()
    {
        HandleWaveStarted(introWaveConfig);
    }

    // private void ResetFunc()
    // {
    //     Debug.Log("Final wave from material controller");
    //     material.SetFloat("_waves_Amp", 0);
    // }

    private void HandleWaveStarted(WaveConfig obj)
    {
        material.SetFloat("_speed", obj.speed);
        material.SetFloat("_waves_Amount", obj.wavesAmount);
        material.SetFloat("_waves_Amp", obj.wavesAmp);
        material.SetFloat("_noise_Amp", obj.noiseAmp);
        material.SetFloat("_noise_Scale", obj.noiseScale);
    }
}