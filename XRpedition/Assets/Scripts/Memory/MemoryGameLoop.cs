using System.Collections;
using UnityEngine;

public class MemoryGameLoop : MonoBehaviour
{
    [Header("Card Prefabs (unique)")]
    [SerializeField] private GameObject[] cardPrefabs;

    [Header("Grid Settings")]
    [SerializeField] private int columns = 4;
    [SerializeField] private int rows = 4;
    [SerializeField] private float spacing = 2f;

    [Header("UI")]
    [SerializeField] private GameObject UICanvas;

    [Header("VR")]
    [SerializeField] private Transform playerHead;

    public Card Card1;
    public Card Card2;

    private int[] gridValues;
    private bool checkingMatch = false;

    void Start()
    {
        GenerateValues();
        Shuffle(gridValues);
        SpawnCards();

        playerHead = GameObject.FindWithTag("MainCamera").transform;
        Instantiate(UICanvas, playerHead);
    }

    private void GenerateValues()
    {
        int totalCards = cardPrefabs.Length * 2;
        gridValues = new int[totalCards];

        int index = 0;
        for (int i = 0; i < cardPrefabs.Length; i++)
        {
            gridValues[index] = i;
            gridValues[index + 1] = i;
            index += 2;
        }
    }

    private void Shuffle(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int rand = Random.Range(i, array.Length);
            int temp = array[rand];
            array[rand] = array[i];
            array[i] = temp;
        }
    }

    private void SpawnCards()
    {
        for (int i = 0; i < gridValues.Length; i++)
        {
            int id = gridValues[i];
            GameObject prefab = cardPrefabs[id];

            int row = i / columns;
            int col = i % columns;

            Vector3 position = new Vector3(
                col * spacing, 2.1f, row * spacing
            );

            GameObject cardObj = Instantiate(prefab, position, Quaternion.identity);
            cardObj.transform.SetParent(transform, false);

            Card card = cardObj.AddComponent<Card>();
            card.gameLoop = this;
            card.cardID = id;
        }
    }

    public void TryCheckCards()
    {
        if (Card1 != null && Card2 != null && !checkingMatch)
        {
            StartCoroutine(CheckCards());
        }
    }

    private IEnumerator CheckCards()
    {
        checkingMatch = true;

        if (Card1.cardID == Card2.cardID)
        {
            // Match gevonden
            yield return new WaitForSeconds(0.5f); // korte pauze
            Card1.DestroyCard();
            Card2.DestroyCard();
        }
        else
        {
            // Geen match, reset kaarten
            yield return new WaitForSeconds(1f);
            Card1.ResetCard();
            Card2.ResetCard();
        }

        Card1 = null;
        Card2 = null;
        checkingMatch = false;
    }
}
