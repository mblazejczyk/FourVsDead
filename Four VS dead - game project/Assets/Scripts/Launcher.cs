using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameIF;
    [SerializeField] TMP_InputField roomSpaceIF;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_InputField roomPassword;
    [SerializeField] Transform roomlistContent;
    [SerializeField] Transform PlayerlistContent;
    [SerializeField] GameObject roomListitemPrefab;
    [SerializeField] GameObject PlayerListitemPrefab;
    [SerializeField] GameObject StartGameButton;
    public GameObject menuButtons;
    public ChatSystem chat;


    private void Awake()
    {
        Instance = this;
        Cursor.visible = true;
    }
    void Start()
    {
        GameObject.FindGameObjectWithTag("DiscordController").GetComponent<DiscordController>().Change("in_mainmenu", "In main menu", "Main menu", "starring at main menu screen");
    }

    public void ConnectToPhoton()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        menuButtons.SetActive(true);
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        MenuManager.Instance.OpenMenu("title");
        GameObject.FindGameObjectWithTag("DiscordController").GetComponent<DiscordController>().Change("in_mainmenu", "In main menu", "Main menu", "looking around menu");
        PhotonNetwork.NickName = GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().login;
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameIF.text))
        {
            return;
        }
        RoomOptions roomOptions = new RoomOptions();
        if (roomSpaceIF.text == "")
        {
            roomSpaceIF.text = "4";
        }
        else
        {
            if (int.Parse(roomSpaceIF.text) > 4)
            {
                roomSpaceIF.text = "4";
            }
            else if (int.Parse(roomSpaceIF.text) < 1)
            {
                roomSpaceIF.text = "1";
            }
        }
        byte mxp = byte.Parse(roomSpaceIF.text);
        Debug.Log(mxp);
        roomOptions.MaxPlayers = mxp;

        if (roomPassword.text != "")
        {
            roomNameIF.text += "+PRIVATE_PASSWORD:" + roomPassword.text;
        }
        
        PhotonNetwork.CreateRoom(roomNameIF.text, roomOptions, null);
        MenuManager.Instance.OpenMenu("loading");
        roomNameIF.text = "";
        roomPassword.text = "";
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        menuButtons.SetActive(false);
        if (PhotonNetwork.CurrentRoom.Name.Contains("+PRIVATE_PASSWORD:"))
        {
            string replaced = PhotonNetwork.CurrentRoom.Name.Replace("+PRIVATE_PASSWORD:", "▓");
            string[] splited = replaced.Split('▓');
            roomNameText.text = splited[0];
        }
        else
        {
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        }

        foreach (Transform child in PlayerlistContent)
        {
            Destroy(child.gameObject);
        }

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(PlayerListitemPrefab, PlayerlistContent).GetComponent<PlayerListitem>().SetUp(players[i]);
        }

        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
        chat.SendSystemMessage("New player joined: " + PhotonNetwork.NickName);
        GameObject.FindGameObjectWithTag("DiscordController").GetComponent<DiscordController>().Change("in_room", "In room",  "In room: " + roomNameText.text + "(" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + ")", "preparing for battle");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room creation failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");

    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomlistContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList) { continue; }
            Instantiate(roomListitemPrefab, roomlistContent).GetComponent<RoomListitem>().SetUp(roomList[i]);
        }
    }

    public void JoinRoom(RoomInfo info)
    {
        if(info.PlayerCount >= info.MaxPlayers)
        {
            MenuManager.Instance.OpenMenu("error");
            errorText.text = "Max players in room";
            return;
        }
        
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListitemPrefab, PlayerlistContent).GetComponent<PlayerListitem>().SetUp(newPlayer);
        
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        StartCoroutine(StartGameCountdown());
    }

    IEnumerator StartGameCountdown()
    {
        for(int i = 5; i > 0; i--)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                chat.SendSystemMessage("Game starting in " + i + "...");
            }
            yield return new WaitForSeconds(1);
        }
        chat.SendSystemMessage("Game starts now...");
        PhotonNetwork.LoadLevel(2);
    }

    public GameObject passwordPrompt;
    private string correctPassword = "";
    public void AskForPassword(RoomInfo choosenRoom, string correctPass)
    {
        choosenRoomInfo = choosenRoom;
        correctPassword = correctPass;
        passwordPrompt.SetActive(true);
    }

    [HideInInspector] public RoomInfo choosenRoomInfo;
    public void ConfirmPassword(TMP_InputField password)
    {
        passwordPrompt.SetActive(false);
        if (correctPassword != password.text)
        {
            MenuManager.Instance.OpenMenu("error");
            errorText.text = "Password incorrect";
            return;
        }
        else
        {
            JoinRoom(choosenRoomInfo);
        }
    }

    public void ChangeServer(string server)
    {
        MenuManager.Instance.OpenMenu("loading");
        PhotonNetwork.Disconnect();
        if(server == "best")
        {
            PhotonNetwork.ConnectToBestCloudServer();
        }
        else
        {
            PhotonNetwork.ConnectToRegion(server);
        }
    }
}
