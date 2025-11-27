using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] FishPrefabs;
    [SerializeField] private float AmountFish;

    private Vector3 spawnPosition;

    
    void Start()
    {
        SpawnFish();
    }

    void SpawnFish()
    {
        for (int i = 0; i < AmountFish; i++)
        {
            Instantiate(FishPrefabs[Random.Range(0, FishPrefabs.Length)], transform.position, Quaternion.identity);
        }
    }

    public void SpawnOneRandomFish()
    {
        Instantiate(FishPrefabs[Random.Range(0, FishPrefabs.Length)], transform.position, Quaternion.identity);
    }

    public void SpawnSpecificFish(int x)
    {
        Instantiate(FishPrefabs[x], transform.position, Quaternion.identity);
    }
    
    
}