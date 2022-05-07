using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioSource music;
    [SerializeField] AudioClip insufficientFunds;

    [SerializeField, Range(0, 100)] float volume;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySoundEffect(string sfxName)
    {
        AudioClip clip;
        switch (sfxName)
        {
            case "InsufficientFunds":
                clip = insufficientFunds;
                break;
            default:
                clip = insufficientFunds;
                break;
        }

        if (!sfx.isPlaying) sfx.PlayOneShot(clip, volume/100);
    }
}
