using System.Collections;
using UnityEngine;

public class SceneMeshSpawner : MonoBehaviour
{
    [SerializeField] GameObject SceneMeshPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnSceneMesh());
    }

    private IEnumerator SpawnSceneMesh()
    {
        yield return new WaitForSeconds(1f);
        
        Instantiate(SceneMeshPrefab);
    }
}
