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
    public AudioSource bgmSource;
    AudioHighPassFilter bgmEffect;

    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int sfxChannels;
    public AudioSource[] sfxSource;

    public AudioClip boosterClip;
    public AudioSource boosterSource;

    public AudioClip footClip;
    public AudioSource footSource;

    int channelIndex;

    public enum Sfx
    {
        BrakingSound,
        ButtonSelect,
        CountDown,
        FstJump,
        FstJump2,
        GameOverVoice,
        Hurdle,
        KnockBack,
        MapSelect,
        OuchVoice,
        SndJump,
        SndJump2,
        StageStart,
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

    private void Start()
    {
        PlayBgm(true, 1);
    }

    void initVolume()
    {
        bgmSource.volume = bgmVolume;
        boosterSource.volume = sfxVolume;
        footSource.volume = sfxVolume;
        for (int index = 0; index < sfxSource.Length; index++)
        {
            sfxSource[index].volume = sfxVolume;
        }
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmSource = bgmObject.AddComponent<AudioSource>();
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        bgmSource.playOnAwake = false; 
        bgmSource.loop = true;

        GameObject boosterSound = new GameObject("BoosterPlayer");
        boosterSound.transform.parent = transform;
        boosterSource = boosterSound.AddComponent<AudioSource>();
        boosterSource.loop = true;
        boosterSource.playOnAwake = false;

        GameObject footSound = new GameObject("FootPlayer");
        footSound.transform.parent = transform;
        footSource = footSound.AddComponent<AudioSource>();
        footSource.loop = true;
        footSource.playOnAwake = false;

        GameObject sfxObject = new GameObject("sfxSource");
        sfxObject.transform.parent = transform;
        sfxSource = new AudioSource[sfxChannels];

        for(int index=0; index<sfxSource.Length; index++)
        {
            sfxSource[index] = sfxObject.AddComponent<AudioSource>();
            sfxSource[index].playOnAwake = false;
            sfxSource[index].bypassListenerEffects = true;
        }

        initVolume();
    }

    public void PlayBgm(bool isPlay, int bgmIndex = 1)
    {
        if (isPlay)
        {
            if (bgmSource != null)  
            {
                if (bgmIndex == 1)
                    bgmSource.clip = bgmClip[0];
                else if (bgmIndex == 2)
                    bgmSource.clip = bgmClip[1];

                bgmSource.Play();
            }
        }
        else
        {
            if (bgmSource != null)  
            {
                bgmSource.Stop();
            }
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxSource.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxSource.Length;

            if (sfxSource[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxSource[loopIndex].clip = sfxClip[(int)sfx];
            sfxSource[loopIndex].Play();

            break;
        }
    }

    public void PlayBooster()
    {
        if (boosterSource != null)
            boosterSource.clip = boosterClip;

        boosterSource.Play();
    }

    public void StopBooster()
    {
        boosterSource.Stop();
    }

    public void PlayFoot()
    {
        if (footSource != null)
            footSource.clip = footClip;

        footSource.Play();
    }

    public void StopFoot()
    {
        footSource.Stop();
    }

    public void SFXVolume(float volume)
    {
        sfxVolume = volume;
        for (int index = 0; index < sfxSource.Length; index++)
        {
            sfxSource[index].volume = sfxVolume;
        }
        boosterSource.volume = sfxVolume;
        footSource.volume = sfxVolume;
    }

    public void BGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = bgmVolume;
    }

}
