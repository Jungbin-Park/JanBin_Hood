using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public ItemAsset hItem;
    void Start()
    {
        if(hItem != null) {
            GetComponent<SpriteRenderer>().sprite = hItem.itemImage;
        }
    }
}
