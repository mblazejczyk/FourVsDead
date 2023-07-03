using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialController : MonoBehaviour
{
    public GameObject captions;
    public TMP_Text captions_text;
    public int currentTutorial;
    public AudioClip[] clips;

    private int currentCaption;
    public string[] captionFragments;
    public float[] captionDelays;

    private void Start()
    {
        StartNextTutorial();
    }

    public void StartNextTutorial()
    {
        if (gameObject.GetComponent<AudioSource>().isPlaying) { return; }
        gameObject.GetComponent<AudioSource>().clip = clips[currentTutorial];
        gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(captionDelayer());
        currentTutorial++;
    }

    IEnumerator captionDelayer()
    {
        captions.SetActive(true);
        captions_text.text = captionFragments[currentCaption];
        yield return new WaitForSeconds(captionDelays[currentCaption]);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ModifyCoins(1, 99999);
        currentCaption++;
        if(currentCaption >= captionDelays.Length)
        {
            yield return new WaitForSeconds(6.11f);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().TutorialEnd();
        }
        else if (captionDelays[currentCaption] != 0)
        {
            StartCoroutine(captionDelayer());
        }
        else
        {
            captions_text.text = captionFragments[currentCaption];
            yield return new WaitForSeconds(6.11f);
            captions.SetActive(false);
        }
    }
}
