using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class NPCEncounter : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject npcSprite;
    
    GameManager gameManager;
    UIManager uiManager;
    AudioManager audioManager;

    public NPC npc;
    public TaskPreset task;
    public bool isTask;
    public int dialogueIndex;
    public string[] dialogue;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlaySoundEffect("NPCVoice");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleNext();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        HandleNext();
    }

    void HandleNext()
    {
        if (!gameManager.playing)
        {
            // move dialogue along

            dialogueIndex++;
            if (dialogueIndex == dialogue.Length)
            {
                if (!isTask)
                {
                    gameManager.playing = true;
                    uiManager.HideNPCPopup();
                }
                else
                {
                    dialogueText.text = "See your new task in the task menu!";
                }
                return;
            }
            else if (dialogueIndex > dialogue.Length)
            {
                gameManager.playing = true;
                uiManager.HideNPCPopup();

                return;
            }
            else
            {
                //audioManager.PlaySoundEffect("NPCVoice");
            }
            dialogueText.text = dialogue[dialogueIndex];
        }
    }
}
