using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public class SpawnManager : MonoBehaviour
{
    private HeatGauge hg;
    private float hotSpotChance;
    private float raidChance = 50;
    [SerializeField] Vector2 spawnBounds = new Vector2(100, 100);

    [SerializeField] private GameObject hotSpotPrefab;

    [Header("Delays")]
    private float minDelay = 8;
    private float maxDelay = 15;

    [SerializeField] private Transform[] spawnLocs;
    [SerializeField] private GameObject[] FFTypes;
    // Start is called before the first frame update
    void Start()
    {
        hg = HeatGauge.instance;

        StartCoroutine("LoopSpawns");
        StartCoroutine("LoopEnemySpawns"); 

    }

    // Update is called once per frame
    public void ChanceThresholds()
    {
        if(HeatGauge.heat > 50)
        {
            raidChance = .5f - HeatGauge.currMargin;
        } else if (HeatGauge.heat < 50)
        {
            raidChance = .5f + HeatGauge.currMargin;
        }
    }
    private void SpawnHotspot()
    {
        NavMeshHit hit;
        if ( NavMesh.SamplePosition(
            new Vector3(
            Random.Range(-spawnBounds.x, spawnBounds.x), 
            1.5f, 
            Random.Range(-spawnBounds.y, spawnBounds.y)),
            out hit,
            1.0f, NavMesh.AllAreas))
        {
            Instantiate(hotSpotPrefab, hit.position, Quaternion.identity);
            hg.HotspotSpawn();
            UIManager.instance.PopUp("HotspotAlert");
        } else
        {
            SpawnHotspot();
        }
    }
    IEnumerator LoopSpawns()
    {
        float tempTime =  Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(tempTime);
        ChanceThresholds();
        float tempRand = Random.value;
        if (tempRand >= raidChance)
        {
            SpawnHotspot();
        }
        else
        {
            SpawnRaid();
        }
        StartCoroutine("LoopSpawns");
    }

    IEnumerator LoopEnemySpawns()
    {
        SpawnEnemy(2);
        float tempRand = Random.Range(2, 6);
        yield return new WaitForSeconds(tempRand);
        StartCoroutine("LoopEnemySpawns");
    }

    private void SpawnRaid()
    {
        int amount = (int) Random.Range(3, 8);
        SpawnEnemy(amount);
        UIManager.instance.PopUp("RaidAlert");
        StartCoroutine("LoopEnemySpawns");
    }

    private void SpawnEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int prefabType;
            int spawnloc;
            prefabType = Random.Range(0, FFTypes.Length);
            spawnloc = Random.Range(0, spawnLocs.Length);
            Instantiate(FFTypes[prefabType], spawnLocs[spawnloc].position, Quaternion.identity);
        }
    }
}
