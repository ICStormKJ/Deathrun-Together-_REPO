using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    public void ChangeVolume(float slider)
    {
        mixer.SetFloat("Volume", Mathf.Log10(slider) * 20);
    }
}
