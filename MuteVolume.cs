using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MuteVolume : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer effectsMixer;

    public GameObject muteButton;
    public GameObject unMuteButton;

    public void Mute(){

        musicMixer.SetFloat ("MusicVol", -80);
        effectsMixer.SetFloat ("EffectsVol", -80);

        unMuteButton.SetActive(true);
        muteButton.SetActive(false);

    }

    public void UnMute(){

        musicMixer.SetFloat ("MusicVol", 0);
        effectsMixer.SetFloat ("EffectsVol", 0);
        muteButton.SetActive(true);
        unMuteButton.SetActive(false);
    }
}
