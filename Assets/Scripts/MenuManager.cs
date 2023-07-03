using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public Animator PlayButtons;
    [SerializeField] Menu[] menus;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if(menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if(menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        if(PlayButtons.GetCurrentAnimatorStateInfo(0).IsName("idle_open_btn"))
        {
            PlayButtons.SetTrigger("close");
        }
    }

    public void StartGame()
    {
        string menuName = "loading";
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void ResetTrigger()
    {
        PlayButtons.SetTrigger("close");
        
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        if (PlayButtons.GetCurrentAnimatorStateInfo(0).IsName("idle_open_btn"))
        {
            PlayButtons.SetTrigger("close");
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void Quit()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }

    public void LogOut()
    {
        PhotonNetwork.Disconnect();
        Destroy(GameObject.FindGameObjectWithTag("RoomManager"));
        SceneManager.LoadScene(0);
    }

    public void OpenButtons()
    {
        PlayButtons.SetTrigger("open");
    }
}
