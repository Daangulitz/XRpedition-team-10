using UnityEngine;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Input;

public class ChestFruitSpawner : MonoBehaviour
{
    public GameObject fruitPrefab;

    [Header("Spawn Settings")]
    public float spawnCooldown = 0.5f; // Time between spawns

    private float nextSpawnTime = 0f;
    private HandGrabInteractor currentHand;

    private void OnTriggerEnter(Collider other)
    {
        var hand = other.GetComponentInParent<HandGrabInteractor>();
        if (hand != null)
            currentHand = hand;
    }

    private void OnTriggerExit(Collider other)
    {
        var hand = other.GetComponentInParent<HandGrabInteractor>();
        if (hand == currentHand)
            currentHand = null;
    }

    private void Update()
    {
        if (currentHand == null)
            return;

        bool isPinching = currentHand.Hand.GetFingerPinchStrength(HandFinger.Index) > 0.7f;

        // Only spawn if pinching, hand is empty, AND cooldown expired
        if (isPinching &&
            !currentHand.HasSelectedInteractable &&
            Time.time >= nextSpawnTime)
        {
            SpawnFruitInHand(currentHand);
            nextSpawnTime = Time.time + spawnCooldown; // Reset cooldown
        }
    }

    private void SpawnFruitInHand(HandGrabInteractor hand)
    {
        GameObject fruit = Instantiate(fruitPrefab);

        Transform pinch = hand.PinchPoint;
        fruit.transform.position = pinch.position;
        fruit.transform.rotation = pinch.rotation;
    }
}