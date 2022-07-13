using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerController : MonoBehaviour
{

    

    [SerializeField] AudioMixer myAudioMixer;

    private void Awake()
    {
        GetComponent<Slider>().value = 0.6f;
        
    }


    public void SetMusicVolume(float sliderValue) 
    {

        myAudioMixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    

    }

    public void SetSFXVolume(float sliderValue)
    {
        //convert slider value to an algorithem value based 10 ,and we *20 times so we can take it into account
        myAudioMixer.SetFloat("SFX",Mathf.Log10(sliderValue)*20);

    }
}
