using System;
using UnityEngine;

public class SmoothieGlasSpawner : MonoBehaviour
{
    [SerializeField] private GameObject SmoothieGlasPrefab;
    [SerializeField] private Transform TargetPosition;


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SmoothieGlas"))
        {
            Instantiate(SmoothieGlasPrefab, TargetPosition.position, TargetPosition.rotation);
        }
    }
}
