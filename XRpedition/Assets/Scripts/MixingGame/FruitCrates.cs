using UnityEngine;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Input;

public class ChestFruitSpawner : MonoBehaviour
{
    public GameObject fruitPrefab;

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

        // Detect pinch
        bool isPinching = currentHand.Hand.GetFingerPinchStrength(HandFinger.Index) > 0.7f;

        // Only spawn if pinching but nothing is grabbed
        if (isPinching && !currentHand.HasSelectedInteractable)
        {
            SpawnFruitInHand(currentHand);
        }
    }

    private void SpawnFruitInHand(HandGrabInteractor hand)
    {
        // Instantiate fruit
        GameObject fruit = Instantiate(fruitPrefab);

        // Place at pinch point
        Transform pinch = hand.PinchPoint;
        fruit.transform.position = pinch.position;
        fruit.transform.rotation = pinch.rotation;
    }
}