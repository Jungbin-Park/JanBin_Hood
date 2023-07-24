using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

class PacketHandler
{
	public static void S_ChatHandler(PacketSession session, IMessage packet)
	{
		S_Chat chatPacket = packet as S_Chat;
		ServerSession serverSession = session as ServerSession;
        Debug.Log("S_ChatHandler");
		//Debug.Log($"{chatPacket.PlayerId}:{chatPacket.Context}");

        UIFieldManager.Instance.RecvChatMsg(chatPacket.PlayerId, chatPacket.Context);
        
    }

	public static void S_EnterGameHandler(PacketSession session, IMessage packet)
	{
        // 게임 입장시 플레이어 정보 보내줌
		S_EnterGame enterGamePacket = packet as S_EnterGame;

        Managers.Object.Add(enterGamePacket.Player, myPlayer: true);

        UIFieldManager.Instance.CoinView(enterGamePacket.Coin);
        

        Debug.Log("S_EnterGameHandler");
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_LeaveGame leaveGamePacket = packet as S_LeaveGame;

        Managers.Object.Clear();

        Debug.Log("S_LeaveGameHandler");
    }
    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;

        foreach(ObjectInfo obj in spawnPacket.Objects)
        {
            Managers.Object.Add(obj, myPlayer: false);
        }

        Debug.Log("S_SpawnHandler");
    }
    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawnPacket = packet as S_Despawn;

        foreach (int id in despawnPacket.ObjectIds)
        {
            Managers.Object.Remove(id);
        }

        Debug.Log("S_DespawnHandler");
    }

    // 서버에 응답
    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_Move movePacket = packet as S_Move;

        GameObject go = Managers.Object.FindById(movePacket.ObjectId);
        if (go == null)
            return;

        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return;

        bc.PosInfo = movePacket.PosInfo;
    }
    
    public static void S_RegHandler(PacketSession session, IMessage packet)
    {  
        UI_LoginScene uiLoginScene = new UI_LoginScene();
        S_Reg regPacket = packet as S_Reg;
        if(regPacket.RegOk == 1)
        {
            UI_LoginScene.Instance.NotiText("Registration Success");
        }
        else
        {
            UI_LoginScene.Instance.NotiText("ID already exists");
        }
    }

    public static void S_ConnectedHandler(PacketSession session, IMessage packet)
    {
        S_Connected connectedPacket = new S_Connected();
        Debug.Log("S_connectedHandler");
    }
    
    public static void S_LoginHandler(PacketSession session, IMessage packet)
    {
        UI_LoginScene uiLoginScene = new UI_LoginScene();
        S_Login loginPacket = packet as S_Login;

        if(loginPacket.LoginOk == 1)
        {
            UI_LoginScene.Instance.NotiText($"'{UI_LoginScene.Instance.textId.text}' Login Success ");

            Managers.Scene.LoadScene(Define.Scene.Game);

            C_Connected resConnectedPacket = new C_Connected();
            resConnectedPacket.ConnectedOk = true;
            resConnectedPacket.MapId = 1;
            Managers.Network.Send(resConnectedPacket);
        }
        else
        {
            UI_LoginScene.Instance.NotiText($"Login Fail");
        }
    }

    public static void S_PortalHandler(PacketSession session, IMessage packet)
    {
        S_Portal csPacket = packet as S_Portal;
        Debug.Log("S_PortalHandler");

        GameObject go = Managers.Object.FindById(csPacket.PlayerId);
        if (go == null)
            return;

        CreatureController cc = go.GetComponent<CreatureController>();
        if (cc == null)
            return;

        go.transform.position =
            new Vector3Int(csPacket.PosInfo.PosX, csPacket.PosInfo.PosY, 0) + new Vector3(0.5f, 0.5f); ;
        cc.CellPos = new Vector3Int(csPacket.PosInfo.PosX, csPacket.PosInfo.PosY, 0);
        

    }

    public static void S_SkillHandler(PacketSession session, IMessage packet)
    {
        S_Skill skillPacket = packet as S_Skill;

        GameObject go = Managers.Object.FindById(skillPacket.ObjectId);
        if (go == null)
            return;

        CreatureController cc = go.GetComponent<CreatureController>();
        if(cc != null)
        {
            cc.UseSkill(skillPacket.Info.SkillId);
        }
    }

    public static void S_ChangeHpHandler(PacketSession session, IMessage packet)
    {
        S_ChangeHp changePacket = packet as S_ChangeHp;

        GameObject go = Managers.Object.FindById(changePacket.ObjectId);
        if (go == null)
            return;

        CreatureController cc = go.GetComponent<CreatureController>();
        if (cc != null)
        {
            cc.Hp = changePacket.Hp;
            // TODO : UI
            Debug.Log($"ChangeHP : {cc.Hp}");
        }
    }

    public static void S_DieHandler(PacketSession session, IMessage packet)
    {
        S_Die diePacket = packet as S_Die;

        GameObject go = Managers.Object.FindById(diePacket.ObjectId);
        if (go == null)
            return;

        CreatureController cc = go.GetComponent<CreatureController>();
        if (cc != null)
        {
            cc.Hp = 0;
            cc.OnDead();
        }
        
    }

    public static void S_MyroomSaveHandler(PacketSession session, IMessage packet)
    {
        // 저장
    }

    public static void S_MyroomLoadHandler(PacketSession session, IMessage packet)
    {
        S_MyroomLoad smyroomLoadPacket = packet as S_MyroomLoad;

        Debug.Log($"S_MyroomLoadHandler - smyroomLoadPacket - {smyroomLoadPacket.PlayerId}");
        Debug.Log($"S_MyroomLoadHandler -smyroomLoadPacket - {smyroomLoadPacket.Buildlist}");

        string data = smyroomLoadPacket.Buildlist;
        Debug.Log($"data - {data}");
        // 로드응답
        CraftManual.Instance.LoadObjects(data);
    }

}
