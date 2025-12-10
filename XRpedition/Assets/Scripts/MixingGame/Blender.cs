using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Blender : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FruitOrderManager _Fom;
    [SerializeField] private AudioClip InBlender;
    [SerializeField] private AudioClip Blendering;
    public TextMeshProUGUI BlenderText;
    [SerializeField] private GameObject KlemBordPrefab;
    [SerializeField] private Transform KlemBordSpawnPoint;

    [Header("Liquid Material")]
    [SerializeField] private Renderer LiquidRenderer;   
    private Material LiquetteMaterial;

    private float matfill = 0.1f;
    private AudioSource audioSource;
    private Animator animator;

    private List<string> FruitsAdded = new List<string>();
    private HashSet<Collider> processedFruits = new HashSet<Collider>();
    private float triggerDelay = 0.1f;

    private int ResetGameInt;

    public bool IsCorrect;

    [SerializeField] private SmoothieGlassShader SmoothieGlassShader;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (_Fom == null)
            _Fom = FindObjectOfType<FruitOrderManager>();

        BlenderText.text = "Wacht op de klant";

        if (LiquidRenderer != null)
        {
            LiquetteMaterial = LiquidRenderer.material;
            LiquetteMaterial.SetFloat("_Fill", matfill);
        }
        else
        {
            Debug.LogWarning("Blender: LiquidRenderer not assigned!");
        }

        SmoothieGlassShader = FindObjectOfType<SmoothieGlassShader>();
    }

    private void Update()
    {
        if (_Fom == null)
            _Fom = FindObjectOfType<FruitOrderManager>();
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

        // Handle clipboard
        if (fruit.CompareTag("Klembord"))
        {
            Instantiate(KlemBordPrefab, KlemBordSpawnPoint.position, KlemBordSpawnPoint.rotation);
            yield return new WaitForSeconds(0.05f);
            processedFruits.Remove(fruit);
            yield break;
        }

        string fruitTag = fruit.tag;

        // Valid fruit
        if (_Fom != null && _Fom.IsValidFruit(fruitTag))
        {
            FruitsAdded.Add(fruitTag);

            // Update material fill
            matfill += 0.2f;
            if (LiquetteMaterial != null)
                LiquetteMaterial.SetFloat("_Fill", matfill);

            // Sound
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
        IsCorrect = allCorrect;
        BlenderText.text = "Geef de klant het glas.";

        FruitsAdded.Clear();

        StartCoroutine(ResetFillAfterDelay());
    }

    private IEnumerator ResetFillAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        StartCoroutine(SmoothDrain(0.1f, 1.2f));
        SmoothieGlassShader.FillGlass();

        // 🔥 RESET GLASS TRIGGER FOR NEXT ROUND
        SmoothieGlassTrigger trigger = FindFirstObjectByType<SmoothieGlassTrigger>();
        if (trigger != null)
            trigger.ResetGlassTrigger();

        if (ResetGameInt >= 3)
            ResetGame();
    }

    private IEnumerator SmoothDrain(float targetFill, float duration)
    {
        float startFill = matfill;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            matfill = Mathf.Lerp(startFill, targetFill, time / duration);

            if (LiquetteMaterial != null)
                LiquetteMaterial.SetFloat("_Fill", matfill);

            yield return null;
        }

        matfill = targetFill;

        if (LiquetteMaterial != null)
            LiquetteMaterial.SetFloat("_Fill", matfill);
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
