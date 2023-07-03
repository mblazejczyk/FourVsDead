using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNextTutorial : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("TutorialController").GetComponent<TutorialController>().StartNextTutorial();
        Destroy(gameObject);
    }
}
