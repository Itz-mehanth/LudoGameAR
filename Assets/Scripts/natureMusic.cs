using UnityEngine;

public class NatureMusic : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        src.clip = sfs;
        src.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
