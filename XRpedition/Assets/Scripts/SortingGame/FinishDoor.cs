using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FinishDoor : MonoBehaviour
{
    [SerializeField] private string[] HumanName = { "HumanBlue", "HumanRed", "HumanYellow", "HumanGreen" };
    [SerializeField] private Material[] Materials; // same length as HumanName
    [SerializeField] private Renderer[] doorCubes; // all cube renderers

    private List<string> remainingHumans;
    private List<Material> remainingMaterials;
    private string currentHumanTag;
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private void Start()
    {
        // Initialize remaining humans/materials lists
        remainingHumans = new List<string>(HumanName);
        remainingMaterials = new List<Material>(Materials);
        audioSource = GetComponent<AudioSource>();

        if (doorCubes.Length == 0)
        {
            doorCubes = GetComponentsInChildren<Renderer>();
        }

        SetNewHuman();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(currentHumanTag))
        {
            Destroy(other.gameObject);
            audioSource.clip = audioClip;
            audioSource.Play();
            
            int indexToRemove = remainingHumans.IndexOf(currentHumanTag);
            if (indexToRemove >= 0)
            {
                remainingHumans.RemoveAt(indexToRemove);
                remainingMaterials.RemoveAt(indexToRemove);
            }
            
            if (remainingHumans.Count > 0)
            {
                SetNewHuman();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void SetNewHuman()
    {
        int index = Random.Range(0, remainingHumans.Count);
        currentHumanTag = remainingHumans[index];
        Material mat = remainingMaterials[index];

        // Change all cubes to this material
        foreach (Renderer rend in doorCubes)
        {
            rend.material = mat;
        }

        Debug.Log("Next target human: " + currentHumanTag);
    }
}
