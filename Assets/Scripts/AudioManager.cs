using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public AudioClip[] bgmClip;
    public float bgmVolume;
    public Slider bgmslider;
    public AudioSource bgmSource;

    public AudioClip[] sfxClip;
    public float sfxVolume;
    public Slider sfxslider;
    public AudioSource sfxSource;

    public enum Bgm
    {
        MainBgm,
        TutorialBgm,
    }

    public enum Sfx
    {
        FirstJump,
        SecondJump,
        GameOverVoice,
        MapSelect,
        FootSound,
        ButtonSound,
        BoosterSound,
        StartSound,
        KnockBackSound,
        OuchSound,
        HurdleSound,
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        bgmslider.value = bgmVolume;
        sfxslider.value = sfxVolume;
    }

    public void PlayBgm()
    {

    }

    public void PlaySfx()
    {

    }

    public void BgmVolume()
    {

    }

    public void SFXVolume()
    {

    }

}
