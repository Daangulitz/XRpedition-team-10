using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blender : MonoBehaviour
{
    [SerializeField] private FruitOrderManager _Fom;
    [SerializeField] private AudioClip InBlender;
    [SerializeField] private AudioClip Blendering;
    [SerializeField] private TextMeshProUGUI BlenderText;

    private AudioSource audioSource;
    private Animator animator;

    private int FruitInside = 0;
    private List<string> FruitsAdded = new List<string>();

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (_Fom == null)
            _Fom = FindObjectOfType<FruitOrderManager>();

        BlenderText.text = ""; 
    }

    private void OnTriggerEnter(Collider other)
    {
        string fruitTag = other.tag;

        if (_Fom.IsValidFruit(fruitTag))
        {
            FruitsAdded.Add(fruitTag);
        }
        else
        {
            FruitsAdded.Add("WRONG");
        }
        
        FruitInside++;
        audioSource.clip = InBlender;
        audioSource.Play();
        Destroy(other.gameObject);
        
        if (FruitInside >= 3)
        {
            Blend();
        }
    }

    private void Blend()
    {
        animator.SetTrigger("Mixing");

        audioSource.clip = Blendering;
        audioSource.Play();

        bool allCorrect = _Fom.IsCorrectCombination(FruitsAdded);

        if (allCorrect)
        {
            BlenderText.text = "Goed Gedaan";
        }
        else
        {
            BlenderText.text = "Verkeerde Combinatie";
        }
        
        _Fom.NewOrder();
        FruitsAdded.Clear();
        FruitInside = 0;
    }
}
