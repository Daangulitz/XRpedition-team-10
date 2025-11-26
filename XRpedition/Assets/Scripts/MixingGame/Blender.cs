using System;
using System.Collections.Generic;
using UnityEngine;

public class Blender : MonoBehaviour
{
    [SerializeField] private FruitOrderManager _Fom;
    [SerializeField] private Transform Bowl;

    private int FruitInside = 0;
    
    public List<string> FruitsAdded = new List<string>();

    private bool CorrectFruitInside;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        _Fom = FindObjectOfType<FruitOrderManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        string FruitTag = other.tag;

        if (_Fom.IsValidFruit(FruitTag))
        {
            FruitsAdded.Add(FruitTag);
            FruitInside++;
        }
        
        Destroy(other.gameObject);

        if (FruitInside >= 3)
        {
            Blend();
        }
    }

    private void Blend()
    {
        animator.SetTrigger("Mixing");
        foreach (Transform child in Bowl)
        { 
            Destroy(child.gameObject);
        }
    }
}