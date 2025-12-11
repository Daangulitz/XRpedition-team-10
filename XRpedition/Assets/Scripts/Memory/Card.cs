using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Card : MonoBehaviour
{
    public float ID;
    [HideInInspector] public MemoryGameLoop gameLoop;
    [HideInInspector] public bool IsAddedToManager = false;
    public bool IsFlippedUp;
    
    private BoxCollider boxCollider;
    private AudioSource audioSource;
    //[SerializeField] private AudioClip Flip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
    }
    
    private void Update()
    {
        //Adds ID to manager
        if (!IsAddedToManager && IsFlippedUp)
        {
            AddIDToManager();
        }
        
        //Deletes ID from manager
        if (IsAddedToManager && !IsFlippedUp)
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
        
        // //als ie wel is omgedraaid, maar niet aan CardIDs [0] of [1] is, draait ie terug
        // if (gameLoop.CardIDs[0] != ID && gameLoop.CardIDs[0] != null && gameLoop.CardIDs[1] != ID && IsFlippedUp)
        // {
        //     FlipBack();
        // }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PinchArea") {
            if (!IsFlippedUp && gameLoop.CanFlip) 
            {
                FlipCardAround(); 
            }
        }
    }

    private void FlipCardAround()
    {
        if (!IsFlippedUp)
        {
            StartCoroutine(RotateCard(1));
            IsFlippedUp = true;
        }
    }
    
    private void AddIDToManager()
    {
        gameLoop.CardIDs.Add(ID);
        IsAddedToManager = true;
    }

    public void FlipBack()
    {
        if (IsFlippedUp)
        {
            StartCoroutine(RotateCard(0));
        }
    }
    
    private IEnumerator RotateCard(int x)
    {
        boxCollider.enabled = false;
        float duration = 0.5f;
        //audioSource.PlayOneShot(Flip);
        
        if (x == 0)
        {
            yield return StartCoroutine(RotateTo(Quaternion.Euler(0, 0, 0), duration));
            IsFlippedUp = false;
        } 
        else if (x == 1)
        {
            yield return StartCoroutine(RotateTo(Quaternion.Euler(0, 0, 180), duration));
            IsFlippedUp = true;
        }
        boxCollider.enabled = true;
        
        if (gameLoop.CardIDs.Count >= 2)
        {
            gameLoop.CheckMatch();
        }
    }
    
    private IEnumerator RotateTo(Quaternion targetRot, float duration)
    {
        float elapsed = 0;
        Quaternion startRot = transform.rotation;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot;
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
