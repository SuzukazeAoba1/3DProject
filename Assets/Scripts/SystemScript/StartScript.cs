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
        scriptMessage.text = "출발 타이밍에 ↑(위쪽 화살표)를 누르면\n더 빨리 갈 수 있어!\n실패하지 않게 조심해!";
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
