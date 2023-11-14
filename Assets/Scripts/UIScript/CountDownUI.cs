using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CountDownUI : MonoBehaviour
{
    private bool isCheck;

    public TextMeshProUGUI countText;
    public Canvas countDownCanvas;

    public int countDownStartNum;
    public float countDownNum;
    public string countDownEndMsg;

    // Start is called before the first frame update

    private void Start()
    {
        isCheck = false;
    }

    void Update()
    {
        
        if(GameManager.instance.startTimer <= 3f && !isCheck)
        {
            StartCountDown();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.CountDown);
        }
            
    }

    private void StartCountDown()
    {
        countDownNum = Mathf.CeilToInt(GameManager.instance.startTimer);
        countDownCanvas.gameObject.SetActive(true);

        StartCoroutine(CountDownCo());
    }

    private IEnumerator CountDownCo()
    {
        isCheck = true;

        if(countDownNum > 0)
        {
            countText.text = $"{countDownNum:N0}";
        }
        else if((countDownNum == 0))
        {
            countText.text = countDownEndMsg;
        }


        yield return new WaitForSeconds(1f);
        countDownNum--;
  
        if (countDownNum >= 0)
        {
            StartCoroutine(CountDownCo());
        }
       else
        {
            countDownCanvas.gameObject.SetActive(false);
        }

    }
}
