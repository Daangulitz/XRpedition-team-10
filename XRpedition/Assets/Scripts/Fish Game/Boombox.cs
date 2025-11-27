using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Boombox : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip BGMusic;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = BGMusic;
        audioSource.Play();
    }
}
