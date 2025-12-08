using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Net : MonoBehaviour
{
    private FishMovement FishScript;
    
    private AudioSource audioSource;

    [SerializeField] private AudioClip NetSwingSound;
    [SerializeField] private int count;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        count = 0;
    }

    private void Update()
    {
        if (count >= 5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        FishScript = other.GetComponent<FishMovement>();
        if (FishScript != null)
        {
            FishScript.Caught();
            count++;
        }
    }
    
    
}
