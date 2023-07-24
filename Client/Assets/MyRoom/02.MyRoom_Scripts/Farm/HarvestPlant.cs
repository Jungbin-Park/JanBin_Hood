 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestPlant : MonoBehaviour
{
    public GameObject yetGrown;
    private void Start()
    {
        if (yetGrown != null)
        {
            GrowPlant gp = yetGrown.GetComponent<GrowPlant>();
            if (gp.item.reharvestAble)
            {
                gp.maxGrowTime = gp.item.reharvestTime;

                gp.reharvested = true;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(pos, Vector2.zero);
            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);
            Debug.Log(hit2d.collider.name);
            if (hit2d.collider != null && hit2d.transform.gameObject == this.gameObject)
            {
                Harvest();
            }
        }
    }
    void Harvest()
    {
        List<GameObject> builds = TurnManager.instance.builded;
        GrowPlant gp = yetGrown.GetComponent<GrowPlant>();
        builds.Remove(this.gameObject);
        if (gp.item.reharvestAble)
        {
            GameObject obj = Instantiate(yetGrown, transform.position, transform.rotation);
            obj.GetComponent<GrowPlant>().plantTurn = TurnManager.instance.turn;
            builds.Add(obj);
        }
        builds.Remove(this.gameObject);
        if(gp.item.itemType == ItemAsset.ItemType.Seed)
        {
            InventoryManager.Instance.AddItem(gp.item);
            InventoryManager.Instance.AddItem(gp.item);
        }
        else if(gp.item.itemType == ItemAsset.ItemType.Furniture)
        {
            int harvPrice = (int)(gp.item.marketPrice / 20f);
            //string upd = $"UPDATE Account SET umoney = umoney + {harvPrice} WHERE usid = '{MarketClient.instance.id}'";
            //MarketClient.SendCMD(upd, 'I');
            //MarketClient.instance.SetMoney();
        }

        Destroy(this.gameObject);
    }
}
