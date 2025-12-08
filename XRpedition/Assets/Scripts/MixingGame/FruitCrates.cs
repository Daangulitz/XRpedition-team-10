using Oculus.Interaction;
using UnityEngine;
using Oculus.Interaction.HandGrab;

public class ChestFruitSpawner : MonoBehaviour
{
    public GameObject[] fruitPrefab;

    [Header("Spawn Settings")]
    public Transform spawnPoint;       // Empty GameObject where fruits appear
    public float spawnCooldown = 0.5f; // Prevents multiple spawns too quickly

    private float nextSpawnTime = 0f;

    private void OnTriggerExit(Collider other)
    {
        var hand = other.GetComponentInParent<HandGrabInteractor>();
        if (hand != null && Time.time >= nextSpawnTime)
        {
            SpawnFruit();
            nextSpawnTime = Time.time + spawnCooldown;
        }
    }

    private void SpawnFruit()
    {
        if (fruitPrefab == null || spawnPoint == null)
            return;

        GameObject fruit = Instantiate(fruitPrefab[Random.Range(0, fruitPrefab.Length)], spawnPoint.position, spawnPoint.rotation);

        // Ensure it has Rigidbody & Grabbable enabled
        Rigidbody rb = fruit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        var grabbable = fruit.GetComponent<Grabbable>();
        if (grabbable != null)
            grabbable.enabled = true;
    }
}