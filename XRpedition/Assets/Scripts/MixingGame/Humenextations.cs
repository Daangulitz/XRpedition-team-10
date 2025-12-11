using UnityEngine;

public class Humenextations : MonoBehaviour
{
    [SerializeField] private GameObject[] Hats;
    [SerializeField] private GameObject[] Neks;
    [SerializeField] private GameObject[] Faces;

    public void SetHumanThings()
    {
        GameObject hat = Hats[Random.Range(0, Hats.Length)];
        GameObject nek = Neks[Random.Range(0, Neks.Length)];
        GameObject fac = Faces[Random.Range(0, Faces.Length)];

        hat.SetActive(true);
        nek.SetActive(true);
        fac.SetActive(true);
    }
    
    public void SetActiveFalse()
    {
        foreach (GameObject hat in Hats)
        {
            hat.SetActive(false);
        }

        foreach (GameObject nek in Neks)
        {
            nek.SetActive(false);
        }

        foreach (GameObject fac in Faces)
        {
            fac.SetActive(false);
        }
    }
}
