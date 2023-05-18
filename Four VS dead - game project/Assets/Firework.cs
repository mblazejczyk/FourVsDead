using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class Firework : MonoBehaviour
{
    public Color[] c;
    void Start()
    {
        int rand = Random.Range(0, c.Length);
        gameObject.GetComponent<SpriteRenderer>().color = c[rand];
        gameObject.GetComponent<Light2D>().color = c[rand];
        gameObject.GetComponent<AudioSource>().pitch = Random.Range(0, 2);
        gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(destroyme());
    }

    IEnumerator destroyme()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
