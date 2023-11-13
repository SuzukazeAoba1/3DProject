using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnMainButton()
    {
        SceneManager.LoadScene("MainUI");
    }

    public void GameSelectButton()
    {
        SceneManager.LoadScene("StartUI");
    }

    public void GameStartButton()
    {
        SceneManager.LoadScene("ObjectScene");
    }

    public void ReturnStageButton()
    {
        SceneManager.LoadScene("StartUI");
    }

    public void GoOptionButton()
    {
        SceneManager.LoadScene("OptionUI");
    }

    public void ExitButton()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif

    }
}
