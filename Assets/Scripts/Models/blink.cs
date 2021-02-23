using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blink : MonoBehaviour
{
    public Texture[] eyes;
    public Material material;
    void Start()
    {
        StartCoroutine(blinks(false));
    }
     IEnumerator blinks(bool closed)
    {
    
        if (closed)
        {
            yield return new WaitForSeconds(Random.Range(1f, 2.2f));
            material.mainTexture = eyes[1];
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(.2f, .5f));
            material.mainTexture = eyes[0];
        }
        StartCoroutine(blinks(!closed));
    }
}
