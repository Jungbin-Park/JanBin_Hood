using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu]
public class ItemAsset : ScriptableObject
{
    public enum ItemType
    {
        Seed,
        Furniture
    }

    public int id;
    public string itemName;
    public bool stackable = true;
    public string description;
    public Sprite itemImage;
    public ItemType itemType;
    public GameObject go_prefab;
    public GameObject go_PreviewPrefab;

    public float growTime = 10f;
    public bool reharvestAble = true;
    public float reharvestTime = 0f;
    public int marketPrice = 0;
}
