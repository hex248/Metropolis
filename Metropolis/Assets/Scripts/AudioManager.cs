using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioSource music;
    [SerializeField] AudioClip insufficientFunds;
    [SerializeField] AudioClip taskCompleted;
    [SerializeField] AudioClip npcVoice;
    AudioClip playingClip;

    [SerializeField, Range(0, 100)] float volume;

    public void PlaySoundEffect(string sfxName)
    {
        AudioClip clip;
        switch (sfxName)
        {
            case "InsufficientFunds":
                clip = insufficientFunds;
                break;
            case "TaskComplete":
                clip = taskCompleted;
                break;
            case "NPCVoice":
                clip = npcVoice;
                break;
            default:
                clip = insufficientFunds;
                break;
        }

        if (playingClip != clip)
        {
            playingClip = clip;

            sfx.PlayOneShot(clip, volume / 100);
        }
    }
}
