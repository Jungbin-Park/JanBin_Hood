using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmSlot : MonoBehaviour
{
    public Image img;
    public TMP_Text tex;
    void Update()
    {
        ItemAsset item = InventoryManager.Instance.GetSelectedItem(false);
        if (item != null)
        {
            img.sprite = item.itemImage;
            tex.text = item.description;
        }
    }
}
