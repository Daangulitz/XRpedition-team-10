using System;
using UnityEngine;

public class FinishDoor : MonoBehaviour
{
    [SerializeField] private string HumanName;
    [SerializeField] private GameObject TestParticelEffect;
    [SerializeField] private Transform TargetPosition;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(HumanName))
        {
            Instantiate(TestParticelEffect);
            Destroy(other.gameObject);
        }
    }
}
