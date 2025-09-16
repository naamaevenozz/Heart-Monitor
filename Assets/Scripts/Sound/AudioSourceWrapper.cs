using Sound;
using UnityEngine;
using Utilities;
using Utils;

public class AudioSourceWrapper : MonoBehaviour, IPoolable
{
    private AudioSource audioSource;
    private bool isPlaying;
    
    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isPlaying || audioSource.isPlaying)
            return;
        isPlaying = false;
        SoundPool.Instance.Return(this);
    }

    public void Play(AudioClip clip, float volume,bool loop)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
        isPlaying = true;
    }

    public void Reset()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = null;
        audioSource.volume = 1f;
        isPlaying = false;
    }
    
    public bool IsPlaying()
    {
        return audioSource != null && audioSource.isPlaying;
    }

}
