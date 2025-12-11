using UnityEngine;
using System.Collections;
using System.Linq;

public class FishGameLoop : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    [SerializeField] private float TimeActive = 5f;
    [SerializeField] private Material[] FishnetMat;

    private GameObject CatchRed;
    private GameObject CatchBlue;
    private GameObject CatchGreen;
    private GameObject CatchYellow;
    private GameObject CatchPurple;
    private GameObject CatchOrange;
    private GameObject Wrong;
    private GameObject Right;

    private GameObject[] allCatchUI;
    private GameObject currentActiveUI;
    
    [SerializeField] private Renderer[] fishNetRenderers;

    public string CurrentColor;
    
    private FishSpawner FishSpawner;


    private void Update()
    {
        if (Canvas == null)
        {
            Canvas = GameObject.FindWithTag("FishGameCanvas");
            // Find UI elements
            CatchRed = Canvas.transform.Find("Red").gameObject;
            CatchBlue = Canvas.transform.Find("Blue").gameObject;
            CatchGreen = Canvas.transform.Find("Green").gameObject;
            CatchYellow = Canvas.transform.Find("Yellow").gameObject;
            CatchPurple = Canvas.transform.Find("Purple").gameObject;
            CatchOrange = Canvas.transform.Find("Orange").gameObject;
            Wrong = Canvas.transform.Find("Wrong").gameObject;
            Right = Canvas.transform.Find("Right").gameObject;

            allCatchUI = new GameObject[] {
                CatchRed, CatchBlue, CatchGreen, CatchYellow, CatchPurple, CatchOrange
            };
        
            FishSpawner = GameObject.FindWithTag("Spawner").GetComponent<FishSpawner>();
        
            fishNetRenderers = GameObject.FindGameObjectsWithTag("FishNet")
                .Select(go => go.GetComponent<Renderer>())
                .Where(r => r != null)
                .ToArray();
        
            DisableAllUI();
            EnableUI("fish");
        }
    }
    
    
    private void EnableUI(string type)
    {
        StartCoroutine(EnableUISequence(type));
    }

    private IEnumerator EnableUISequence(string type)
    {
        if (type == "fish")
        {
            int randomIndex = Random.Range(0, allCatchUI.Length);
            currentActiveUI = allCatchUI[randomIndex];

            currentActiveUI.SetActive(true);
            CurrentColor = currentActiveUI.name;

            SpawnFish(CurrentColor);
            UpdateFishNetMaterial(CurrentColor);
        }
        else if (type == "right")
        {
            // Hide old objective
            currentActiveUI?.SetActive(false);

            currentActiveUI = Right;
            currentActiveUI.SetActive(true);

            yield return new WaitForSeconds(TimeActive);
            currentActiveUI.SetActive(false);

            EnableUI("fish");
            yield break;
        }
        else if (type == "wrong")
        {
            // Hide old objective
            currentActiveUI?.SetActive(false);

            currentActiveUI = Wrong;
            currentActiveUI.SetActive(true);

            yield return new WaitForSeconds(TimeActive);
            currentActiveUI.SetActive(false);

            EnableUI("fish");
            yield break;
        }
    }


    private void DisableAllUI()
    {
        foreach (var ui in allCatchUI)
            ui.SetActive(false);

        Wrong.SetActive(false);
        Right.SetActive(false);
    }

    public void WrongFish()
    {
        Debug.Log("Wrong fish!");
        EnableUI("wrong");
    }

    public void RightFish()
    {
        Debug.Log("Right fish!");
        EnableUI("right");
    }

    private void SpawnFish(string color)
    {
        switch (color)
        {
            case "Red":    FishSpawner.SpawnSpecificFish(0); FishSpawner.SpawnSpecificFish(0); FishSpawner.SpawnSpecificFish(0); break;
            case "Orange": FishSpawner.SpawnSpecificFish(1); FishSpawner.SpawnSpecificFish(1); FishSpawner.SpawnSpecificFish(1); break;
            case "Yellow": FishSpawner.SpawnSpecificFish(2); FishSpawner.SpawnSpecificFish(2); FishSpawner.SpawnSpecificFish(2); break;
            case "Green":  FishSpawner.SpawnSpecificFish(3); FishSpawner.SpawnSpecificFish(3); FishSpawner.SpawnSpecificFish(3); break;
            case "Blue":   FishSpawner.SpawnSpecificFish(4); FishSpawner.SpawnSpecificFish(4); FishSpawner.SpawnSpecificFish(4); break;
            case "Purple": FishSpawner.SpawnSpecificFish(5); FishSpawner.SpawnSpecificFish(5); FishSpawner.SpawnSpecificFish(5); break;
        }
    }
    
    private void UpdateFishNetMaterial(string color)
    {
        int index = color switch
        {
            "Red"    => 0,
            "Orange" => 1,
            "Yellow" => 2,
            "Green"  => 3,
            "Blue"   => 4,
            "Purple" => 5,
            _ => -1
        };

        if (index < 0 || index >= FishnetMat.Length)
        {
            Debug.LogWarning("Invalid fishnet material index for color: " + color);
            return;
        }

        foreach (var rend in fishNetRenderers)
        {
            rend.material = FishnetMat[index];
        }
    }
}
