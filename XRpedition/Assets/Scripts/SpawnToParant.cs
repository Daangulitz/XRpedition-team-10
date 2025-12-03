using UnityEngine;

public class SpawnToParant : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    
    private void Start()
    {
        Transform parent = GameObject.FindGameObjectWithTag("MainCamera").transform;

        Instantiate(Canvas, parent);
    }
}
