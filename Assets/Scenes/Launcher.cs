using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField nickname;

    private void Start()
    {
        if (PlayerPrefs.HasKey("nickname"))
            nickname.text = PlayerPrefs.GetString("nickname");
    }

    public void QuickConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("Open Room");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Chat");
    }

    public void SetNickname(string nickname)
    {
        PlayerPrefs.SetString("nickname",nickname);
        PhotonNetwork.NickName = nickname;
    }
}
