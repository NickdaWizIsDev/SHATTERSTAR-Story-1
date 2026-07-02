using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class OptionsManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;

        // Note: UI Sliders MUST have a Min Value of 0.0001 (not 0) to prevent a Mathf.Log10(0) error!
        // Max Value should be set to 1.

        public void OnMasterChange(float value)
        {
            mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        }

        public void OnBGMChange(float value)
        {
            mixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
        }

        public void OnSFXChange(float value)
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        }
    }
}