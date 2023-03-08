using Discord;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscordController : MonoBehaviour
{
    public long applicationID;
    [Space]
    public string details = "inroom";
    public string state = "Current velocity: ";
    [Space]
    public string largeImage = "logo";
    public string largeText = "Four vs Dead";
    public string smallImage = "";
    public string smallText = "";

    private long time;

    private static bool instanceExists;
    public Discord.Discord discord;

    void Awake()
    {
        // Transition the GameObject between scenes, destroy any duplicates
        if (!instanceExists)
        {
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Log in with the Application ID
        discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);

        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        UpdateStatus();
    }
    private bool OneDisplay = false;
    void Update()
    {
        // Destroy the GameObject if Discord isn't running
        try
        {
            discord.RunCallbacks();
            if (!OneDisplay)
            {
                OneDisplay = true;
                GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMPro.TMP_Text>().text = "<color=green>Connected to Discord</color>";
                GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", true);
                StartCoroutine(Infobox(false));
            }
        }
        catch
        {
            GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMPro.TMP_Text>().text = "<color=red>Couldn't connect to Discord</color>";
            GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", true);
            StartCoroutine(Infobox(true));
        }

    }
    IEnumerator Infobox(bool i)
    {
        if (i)
        {
            yield return new WaitForSeconds(3);
            GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", false);
            yield return new WaitForSeconds(3);
            Destroy(gameObject);

        }
        else
        {
            yield return new WaitForSeconds(3);
            GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", false);
        }
    }
    void LateUpdate()
    {
        UpdateStatus();
    }

    void UpdateStatus()
    {
        // Update Status every frame
        try
        {
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                Details = details,
                State = state,
                Assets =
                {
                    LargeImage = largeImage,
                    LargeText = largeText,
                    SmallImage = smallImage,
                    SmallText = smallText
                },
                Timestamps =
                {
                    Start = time
                }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok) Debug.LogWarning("Failed connecting to Discord!");
            });
        }
        catch
        {
            if (!OneDisplay)
            {
                OneDisplay = true;
                GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMPro.TMP_Text>().text = "<color=red>Discord disconnected</color>";
                GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", true);
                StartCoroutine(Infobox(true));
            }
        }
    }

    public void Change(string img, string sTxt, string det, string st)
    {
        smallImage = img;
        smallText = sTxt;
        details = det;
        state = st;
        UpdateStatus();
    }
}