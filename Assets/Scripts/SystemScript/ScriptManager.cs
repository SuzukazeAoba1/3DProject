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
            Debug.Log("충돌");
        }
    }

    void Update()
    {
        Debug.Log(isPrint);
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
                printScript = "c키를 누르면 점프할 수 있어!";
                break;

            case 3:
                printScript = "c키를 두번 누르면 2단 점프가 가능해!";
                break;

            case 4:
                printScript = "이거 뭐라씀? 넉백... 당할때\n착지직전 z쓰면 부스터 가능 알아서 고쳐주셈 이거";
                break;

            case 5:
                printScript = "부스터 설명";
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
