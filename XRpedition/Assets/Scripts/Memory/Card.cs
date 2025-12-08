using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{
    public int cardID;
    public MemoryGameLoop gameLoop;

    private bool isFlipped = false;
    private bool isAnimating = false;

    [Header("Flip Settings")]
    [SerializeField] private float flipDuration = 0.3f;

    /// <summary>
    /// Wordt aangeroepen wanneer de speler de kaart selecteert in VR
    /// </summary>
    public void Flip()
    {
        if (isFlipped || isAnimating) return;

        isFlipped = true;
        gameLoop.Card1 ??= this;
        if (gameLoop.Card1 != this)
            gameLoop.Card2 ??= this;

        // Start smooth flip animatie
        StartCoroutine(FlipAnimation(180f));

        // Check op match in de game loop
        gameLoop.TryCheckCards();
    }

    public void ResetCard()
    {
        if (!isFlipped || isAnimating) return;

        isFlipped = false;
        StartCoroutine(FlipAnimation(-180f));
    }

    public void DestroyCard()
    {
        Destroy(gameObject);
    }

    private IEnumerator FlipAnimation(float angle)
    {
        isAnimating = true;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(angle, 0f, 0f);

        float elapsed = 0f;
        while (elapsed < flipDuration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / flipDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
        isAnimating = false;
    }
}