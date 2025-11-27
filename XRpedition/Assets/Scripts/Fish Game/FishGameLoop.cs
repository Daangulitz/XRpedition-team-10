using UnityEngine;
using System.Collections;

public class FishGameLoop : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    [SerializeField] private float TimeActive = 5f;

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

    public string CurrentColor;
    
    private FishSpawner FishSpawner;

    void Awake()
    {
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
        
        FishSpawner = GameObject.FindWithTag("Spawner").GetComponent<FishSpawner>();    }

    void Start()
    {
        DisableAllUI();
        EnableUI("fish");
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

            yield return new WaitForSeconds(TimeActive);
            currentActiveUI.SetActive(false);
        }
        else if (type == "right")
        {
            currentActiveUI = Right;
            currentActiveUI.SetActive(true);

            yield return new WaitForSeconds(TimeActive);
            currentActiveUI.SetActive(false);

            EnableUI("fish");
            yield break;
        }
        else if (type == "wrong")
        {
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
            case "Red":
                FishSpawner.SpawnSpecificFish(0);
                break;            
            case "Orange":
                FishSpawner.SpawnSpecificFish(1);
                break;            
            case "Yellow":
                FishSpawner.SpawnSpecificFish(2);
                break;
            case "Green":
                FishSpawner.SpawnSpecificFish(3);
                break;
            case "Blue":
                FishSpawner.SpawnSpecificFish(4);
                break;
            case "Purple":
                FishSpawner.SpawnSpecificFish(5);
                break;
        }

    }
    
}
