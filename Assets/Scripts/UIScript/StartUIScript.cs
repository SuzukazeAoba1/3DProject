using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartUIScript : MonoBehaviour
{
    public int stageId;

    public Sprite tutorialMapImage;

    public Image mapSetImage;
    public GameObject stageImage;
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
        string tempText = "�ʺ��ڸ� ����\nƩ�丮�� ��";
        mapInfoText.text = tempText;
        chekcButton.interactable = true;
        stageImage.SetActive(true);
    }

    public void onClickWholeMap()
    {
        stageId = 2;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.MapSelect);
        string tempText = "���� ��";
        stageImage.SetActive(false);
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
                SceneManager.LoadScene("main");
                break;
            case 2:
                SceneManager.LoadScene("2Stage");
                break;
        }
    }
}
