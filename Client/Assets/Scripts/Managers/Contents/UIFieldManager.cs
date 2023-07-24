using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class UIFieldManager : MonoBehaviour
{
    private static UIFieldManager _instance = null;
    public static UIFieldManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    // 채팅창
    [SerializeField]
    Text ChatLog;
    
    public InputField ChatMsg;

    [SerializeField]
    Scrollbar vScrollBar;

    public GameObject panelChat;

    // 인게임 UI
    [SerializeField]
    TMP_Text coinText;

    private const int MaxChatLogWidth = 20;


    // 마이룸 UI
    public GameObject MyRoomUI;


    // NPC UI
    public GameObject panelNPC;


    private void Start()
    {
        panelChat.SetActive(false);
        MyRoomUI.SetActive(false);
        panelNPC.SetActive(false);
    }

    public void SendChatMsg()
    {
        string chatStr = ChatMsg.text;

        C_Chat chat = new C_Chat()
        {
            Context = chatStr
        };
        Managers.Network.Send(chat);

        ChatMsg.text = null;
        ChatMsg.DeactivateInputField();
    }

    public void RecvChatMsg(string id, string chatStr)
    {
        string recvChat;
        if (chatStr.Length > MaxChatLogWidth)
        {
            recvChat = (chatStr.Substring(0, MaxChatLogWidth) + "\n" + (chatStr.Substring(MaxChatLogWidth)));
        }
        else
        {
            recvChat = chatStr;
        }
        ChatLog.text += $"{id} >> {recvChat}\n";
        vScrollBar.value = 0;
    }

    public void CoinView(string coin)
    {
        coinText.text = coin;
        InventoryManager.Instance.moneyText.text = coin;
    }



}
