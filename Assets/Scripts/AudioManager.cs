using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource source;
    public AudioClip loadedImage;
    public AudioClip loadingClip;
    public AudioClip BGM;
    public AudioClip onClick;

    public AudioClip monitorButtonPress;

    public bool isMuted;
    public static AudioManager instance { get; private set;}
  
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        source = GetComponent<AudioSource>();
        PlayBGM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGM()
    {
        source.clip = BGM;
        source.Play(0);
    }
    public void PlayRecordClip()
    {
        source.PlayOneShot(onClick);
    }

    public void PlayMonitorButtonClip()
    {
        source.PlayOneShot(monitorButtonPress);
    }
}
