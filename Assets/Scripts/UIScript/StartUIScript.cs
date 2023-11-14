using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartUIScript : MonoBehaviour
{
    public List<MapData> maps;
    public Image mapNameImage;
    public Image mapSetImage;
    public TextMeshProUGUI mapInfoText;
    public Button chekcButton;

    // Start is called before the first frame update
    void Awake()
    {
    }

    private void Start()
    {
        MapUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MapUpdate()
    {
        maps[0].get(1, "Ʃ�丮�� ��", "������ ������ ���� Ʃ�丮�� �� (����� �����? �˾Ƽ� ��������)");
        maps[1].get(2, "???", "������Ʈ ����");
    }

    public void OnClickTutoMap()
    {
        mapInfoText.text = maps[0].setInfo();
    }

    public void onClickWholeMap()
    {
        mapInfoText.text = maps[1].setInfo();
    }
}
