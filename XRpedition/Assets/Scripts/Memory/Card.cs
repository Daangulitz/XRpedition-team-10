using UnityEngine;

public class Card : MonoBehaviour
{
    public bool isTurnedAround = false;
    [SerializeField] private int rangeTurnedAround = 170;
    void Start()
    {
        
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
        }
        else
        {
            isTurnedAround = false;
        }
    }
}
