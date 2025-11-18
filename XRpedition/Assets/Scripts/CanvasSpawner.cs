using UnityEngine;

public class SpawnMenuInFront : MonoBehaviour
{
    [SerializeField] private GameObject menuPrefab;
    [SerializeField] private Transform playerHead; // center eye anchor
    [SerializeField] private float distanceInFront = 1f;
    [SerializeField] private float heightOffset = 1.7f;

    private GameObject menuInstance;

    private void Start()
    {
        SpawnMenu();
    }
    
    public void SpawnMenu()
    {
        if (menuInstance != null) return;

        Debug.LogError("Spawning menu");
        Vector3 forward = playerHead.forward;
        forward.y = 0;                   
        forward.Normalize();

        Vector3 spawnPos = playerHead.position 
                           + forward * distanceInFront 
                           + new Vector3(0, heightOffset, 0);
        
        menuInstance = Instantiate(menuPrefab, spawnPos, Quaternion.identity);
        
        menuInstance.transform.LookAt(playerHead);
        menuInstance.transform.rotation = Quaternion.Euler(
            0,
            menuInstance.transform.rotation.eulerAngles.y + 180,
            0
        );
    }
}