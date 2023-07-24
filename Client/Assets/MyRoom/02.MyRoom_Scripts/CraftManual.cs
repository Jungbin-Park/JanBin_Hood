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

    private bool isActivated = false;  // CraftManual UI Ȱ�� ����
    private bool isPreviewActivated = false; // �̸� ���� Ȱ��ȭ ����

    [SerializeField]
    private GameObject go_BaseUI; // �⺻ ���̽� UI

    [SerializeField]
    private ItemAsset craft_fire;  // �� �ǿ� �ִ� ���Ե�.

    //[SerializeField]
    private Tilemap tilemap; // Ÿ�ϸ�   

    private int selNum;
    private List<BuildedObj> buildedList = new List<BuildedObj>();
    private Quaternion buildRot = Quaternion.identity;



    public Build[] build_myroom;
    private GameObject go_Preview; // �̸� ���� �������� ���� ����
    private GameObject go_Prefab; // ���� ������ �������� ���� ���� 

    [SerializeField]
    private Transform tf_Player;  // �÷��̾� ��ġ
    [SerializeField]
    private GameObject TestObject;  // �׽�Ʈ

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
        // ���콺 ��ġ�κ��� ���̸� ��� �浹 �˻�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D ray2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, layerMask);
        //RaycastHit2D hit2d = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);
        //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))            
        if (ray2D)
        {
            //Debug.Log("Raycast");
            // �̸����� ������Ʈ ��ġ �̵�
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
                        Debug.Log("�̹� ����");
                        return;
                    }
                }
                // ����� ��ġ ������ ����Ͽ� ������ ����
                InventoryManager.Instance.GetSelectedItem(true);
                //TurnManager.instance.builded.Add(go_Preview.transform.position);
                GameObject obj = Instantiate(go_Prefab, go_Preview.transform.position, Quaternion.identity);
                TurnManager.instance.builded.Add(obj);
                //Debug.Log($"go_Preview - {go_Preview.transform.position}");
                Destroy(go_Preview);

                // ����Ʈ�迭�� ���
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
        //��Ŷ ������
        C_MyroomSave cmyroomsave = new C_MyroomSave();
        {
            cmyroomsave.Buildlist = blist;
        }
        // ����Ʈ�迭 Ǫ��
        // ��Ŷ ������
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