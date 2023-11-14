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
            Debug.Log("�浹");
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
                printScript = "cŰ�� ������ ������ �� �־�!";
                break;

            case 3:
                printScript = "cŰ�� �ι� ������ 2�� ������ ������!";
                break;

            case 4:
                printScript = "�̰� ����? �˹�... ���Ҷ�\n�������� z���� �ν��� ���� �˾Ƽ� �����ּ� �̰�";
                break;

            case 5:
                printScript = "�ν��� ����";
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
