using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartScript : MonoBehaviour
{
    public TextMeshProUGUI scriptMessage;
    // Start is called before the first frame update
    void Start()
    {
        scriptMessage.text = "��� Ÿ�ֿ̹� ��(���� ȭ��ǥ)�� ������\n�� ���� �� �� �־�!\n�������� �ʰ� ������!";
        StartCoroutine(PrintOff());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PrintOff()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }

}
