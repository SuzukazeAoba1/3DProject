using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
  
    public void ReturnMainButton()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonSelect);
        SceneManager.LoadScene("MainUI");
    }

    public void GameSelectButton()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonSelect);
        SceneManager.LoadScene("StartUI");
    }

    public void GameStartButton()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.StageStart);
        SceneManager.LoadScene("main");
        AudioManager.instance.PlayBgm(true, 2);
    }


    public void ReturnStageButton()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonSelect);
        SceneManager.LoadScene("StartUI");

        
        AudioManager.instance.PlayBgm(true, 1);
    }

    public void GoOptionButton()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonSelect);
        SceneManager.LoadScene("OptionUI");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
