using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject playerNamePrefab;
    [SerializeField] private Transform playerNamesContainer;
    [SerializeField] private InputField chatText;
    [SerializeField] private TMP_InputField chatLog;
    [SerializeField] private Scrollbar scrollbar;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        photonView.RPC("CheckPlayers",RpcTarget.All);
    }

    [PunRPC] // call all clients to execute this method
    void CheckPlayers()
    {
        ClearPlayerNames();
        
        var players = PhotonNetwork.PlayerList;
        
        foreach (var player in players)
        {
            var nickname = player.NickName;
            Transform n = Instantiate(playerNamePrefab.transform, playerNamesContainer);
            Text t = n.GetComponent<Text>();
            t.text = nickname;
        }
    }

    // call all clients to execute this method
    [PunRPC] 
    void UpdateChatLog(string message, PhotonMessageInfo info)
    {
        chatLog.text += $"{info.Sender.NickName}: {message}\n";
        scrollbar.value = 1;
    }

    private void ClearPlayerNames()
    {
        foreach (Transform child in playerNamesContainer)
            Destroy(child.gameObject);
    }

    // called via button onclick
    public void SubmitChat()
    {
        chatText.ActivateInputField();
        
        if (chatText.text == string.Empty)
            return;
        
        // execute rpc and send the chat message input of this client
        photonView.RPC("UpdateChatLog",RpcTarget.AllBuffered, chatText.text);
        chatText.text = String.Empty;
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        photonView.RPC("CheckPlayers",RpcTarget.All);
    }
}
