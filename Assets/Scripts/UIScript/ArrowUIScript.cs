using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowUIScript : MonoBehaviour
{
    public Sprite image1;
    public Sprite image2;
    public float switchInterval = 0.1f;

    private Image spriteImage;


    private void OnEnable()
    {
        spriteImage = GetComponent<Image>();

        StartCoroutine(SwitchImages());
    }

    private IEnumerator SwitchImages()
    {
        while (true)
        {
            spriteImage.sprite = image1;
            yield return new WaitForSeconds(switchInterval);

            spriteImage.sprite = image2;
            yield return new WaitForSeconds(switchInterval);
        }
    }
}
