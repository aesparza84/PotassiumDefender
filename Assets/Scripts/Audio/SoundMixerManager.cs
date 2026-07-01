using UnityEngine;
using UnityEngine.Audio;
public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void SetMasterVol(float amount)
    {
        mixer.SetFloat("MasterVolume",amount);
    }
    public void SetSFXVol(float amount)
    {
        mixer.SetFloat("SoundFXVolume",amount);
    }
}
