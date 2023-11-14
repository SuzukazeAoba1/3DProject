using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartUIScript : MonoBehaviour
{
    public Sprite tutorialImage;
    public Sprite tutorialMapImage;
    public Sprite mapImage;

    public Image mapNameImage;
    public Image mapSetImage;
    public GameObject stageImage;
    public TextMeshProUGUI mapInfoText;
    public Button chekcButton;

    // Start is called before the first frame update
    void Awake()
    {
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
        mapNameImage.sprite = tutorialImage;
        string tempText = "�ʺ��ڸ� ����\nƩ�丮�� ��";
        mapInfoText.text = tempText;
        chekcButton.interactable = true;
        stageImage.SetActive(true);
    }

    public void onClickWholeMap()
    {
        mapNameImage.sprite = mapImage;
        string tempText = "���� ��";
        stageImage.SetActive(false);
        mapInfoText.text = tempText;
        chekcButton.interactable = false;
    }
}
