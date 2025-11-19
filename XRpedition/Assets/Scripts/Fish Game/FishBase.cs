using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public abstract class FishBase : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Net"))
        {
            Caught();
        }
    }

    public void Caught()
    {
        //code for being caught
        
        Destroy(gameObject);
    }
    
    
}
