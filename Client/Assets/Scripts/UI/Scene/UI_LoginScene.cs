using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

public class UI_LoginScene : UI_Scene
{
    private static UI_LoginScene _instance = null;
    public static UI_LoginScene Instance
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


    public TMP_Text notification;
    public TMP_InputField textId;
    public TMP_InputField textPw;


    enum Buttons
    {
        ButtonRegister,
        ButtonLogin
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.ButtonRegister).gameObject.BindEvent(OnClickRegisterButton);
        GetButton((int)Buttons.ButtonLogin).gameObject.BindEvent(OnClickLoginButton);
    }

    public void OnClickRegisterButton(PointerEventData evt)
    {
        //Debug.Log("ȸ������ Ŭ��");
        string id = textId.text;
        string pw = textPw.text;
        SaveUserData();
    }

    public void OnClickLoginButton(PointerEventData evt)
    {
        //Debug.Log("�α��� Ŭ��");
        string id = textId.text;
        string pw = textPw.text;
        CheckUserData();
    }
    #region ȸ������
    public void SaveUserData()
    {
        if (!CheckInput(textId.text, textPw.text))
        {
            return;
        }
        SendRegReq();
    }

    private void SendRegReq()
    {   
        C_Reg reg = new C_Reg()
        {
            UniqueId = textId.text,
            Password = textPw.text
        };
        Managers.Network.Send(reg);

        Debug.Log(reg);
    }
    #endregion

    #region �α���
    public void CheckUserData()
    {
        if (!CheckInput(textId.text, textPw.text))
        {
            return;
        }

        SendLoginReq();
    }

    private void SendLoginReq()
    {

        C_Login login = new C_Login()
        {
            UniqueId = textId.text,
            Password = textPw.text,
        };

        Managers.Network.Send(login);

    }

    #endregion

    bool CheckInput(string id, string pwd)
    {
        if (id == "" || pwd == "")
        {
            notification.text = "Please check your ID / PASS";
            return false;
        }
        else
        {
            return true;
        }
    }


    public void NotiText(string text)
    {
        notification.text = text;
    }


    public void GameExit()
    {
        Application.Quit();
    }
}
