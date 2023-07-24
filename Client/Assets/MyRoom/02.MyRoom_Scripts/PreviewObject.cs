using IsoTools.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PreviewObject : MonoBehaviour
{
    private List<Collider> colliderList = new List<Collider>(); // 충돌한 오브젝트들 저장할 리스트

    [SerializeField]
    private int layerGround; // 지형 레이어 (무시하게 할 것)
    private const int IGNORE_RAYCAST_LAYER = 2;  // ignore_raycast (무시하게 할 것)

    private Tilemap tilemap; // 타일맵   
    private Grid _grid;
    private TilemapCollider2D tileCollider;
    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;

    bool canbuild = true;
    void Start()
    {
        Grid grid = FindObjectOfType<Grid>();
        _grid = FindObjectOfType<Grid>();
        Debug.Log($"gridPreview- {grid}");
        if (grid != null)
        {
            tilemap = grid.GetComponentInChildren<Tilemap>();
            tileCollider = grid.GetComponentInChildren<TilemapCollider2D>();
        }
        Debug.Log($"Start -PreviewObject-tilemap {tilemap}");
    }

    void Update()
    {
        do_check();
        ChangeColor();
    }
    private void do_check()
    {
        // 마우스 위치를 2D 월드 좌표로 변환
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 충돌 검사를 수행할 레이어 마스크
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("PreView"));

        // 2D 충돌 검사 수행
        RaycastHit2D hit2d = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = tilemap.WorldToCell(mousePos);
        TileBase tile = tilemap.GetTile(cellPos);
        Debug.Log($"tile -PreivewObject- {tile}");

        // 충돌한 오브젝트가 있다면        
        if (tile != null)
        {
            Debug.Log($"Hit object: {tile}");

            // 충돌한 오브젝트
            if (hit2d.transform.gameObject == gameObject)
            {
                canbuild = false;
                Debug.Log($"PreviewObject - Can build: {canbuild}");

            }
            else
            {
                canbuild = true;
                Debug.Log($"PreviewObject - Can build: {canbuild}");

            }
        }
        if (tile == null)
        {
            canbuild = false;
            Debug.Log($"tile == null - CanNot build: {canbuild}");
        }
    }
    private void ChangeColor()
    {
        if (!canbuild)
        {
            SetColor(red);
        }
        else
        {
            canbuild = true;
            SetColor(green);
        }

    }
    private void SetColor(Material mat)
    {
        foreach (Transform tf_Child in this.transform)
        {
            Material[] newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }
    public bool isBuildable()
    {
        if (canbuild)
            return true;
        else
            return false;
    }
}