using System;
using System.Linq;
using UnityEngine;
using Utilities;
using Utils;

namespace Sound
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField] private AudioSettings settings;
        private AudioSourceWrapper _backgroundMusic;
        private AudioSourceWrapper openingMusic;
        private AudioSourceWrapper endingMusic;

        public void PlaySound(string audioName, Transform spawnTransform, float customVolume = -1f)
        {
            var config = FindAudioConfig(audioName);
            if (config == null)
                return;
            var soundObject = SoundPool.Instance.Get();
            soundObject.transform.position = spawnTransform.position;
            float finalVolume = (customVolume >= 0f) ? customVolume : config.volume;
            soundObject.Play(config.clip, finalVolume, config.loop);
        }

        private AudioConfig FindAudioConfig(string audioName)
        {
            var x = settings.audioConfigs.FirstOrDefault(config => config.name == audioName);
            if(x!= null)
            {
                return x;
            }
            else
            {
                Debug.LogError($"Audio config not found for {audioName}");
                return null;
            }
        }
    }
}