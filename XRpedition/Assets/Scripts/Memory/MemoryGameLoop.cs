using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameLoop : MonoBehaviour
{
    [Header("Card Prefabs (unique)")]
    [SerializeField] private GameObject[] cardPrefabs;
    public List<Card> spawnedCards = new List<Card>();


    [Header("Grid Settings")]
    [SerializeField] private int columns = 4;
    [SerializeField] private int rows = 4;
    [SerializeField] private float spacing = 0.3f;

    [Header("Hierarchy")]
    [SerializeField] private Transform cardsAnchor;

    [Header("UI")]
    [SerializeField] private GameObject UICanvas;
    
    private Transform playerHead;

    public float flipRange;
    
    private int[] gridValues;
    //private bool checkingMatch = false;

    public List<float> CardIDs = new List<float>();
    
    void Start()
    {
        GenerateValues();
        Shuffle(gridValues);
        SpawnCards();

        playerHead = GameObject.FindWithTag("MainCamera")?.transform;
        Instantiate(UICanvas, playerHead.position, playerHead.rotation, playerHead);
        
        CardIDs = new List<float>();
    }

    void Update()
    {
        if (CardIDs.Count >= 2)
        {
            CheckMatch();
        }
    }

    private void GenerateValues()
    {
        int totalCards = cardPrefabs.Length * 2;
        gridValues = new int[totalCards];

        int index = 0;
        for (int i = 0; i < cardPrefabs.Length; i++)
        {
            if (index + 1 >= gridValues.Length)
            {
                Debug.LogError("GenerateValues index out of range — check cardPrefabs en grid size.");
                break;
            }
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
        if (gridValues == null || gridValues.Length == 0)
        {
            Debug.LogError("gridValues is leeg. GenerateValues faalde.");
            return;
        }
        
        float totalWidth = (columns - 1) * spacing;
        float totalDepth = (rows - 1) * spacing;
        Vector3 startLocal = new Vector3(-totalWidth / 2f, 0f, -totalDepth / 2f);

        for (int i = 0; i < gridValues.Length; i++)
        {
            int id = gridValues[i];
            if (id < 0 || id >= cardPrefabs.Length)
            {
                Debug.LogError($"gridValues bevat ongeldige id ({id}) op index {i}");
                continue;
            }

            GameObject prefab = cardPrefabs[id];

            int row = i / columns;
            int col = i % columns;

            Vector3 localPos = startLocal + new Vector3(col * spacing, 0f, row * spacing);
            
            GameObject cardObj = Instantiate(prefab, cardsAnchor);
            cardObj.transform.localPosition = localPos;
            cardObj.transform.localRotation = Quaternion.identity;
                
            Card card = cardObj.GetComponent<Card>();
            if (card == null)
            {
                card = cardObj.AddComponent<Card>();
            }

            card.gameLoop = this;
            card.ID = id;
            
            spawnedCards.Add(card);
            
            if (cardObj.GetComponent<Collider>() == null)
            {
                Debug.LogWarning($"Card prefab '{prefab.name}' heeft geen Collider — voeg een collider toe zodat interactie werkt.");
            }
        }
    }

    private void CheckMatch()
    {
        if (CardIDs[0] == CardIDs[1])
        {
            float ID = CardIDs[0];

            foreach (Card card in spawnedCards)
            {
                if (card.ID == ID)
                {
                    card.MatchFound();
                }
            }

            CardIDs.RemoveAt(0);
            CardIDs.RemoveAt(0);
            UpdateUI("right");
        }
        else
        {
            foreach (Card card in spawnedCards)
            {
                if (card.IsFlippedUp())
                {
                    card.FlipBack();
                }
            }

            CardIDs.RemoveAt(0);
            CardIDs.RemoveAt(0);
            UpdateUI("wrong");
        }
    }



    private void UpdateUI(string rightOrwrong)
    {
        if (rightOrwrong == "right" || rightOrwrong == "Right")
        {
            //UI right
        }
        else if (rightOrwrong == "wrong" || rightOrwrong == "Wrong")
        {
            //UI wrong
        }
        else
        {
            Debug.LogError($"UpdateUI parameter is spelled wrong: {rightOrwrong}");
        }
    }


}
