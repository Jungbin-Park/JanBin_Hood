using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPlant2D : MonoBehaviour
{
    public float growTime = 10f;
    public float maxGrowTime = 0f;
    public bool reharvestAble = true;
    public GameObject FullGrowth;
    private float ptime = 0f;
    private bool harvestAble = false;

    void Start()
    {
        if (maxGrowTime != growTime && maxGrowTime < growTime)
            maxGrowTime = growTime;
    }
    private void Awake()
    {
        ptime = 0f;
    }
    void Update()
    {
        if (!harvestAble)
        {
            if (growTime <= ptime)
            {
                harvestAble = true;
            }
            ptime += Time.deltaTime;
            float sc = (ptime / maxGrowTime) / 2f + 0.5f;
            transform.localScale = Vector3.one * sc;
        }

        if (harvestAble)
        {
            Instantiate(FullGrowth, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
