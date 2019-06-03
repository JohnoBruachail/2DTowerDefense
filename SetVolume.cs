using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer effectsMixer;

    public void SetMasterLevel(float sliderValue){
        //represents the slider value as a logarithm to the base of ten then multiplies it by 20
        //takes the slider value and converts it into a value between -80 and 0 on a logmarithmic scale
        musicMixer.SetFloat ("MusicVol", Mathf.Log10(sliderValue) * 20);
        effectsMixer.SetFloat ("EffectsVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicLevel(float sliderValue){
        //represents the slider value as a logarithm to the base of ten then multiplies it by 20
        //takes the slider value and converts it into a value between -80 and 0 on a logmarithmic scale
        musicMixer.SetFloat ("MusicVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetEffectsLevel(float sliderValue){
        //represents the slider value as a logarithm to the base of ten then multiplies it by 20
        //takes the slider value and converts it into a value between -80 and 0 on a logmarithmic scale
        effectsMixer.SetFloat ("EffectsVol", Mathf.Log10(sliderValue) * 20);
    }
}
