using System;
using System.Collections;
using UnityEngine;

using static Utils;

public class Sapling : MonoBehaviour
{
    public ESapling saplingType;
    public float timeOfFullGrowth;

    private MapManager mapManager;

    public Sapling(ESapling saplingType)
    {
        this.saplingType = saplingType;
    }

    public void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        timeOfFullGrowth = Time.time + UnityEngine.Random.Range(4, 10);
        StartCoroutine(CheckGrowthTime());
    }

    private IEnumerator CheckGrowthTime()
    {
        while(UnityEngine.Time.time < timeOfFullGrowth)
        {
            yield return null;
        }
        Debug.Log("Sapling grew");
        if(saplingType == ESapling.Tree) mapManager.PlaceTree(transform.position);
        else if(saplingType == ESapling.Cactus) mapManager.PlaceCactus(transform.position);
        Destroy(gameObject);
    }
}