using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    [SerializeField] private float TimeActive = 2f;

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

    public string CurrentColor; /*{ get; private set; }*/


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
            CatchRed, CatchBlue, CatchGreen, CatchYellow, CatchPurple, CatchOrange,
        };
    }

    void Start()
    {
        DisableAllUI();
        EnableUI("fish");
    }

    private void EnableUI(string x)
    {
        // Kies random UI object
        if (x == "fish")
        {
            int randomIndex = Random.Range(0, allCatchUI.Length);
            currentActiveUI = allCatchUI[randomIndex];

            currentActiveUI.SetActive(true);
            CurrentColor = currentActiveUI.name;
        }
        else if (x == "right")
        {
            currentActiveUI = Right;
            currentActiveUI.SetActive(true);
            EnableUI("fish");
        }        
        else if (x == "wrong")
        {
            currentActiveUI = Wrong;
            currentActiveUI.SetActive(true);
            EnableUI("fish");
        }
        

        // Na TimeActive weer uit
        StartCoroutine(DisableAfterTime());
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(TimeActive);
        DisableUI();
    }

    private void DisableUI()
    {
        if (currentActiveUI != null)
            currentActiveUI.SetActive(false);
    }

    private void DisableAllUI()
    {
        foreach (var ui in allCatchUI)
            ui.SetActive(false);
        
        Wrong.SetActive((false));
        Right.SetActive((false));
    }
    

    public void WrongFish()
    {
        Debug.Log("Wrong fish!");
        // eventueel nieuwe UI starten
        EnableUI("wrong");
    }

    public void RightFish()
    {
        Debug.Log("Right fish!");
        // eventueel nieuwe UI starten
        EnableUI("right");
    }
}