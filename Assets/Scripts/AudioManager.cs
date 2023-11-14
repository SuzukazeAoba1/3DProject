using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    
    public AudioClip[] bgmClip;
    public float bgmVolume;
    public Slider bgmslider;

    public AudioClip[] sfxClip;
    public float sfxVolume;
    public Slider sfxslider;

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

    public void BGMSliderChange()
    {
        bgmslider.value = bgmVolume;
    }

    public void SFXSliderChange()
    {
        sfxslider.value = sfxVolume;
    }

}
