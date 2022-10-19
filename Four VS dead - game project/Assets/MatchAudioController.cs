using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchAudioController : MonoBehaviour
{
    public AudioClip[] waveBegin;
    public AudioClip[] areaUnlocked;

    public void PlaySound(int soundId)
    {
        switch (soundId)
        {
            case 0:
                gameObject.GetComponent<AudioSource>().clip = waveBegin[Random.Range(0, waveBegin.Length)];
                gameObject.GetComponent<AudioSource>().Play();
                break;
            case 1:
                gameObject.GetComponent<AudioSource>().clip = areaUnlocked[Random.Range(0, areaUnlocked.Length)];
                gameObject.GetComponent<AudioSource>().Play();
                break;
        }
    }
}
