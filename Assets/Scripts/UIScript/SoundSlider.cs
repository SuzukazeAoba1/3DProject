using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public Slider volumeBGM;
    public Slider volumeSFX;
    // Start is called before the first frame update
    private void Start()
    {
        volumeBGM.value = AudioManager.instance.bgmVolume;
        volumeSFX.value = AudioManager.instance.sfxVolume;
    }


    public void BGMVolume()
    {
        AudioManager.instance.BGMVolume(volumeBGM.value);
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(volumeSFX.value);
    }
}
