using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollecter : MonoBehaviour
{
    static public ItemCollecter instance;
    public List<ItemAsset> items;
    public int selected = -1;

    public void Awake()
    {
        if(instance == null )
            instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    int comparer(ItemAsset arg, ItemAsset arg2)
    {
        return arg.id < arg2.id ? -1:1;
    }
    public void Start()
    {
        items.Sort(comparer);
    }
}
