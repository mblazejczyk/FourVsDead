using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public float after = 2f;
    public bool toDestroy = false;
    public void Start()
    {
        if (toDestroy)
        {
            StartCoroutine(life());
        }
    }

    IEnumerator life()
    {
        yield return new WaitForSeconds(after);
        Destroy(gameObject);
    }
}
