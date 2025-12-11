using UnityEngine;

public class SmoothieSpawner : MonoBehaviour
{
    public GameObject SmoothieGlasPrefab;
    public Transform TargetPosition;
    public float spawnCooldown = 0.5f; 

    private bool canSpawn = true;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SmoothieGlas"))
        {
            Instantiate(SmoothieGlasPrefab, TargetPosition.position, TargetPosition.rotation);

        }
    }
}