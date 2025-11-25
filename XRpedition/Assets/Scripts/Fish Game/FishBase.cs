using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public abstract class FishBase : MonoBehaviour
{
    private GameLoop gameLoop;
    [SerializeField] string CurrentColor; 

    void Start()
    {
        gameLoop = GameObject.FindWithTag("GameLoop").GetComponent<GameLoop>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Net"))
        {
            Caught();
        }
    }

    public void Caught()
    {
        // //code for being caught
        // if (gameLoop.CurrentColor == CurrentColor)
        // {
        //     gameLoop.RightFish();
        //     Destroy(gameObject);
        // }
        // else
        // {
        //     gameLoop.WrongFish();
        //     Destroy(gameObject);
        // }
        
        Destroy(gameObject);
        
    }
    
    
}
