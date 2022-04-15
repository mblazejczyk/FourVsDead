using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    
    private void Update()
    {
        StartCoroutine(Dest()); 
        
    }
    IEnumerator Dest()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
