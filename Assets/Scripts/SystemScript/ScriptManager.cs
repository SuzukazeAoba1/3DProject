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
                printScript = "ZŰ�� ������ �ν��͸�,\nCŰ�� ������ ������ �� �� �־�!";
                break;

            case 3:
                printScript = "CŰ�� �ι� ������ 2�� ������ ������!";
                break;

            case 4:
                printScript = "ƨ�ܳ��� ��ֹ��� ������ ��,\n�ٴڿ� ��� ���� Z�� ������ ���� �ν��Ͱ� ������!";
                break;

            case 5:
                printScript = "��� ȭ�� ������ ������\n2�ʰ� �ְ� �ӵ��� �޸� �� �־�!";
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
