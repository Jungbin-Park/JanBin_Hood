using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameToField : MonoBehaviour
{
    private static GameToField _instance = null;
    public static GameToField Instance
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        MyPlayerController mc = collision.GetComponent<MyPlayerController>();

        if (collision.CompareTag("Player"))
        {
            collision.transform.position = new Vector3Int(-47, -4, 0) + new Vector3(0.5f, 0.5f);
            mc.CellPos = new Vector3Int(-47, -4, 0);
            //mc.PosInfo.PosX = -47;
            //mc.PosInfo.PosY = -4;
            //mc.PosInfo.State = CreatureState.Idle;

            C_Portal cpPacket = new C_Portal();
            cpPacket.PosInfo = mc.PosInfo;
            Managers.Network.Send(cpPacket);
        }

    }
}
