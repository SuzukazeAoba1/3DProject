using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScriptManager : MonoBehaviour
{
    public GameObject scriptCanvas;
    public TextMeshProUGUI scriptMessage;

    string printScript;

    public int scriptNum;
    public bool isPrint;

    private void Awake()
    {
        isPrint = false;
        InitScript();
    }

    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isPrint = true;
        }
    }

    void Update()
    {
        if (isPrint)
        {

            scriptMessage.text = printScript;
            scriptCanvas.SetActive(true);
            StartCoroutine(PrintOff());
        }

    }

    void InitScript()
    {
        switch(scriptNum)
        {
            case 2:
                printScript = "Z키를 누르면 부스터를,\nC키를 누르면 점프를 할 수 있어!";
                break;

            case 3:
                printScript = "C키를 두번 누르면 2단 점프가 가능해!";
                break;

            case 4:
                printScript = "튕겨나는 장애물에 당했을 때,\n바닥에 닿기 직전 Z를 누르면 착지 부스터가 가능해!";
                break;

            case 5:
                printScript = "노란 화살 장판을 밟으면\n2초간 최고 속도로 달릴 수 있어!";
                break;

        }
    }

    IEnumerator PrintOff()
    {
        yield return new WaitForSeconds(3f);
        scriptCanvas.SetActive(false);
        gameObject.SetActive(false);
        isPrint = false;
    }
}
