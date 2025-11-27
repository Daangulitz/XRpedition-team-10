using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public abstract class FishBase : MonoBehaviour
{
    private FishGameLoop gameLoop;
    [SerializeField] string CurrentColor; 

    protected void Start()
    {
        gameLoop = GameObject.FindWithTag("GameLoop").GetComponent<FishGameLoop>();
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
        //Destroy(gameObject);
        if (gameLoop.CurrentColor == CurrentColor)
        {
            gameLoop.RightFish();
            Destroy(gameObject);
        }
        else
        {
            gameLoop.WrongFish();
            Destroy(gameObject);
        }
        
    }
    
    
}
