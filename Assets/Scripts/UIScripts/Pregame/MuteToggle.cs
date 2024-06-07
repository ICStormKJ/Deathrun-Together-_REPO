using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MuteToggle : MonoBehaviour
{
    [SerializeField]
    private AudioMixer bgmMixer;
    [SerializeField]
    private AudioMixer sfxMixer;
    public void Mute(bool mute)
    {
        if (mute)
        {
            bgmMixer.SetFloat("Volume", 0.0f);
            sfxMixer.SetFloat("Volume", 0.0f);
        }
        else
        {
            bgmMixer.SetFloat("Volume", 1.0f);
            sfxMixer.SetFloat("Volume", 1.0f);
        }
    }
}
