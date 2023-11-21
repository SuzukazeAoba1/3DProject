using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coilldercheck : MonoBehaviour
{
    public float destorytime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RockDestroy());
    }

    IEnumerator RockDestroy()
    {
        yield return new WaitForSeconds(destorytime);
        Destroy(this.gameObject);
    }

}
