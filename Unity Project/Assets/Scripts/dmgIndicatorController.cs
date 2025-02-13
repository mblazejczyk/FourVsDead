using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dmgIndicatorController : MonoBehaviour
{
    void Awake()
    {
        gameObject.GetComponent<Animator>().SetInteger("animationId", Random.Range(0, 2));
        StartCoroutine(destroyMe());
    }

    public void SetDmg(int dmg){
        gameObject.GetComponent<TMP_Text>().text = "" + dmg;
        Debug.Log("dmg indicated" + transform.position);
    }

    IEnumerator destroyMe(){
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
