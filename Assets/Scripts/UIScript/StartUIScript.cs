using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartUIScript : MonoBehaviour
{
    public int stageId;

    public GameObject tutorialMapImage;
    public GameObject SecondMapImage;

    public Image mapSetImage;
   
    public TextMeshProUGUI mapInfoText;
    public Button chekcButton;

    // Start is called before the first frame update
    void Awake()
    {
        stageId = 0;
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickTutoMap()
    {
        stageId = 1;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.MapSelect);
        string tempText = "초보자를 위한\n튜토리얼 맵";
        mapInfoText.text = tempText;
        chekcButton.interactable = true;

        tutorialMapImage.SetActive(true);
        SecondMapImage.SetActive(false);
    }

    public void onClickWholeMap()
    {
        stageId = 2;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.MapSelect);
        string tempText = "사막의 진귀한\n보물을 찾아라";

        SecondMapImage.SetActive(true);
        tutorialMapImage.SetActive(false);

        mapInfoText.text = tempText;
        chekcButton.interactable = true;
    }

    public void onClickStartButton()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.StageStart);
        switch (stageId)
        {
            case 1:
                AudioManager.instance.PlayBgm(true, 2);
                SceneManager.LoadScene("Stage1");
                break;
            case 2:
                AudioManager.instance.PlayBgm(true, 3);
                SceneManager.LoadScene("Stage2");
                break;
        }
    }
}
