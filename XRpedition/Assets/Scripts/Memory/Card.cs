using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    public float ID;

    [HideInInspector] public MemoryGameLoop gameLoop;
    
    [HideInInspector] public bool IsAddedToManager = false;

    private void Update()
    {

        if (!IsAddedToManager && IsFlippedUp())
        {
            AddIDToManager();
            IsAddedToManager = true;
        }
        else if (IsAddedToManager && !IsFlippedUp())
        { 
            if (gameLoop.CardIDs[0] == ID)
            {
                gameLoop.CardIDs.RemoveAt(0);
            } 
            else if (gameLoop.CardIDs[1] == ID)
            {
                gameLoop.CardIDs.RemoveAt(1);
            }
            IsAddedToManager = false;
        }
    }

    public bool IsFlippedUp()
    {
        float zDelta = Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.z, 0));
        float xDelta = Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.x, 0));
        
        return zDelta > gameLoop.flipRange || xDelta > gameLoop.flipRange;
    }

    private void AddIDToManager()
    {
        gameLoop.CardIDs.Add(ID);
    }

    public void FlipBack()
    {
        StartCoroutine(RotateCard());
    }

    private IEnumerator RotateCard()
    {
        float duration = 0.5f;
        float elapsed = 0;
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.Euler(0, 0, 0); // Ga terug naar plat

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = end;
    }

    public void MatchFound()
    {
        DestroyCard();
    }

    private void DestroyCard()
    {
        Destroy(gameObject);
    }
}
