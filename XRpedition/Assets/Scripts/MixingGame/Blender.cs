// Blender.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Blender : MonoBehaviour
{
    [SerializeField] private FruitOrderManager _Fom;
    [SerializeField] private AudioClip InBlender;
    [SerializeField] private AudioClip Blendering;
    [SerializeField] private TextMeshProUGUI BlenderText;
    [SerializeField] private GameObject KlemBordPrefab;
    [SerializeField] private Transform KlemBordSpawnPoint;
    [SerializeField] private Material LiquetteMaterial;

    private float matfill = 0.1f;
    private AudioSource audioSource;
    private Animator animator;
    private List<string> FruitsAdded = new List<string>();
    private HashSet<Collider> processedFruits = new HashSet<Collider>();
    private float triggerDelay = 1f;
    private int ResetGameInt;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (_Fom == null) _Fom = FindObjectOfType<FruitOrderManager>();
        BlenderText.text = "";
        if (LiquetteMaterial != null) LiquetteMaterial.SetFloat("Fill", matfill);
    }

    private void Update()
    {
        if (_Fom == null) _Fom = FindObjectOfType<FruitOrderManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (processedFruits.Contains(other)) return;
        StartCoroutine(ProcessFruit(other));
    }

    private IEnumerator ProcessFruit(Collider fruit)
    {
        processedFruits.Add(fruit);

        if (fruit == null)
        {
            processedFruits.Remove(fruit);
            yield break;
        }

        if (fruit.CompareTag("Klembord"))
        {
            Instantiate(KlemBordPrefab, KlemBordSpawnPoint.position, KlemBordSpawnPoint.rotation);
            yield return new WaitForSeconds(0.05f);
            processedFruits.Remove(fruit);
            yield break;
        }

        string fruitTag = fruit.tag;

        if (_Fom != null && _Fom.IsValidFruit(fruitTag))
        {
            FruitsAdded.Add(fruitTag);
            matfill += 0.133f;
            if (LiquetteMaterial != null) LiquetteMaterial.SetFloat("Fill", matfill);

            audioSource.clip = InBlender;
            audioSource.Play();

            yield return new WaitForSeconds(0.05f);
            Destroy(fruit.gameObject);
        }
        else
        {
            processedFruits.Remove(fruit);
            yield break;
        }

        yield return new WaitForSeconds(triggerDelay);
        processedFruits.Remove(fruit);

        if (_Fom != null && FruitsAdded.Count >= _Fom.CurrentOrder.Count)
            Blend();
    }

    private void Blend()
    {
        animator.SetTrigger("Mixing");
        ResetGameInt++;
        audioSource.clip = Blendering;
        audioSource.Play();

        bool allCorrect = _Fom != null && _Fom.IsCorrectCombination(FruitsAdded);
        BlenderText.text = allCorrect ? "Goed Gedaan" : "Verkeerde Combinatie";

        if (_Fom != null) _Fom.NewOrder();
        FruitsAdded.Clear();

        StartCoroutine(ResetFillAfterDelay());
    }

    private IEnumerator ResetFillAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        matfill = 0.1f;
        if (LiquetteMaterial != null) LiquetteMaterial.SetFloat("Fill", matfill);
        if (ResetGameInt >= 3) ResetGame();
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
