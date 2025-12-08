using UnityEngine;

public class Card : MonoBehaviour
{
    public bool isTurnedAround = false;
    [SerializeField] private int rangeTurnedAround = 170;
    
    private MemoryGameLoop gameLoop;
    void Start()
    {
        gameLoop = GameObject.FindWithTag("GameLoop").GetComponent<MemoryGameLoop>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfTurnedAround();
    }

    void CheckIfTurnedAround()
    {
        if (transform.eulerAngles.x > rangeTurnedAround || transform.eulerAngles.x < -rangeTurnedAround ||
            transform.eulerAngles.z > rangeTurnedAround || transform.eulerAngles.z < -rangeTurnedAround)
        {
            isTurnedAround = true;
            if (gameLoop.Card1Prefab == null)
            {
                gameLoop.Card1Prefab = gameObject;
            } 
            else if (gameLoop.Card1Prefab != null && gameLoop.Card2Prefab == null)
            {
                gameLoop.Card2Prefab = gameObject;
            }
        }
        else
        {
            isTurnedAround = false;
            if (gameLoop.Card1Prefab == gameObject)
            {
                gameLoop.Card1Prefab = null;
            } 
            else if (gameLoop.Card2Prefab == gameObject)
            {
                gameLoop.Card2Prefab = null;
            }
        }
    }

    public void destroyCard()
    {
        Destroy(gameObject);
    }
}
