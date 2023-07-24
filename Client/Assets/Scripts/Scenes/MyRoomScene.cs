using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRoomScene : BaseScene
{
    protected override void Init()    
    {
        base.Init();

        SceneType = Define.Scene.MyRoom;

        //Managers.Map.LoadMap(2);

        Screen.SetResolution(640, 480, false);

        //GameObject player = Managers.Resource.Instantiate("Creature/MyPlayer");
        //player.name = "Player";
        //Managers.Object.Add(player);

        //Managers.UI.ShowSceneUI<UI_Inven>();
        //Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        //gameObject.GetOrAddComponent<CursorController>();

        //GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        //Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        ////Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
        //GameObject go = new GameObject { name = "SpawningPool" };
        //SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        //pool.SetKeepMonsterCount(2);
    }

    public override void Clear()
    {

    }
}
