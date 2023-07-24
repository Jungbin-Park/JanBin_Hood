using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Google.Protobuf.Protocol;

[System.Serializable]
public class Build
{
    public string buildName;
    public int buildItemNum;
    public GameObject go_Prefab;
    public GameObject go_PreviewPrefab;
}

[Serializable]
public struct BuildedObj
{
    public int itemNum;
    public Vector3 position;
    public Quaternion rotation;
}

public class CraftManual : MonoBehaviour
{
    private static CraftManual _instance = null;

    public static CraftManual Instance
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

    private bool isActivated = false;  // CraftManual UI 활성 상태
    private bool isPreviewActivated = false; // 미리 보기 활성화 상태

    [SerializeField]
    private GameObject go_BaseUI; // 기본 베이스 UI

    [SerializeField]
    private ItemAsset craft_fire;  // 불 탭에 있는 슬롯들.

    //[SerializeField]
    private Tilemap tilemap; // 타일맵   

    private int selNum;
    private List<BuildedObj> buildedList = new List<BuildedObj>();
    private Quaternion buildRot = Quaternion.identity;



    public Build[] build_myroom;
    private GameObject go_Preview; // 미리 보기 프리팹을 담을 변수
    private GameObject go_Prefab; // 실제 생성될 프리팹을 담을 변수 

    [SerializeField]
    private Transform tf_Player;  // 플레이어 위치
    [SerializeField]
    private GameObject TestObject;  // 테스트

    private RaycastHit2D hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    private Vector3 prepos;

    public void SlotClick(int slotNumber)
    {
        craft_fire = InventoryManager.Instance.GetSelectedItem(false);
        go_Preview = Instantiate(craft_fire.go_PreviewPrefab);
        go_Prefab = craft_fire.go_prefab;

        //go_Preview = Instantiate(build_myroom[slotNumber].go_PreviewPrefab);
        //go_Prefab = build_myroom[slotNumber].go_Prefab;
        selNum = build_myroom[slotNumber].buildItemNum;


        go_Preview.SetActive(false);
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
    }
    void Start()
    {
        // GameObject gridObj = GameObject.Find("Map_02");
        Grid grid = FindObjectOfType<Grid>();
        if (grid != null)
        {
            tilemap = grid.GetComponentInChildren<Tilemap>();
        }
        Debug.Log($"CraftManual.cs - [tilemap] - {tilemap}");

    }

    void Update()
    {
        //Debug.Log($"")
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            //craft_fire = InventoryManager.Instance.GetSelectedItem(false);
            //if(craft_fire != null)
            Window();
        }

        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
            if (Input.GetButtonDown("Fire1"))
                Build();
        }

        if (go_Preview != null)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3Int cellPos = tilemap.WorldToCell(worldPos);

            Vector3 cellCenterPos = tilemap.GetCellCenterWorld(cellPos);
            //Debug.Log($"cellCenterPos- {cellCenterPos}");
            cellCenterPos.z = 0;
            go_Preview.transform.position = cellCenterPos;
            go_Preview.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    private void PreviewPositionUpdate()
    {
        // 마우스 위치로부터 레이를 쏘아 충돌 검사
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D ray2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, layerMask);
        //RaycastHit2D hit2d = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);
        //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))            
        if (ray2D)
        {
            //Debug.Log("Raycast");
            // 미리보기 오브젝트 위치 이동
            //go_Preview.transform.position = hit.point;
            go_Preview.transform.position = ray2D.point;
            prepos = go_Preview.transform.position;
            prepos.z = 0;
            go_Prefab.transform.position = prepos;
        }
    }

    private void Build()
    {
        if (go_Preview != null)
        {
            //go_Preview.GetComponent<PreviewObject>() != null && go_Preview.GetComponent<PreviewObject>().isBuildable()
            if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().isBuildable())
            {
                foreach (GameObject pos in TurnManager.instance.builded)
                {
                    if (pos.transform.position == go_Preview.transform.position)
                    {
                        Debug.Log("이미 있음");
                        return;
                    }
                }
                // 저장된 위치 정보를 사용하여 프리팹 생성
                InventoryManager.Instance.GetSelectedItem(true);
                //TurnManager.instance.builded.Add(go_Preview.transform.position);
                GameObject obj = Instantiate(go_Prefab, go_Preview.transform.position, Quaternion.identity);
                TurnManager.instance.builded.Add(obj);
                //Debug.Log($"go_Preview - {go_Preview.transform.position}");
                Destroy(go_Preview);

                // 리스트배열에 담기
                BuildedObj buildedObj;
                buildedObj.rotation = buildRot;
                //buildedObj.postion = hitInfo.point;
                buildedObj.position = obj.transform.position;
                buildedObj.itemNum = selNum;
                buildedList.Add(buildedObj);

                isActivated = false;
                isPreviewActivated = false;
                go_Preview = null;
                go_Prefab = null;

            }
        }
    }

    public void SaveButtonClick()
    {
        string blist = JsonConvert.SerializeObject(buildedList);
        //string blist = JsonUtility.ToJson(buildedList);
        Debug.Log($"blist - {blist}");
        //패킷 플토콜
        C_MyroomSave cmyroomsave = new C_MyroomSave();
        {
            cmyroomsave.Buildlist = blist;
        }
        // 리스트배열 푸시
        // 패킷 보내기
        Managers.Network.Send(cmyroomsave);
    }

    public void LoadObjects(string data)
    {
        buildedList = JsonConvert.DeserializeObject<List<BuildedObj>>(data);

        if (buildedList == null)
        {
            Debug.Log($"buildedList Null - {buildedList}");
            return;
        }

        for (int i = 0; i < buildedList.Count; i++)
        {
            Debug.Log($"i buildedList[i].itemNum- {i},{buildedList[i].itemNum}");
            Instantiate(build_myroom[buildedList[i].itemNum].go_Prefab, buildedList[i].position, buildedList[i].rotation);
        }
    }


    private void Window()
    {
        if (!isActivated)
            OpenWindow();
        else
            CloseWindow();
    }

    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }

    private void Cancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;

        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false);
    }
}