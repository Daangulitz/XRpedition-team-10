using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MemoryGameLoop : MonoBehaviour
{
    [Header("Card Prefabs (unique)")]
    [SerializeField] private GameObject[] cardPrefabs;
    public List<Card> spawnedCards = new List<Card>();


    [Header("Grid Settings")]
    [SerializeField] private int columns = 4;
    [SerializeField] private int rows = 4;
    [SerializeField] private float spacing = 0.3f;
    private int[] gridValues;

    [Header("Hierarchy")]
    [SerializeField] private Transform cardsAnchor;

    [Header("UI")]
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private float TimeActive = 5f;
    private GameObject WrongUI;
    private GameObject RightUI;
    
    [Header("Sound")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip Right;
    [SerializeField] private AudioClip Wrong;
    [SerializeField] private AudioClip BGMusic;
    
    private Transform playerHead;
    
    public bool CanFlip = true;

    [SerializeField] private GameObject cake;
    private bool cakeInstatiated = false;


    public float flipRange;
    
    //private bool checkingMatch = false;

    public List<float> CardIDs = new List<float>();
    
    void Start()
    {
        GenerateValues();
        Shuffle(gridValues);
        SpawnCards();

        playerHead = GameObject.FindWithTag("MainCamera").transform;
        
        GameObject uiInstance = Instantiate(
            UICanvas,
            playerHead.position,
            playerHead.rotation,
            playerHead
        );
        
        Transform uiRoot = uiInstance.transform;

        RightUI = uiRoot.Find("Right")?.gameObject;
        WrongUI = uiRoot.Find("Wrong")?.gameObject;

        if (RightUI == null)
            Debug.LogError("UI error: 'Right' kon niet gevonden worden in de UI prefab.");
        if (WrongUI == null)
            Debug.LogError("UI error: 'Wrong' kon niet gevonden worden in de UI prefab.");

        // Zorg dat ze uit staan
        if (RightUI != null) RightUI.SetActive(false);
        if (WrongUI != null) WrongUI.SetActive(false);

        // Reset card lijst
        CardIDs.Clear();
        
        audioSource = GetComponent<AudioSource>();

    }


    void Update()
    {
        if (CardIDs.Count >= 2)
        {
            CheckMatch();
        }

        GameObject CardsLeft = GameObject.FindWithTag("Card");
        if (!cakeInstatiated && CardsLeft == null)
        {
            Instantiate(cake, cardsAnchor);
            cakeInstatiated = true;
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

    public void CheckMatch()
    {
        CanFlip = false;   // ❗ NIEMAND mag flippen tijdens check

        StartCoroutine(DoCheck());
    }

    private IEnumerator DoCheck()
    {
        yield return new WaitForSeconds(0.6f); // wacht tot animatie van beide klaar is

        if (CardIDs[0] == CardIDs[1])
        {
            float ID = CardIDs[0];

            foreach (Card card in spawnedCards)
            {
                if (card != null)
                {
                    if (card.ID == ID)
                    {
                        audioSource.PlayOneShot(Right);
                        card.MatchFound();
                        
                    }
                }
            }

            UpdateUI("right");
        }
        else
        {
            audioSource.PlayOneShot(Wrong);
            foreach (Card card in spawnedCards)
            {
                if (card != null)
                {
                    if (card.IsFlippedUp)
                    {
                        card.FlipBack();
                    }
                    card.IsAddedToManager = false;
                }

            }

            UpdateUI("wrong");
        }

        CardIDs.Clear();

        yield return new WaitForSeconds(0.6f); // wacht tot flipback klaar

        CanFlip = true;  //  FLIPPEN MAG WEER
    }



    private void UpdateUI(string rightOrwrong)
    {
        StartCoroutine(EnableUISequence(rightOrwrong));

    }
    
    private IEnumerator EnableUISequence(string type)
    {
        if (type == "right" || type == "Right")
        {
            RightUI.SetActive(true);

            yield return new WaitForSeconds(TimeActive);
            RightUI.SetActive(false);
            yield break;
        }
        else if (type == "wrong" || type == "Wrong")
        {
            WrongUI.SetActive(true);

            yield return new WaitForSeconds(TimeActive);
            WrongUI.SetActive(false);
            yield break;
        }
    }
}
