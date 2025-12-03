using UnityEngine;

public class MemoryGameLoop : MonoBehaviour
{
    [Header("Card Prefabs (unique)")]
    [SerializeField] private GameObject[] cardPrefabs;

    [Header("Grid Settings")]
    [SerializeField] private int columns = 4; 
    [SerializeField] private int rows = 4;
    [SerializeField] private float spacing = 2; 
    
    public GameObject Card1Prefab;
    public GameObject Card2Prefab;

    private GameObject[] cards;
    
    private int[] gridValues; 


    void Start()
    {
        GenerateValues();
        Shuffle(gridValues);
        SpawnCards();
        
        cards = GameObject.FindGameObjectsWithTag("Card");
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
                col * spacing, 0, row * spacing
            );
        
            GameObject card = Instantiate(prefab, position, Quaternion.identity);
            card.transform.SetParent(transform, false);
        }
    }

    private void CheckIfCardsAreSame()
    {
        if (Card1Prefab == Card2Prefab)
        {
            RightMatch();
        }
    }

    private void RightMatch()
    {
        
    }

}
