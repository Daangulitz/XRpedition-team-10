using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class ChestFruitSpawner : MonoBehaviour
{
    public GameObject fruitPrefab;

    private HandGrabInteractor currentHand;

    private void OnTriggerEnter(Collider other)
    {
        currentHand = other.GetComponentInParent<HandGrabInteractor>();
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

        // A grab gesture has begun but nothing is grabbed yet
        if (currentHand.State == InteractorState.Select &&
            !currentHand.HasSelectedInteractable)
        {
            SpawnFruitInHand(currentHand);
        }
    }

    private void SpawnFruitInHand(HandGrabInteractor hand)
    {
        // Instantiate fruit
        GameObject fruit = Instantiate(fruitPrefab);

        // Place fruit at the pinch point
        Transform pinch = hand.PinchPoint;

        fruit.transform.position = pinch.position;
        fruit.transform.rotation = pinch.rotation;

        // The HandGrabInteractor will automatically grab it
        // because the fruit has a HandGrabInteractable.
    }
}