using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(AudioSource))]
public abstract class FishBase : MonoBehaviour
{
    private FishGameLoop gameLoop;
    [SerializeField] string CurrentColor;
    
    private AudioSource audioSource;
    [SerializeField] private AudioClip FishCaught;

    [SerializeField] private float RadiusDestroy;

    private FishSpawner spawner;

    protected void Start()
    {
        gameLoop = GameObject.FindWithTag("GameLoop").GetComponent<FishGameLoop>();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        spawner = GameObject.FindWithTag("Spawner").GetComponent<FishSpawner>();
    }

    protected void Update()
    {
        if (transform.position.x >= RadiusDestroy ||
            transform.position.x <= -RadiusDestroy ||
            transform.position.y >= RadiusDestroy ||
            transform.position.y <= -RadiusDestroy ||
            transform.position.z >= RadiusDestroy ||
            transform.position.z <= -RadiusDestroy)
        {
            spawner.SpawnOneRandomFish();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FishNet"))
        {
            Caught();
        }
    }

    public void Caught()
    {
        if (gameLoop.CurrentColor == CurrentColor)
        {
            audioSource.PlayOneShot(FishCaught);
            gameLoop.RightFish();
            Destroy(gameObject);
        }
        else
        {
            gameLoop.WrongFish();
        }
        
    }
    
    
}
