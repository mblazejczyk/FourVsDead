using UnityEngine;

public class ButtonSfxController : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioClip[] upgradeBtnClip;

    public void PlaySound()
    {
        gameObject.GetComponent<AudioSource>().clip = clips[Random.Range(0, clips.Length)];
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void PlayUpgrade()
    {
        gameObject.GetComponent<AudioSource>().clip = upgradeBtnClip[Random.Range(0, upgradeBtnClip.Length)];
        gameObject.GetComponent<AudioSource>().Play();
    }
}
