using UnityEngine;

public class ResetGameButtons : MonoBehaviour
{
    [SerializeField] private GameObject ResetGame;

    public void Resets()
    {
        Instantiate(ResetGame);
    }
}
