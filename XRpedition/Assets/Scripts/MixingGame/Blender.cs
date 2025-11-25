using System;
using UnityEngine;

public class Blender : MonoBehaviour
{
    [SerializeField] private FruitOrderManager _Fom;
    [SerializeField] private Transform Bowl;

    private bool CorrectFruitInside;

    private void OnTriggerEnter(Collider other)
    {
        string FruitTag = other.tag;

        if (_Fom.CurrentOrder.Contains(FruitTag))
        {
            CorrectFruitInside = true;
        }
        else
        {
            CorrectFruitInside = false;
        }
    }

    private void Blend()
    {
        if (CorrectFruitInside)
        {
            foreach (Transform child in Bowl)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            
        }
    }
}