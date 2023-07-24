using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    public ItemAsset item;
    public float maxGrowTime = 0f;

    public GameObject FullGrowth;
    public int plantTurn = 0;
    private bool harvestAble = false;
    public bool reharvested = false;

    private void Awake()
    {
        plantTurn = TurnManager.instance.turn;

        if (reharvested)
            maxGrowTime = item.reharvestTime;
        else
            maxGrowTime = item.growTime;

        TurnManager.instance.harvDele += CalculateHarvestTurn;
    }

    void Update()
    {
        if (harvestAble)
        {
            List<GameObject> builds = TurnManager.instance.builded;
            builds.Add(Instantiate(FullGrowth, transform.position, transform.rotation));
            builds.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }
    void CalculateHarvestTurn()
    {
        int harvestTurn = plantTurn + (int)maxGrowTime;
        if (harvestTurn > TurnManager.maxturn)
        {
            harvestTurn -= TurnManager.maxturn;
        }
        if (harvestTurn <= TurnManager.instance.turn)
        {
            harvestAble = true;
            TurnManager.instance.harvDele -= CalculateHarvestTurn;
        }
    }
}
