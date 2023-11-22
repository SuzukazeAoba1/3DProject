using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ButtonTextScript : MonoBehaviour
{
    public void GameSelectButton()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonSelect);
        SceneManager.LoadScene("StartUI");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void GoOptionButton()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ButtonSelect);
        SceneManager.LoadScene("OptionUI");
    }
}
