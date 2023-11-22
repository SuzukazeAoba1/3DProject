using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject image1;
    public GameObject image2;
    public float switchInterval;

    private void OnEnable()
    {
        StartCoroutine(SwitchImages());
    }

    private IEnumerator SwitchImages()
    {
        while (true)
        {
            image1.SetActive(true);
            image2.SetActive(false);
            Debug.Log("¹¹Áö?");
            yield return new WaitForSeconds(switchInterval);

            image1.SetActive(false);
            image2.SetActive(true);
            Debug.Log("¿ÖÁö?");
            yield return new WaitForSeconds(switchInterval);
        }
    }
}
