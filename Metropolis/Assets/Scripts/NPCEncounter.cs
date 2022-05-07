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

    public NPC npc;
    public TaskPreset task;
    public int dialogueIndex;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!gameManager.playing)
        {
            // move dialogue along

            dialogueIndex++;
            if (dialogueIndex == task.dialogue.Length)
            {
                dialogueText.text = "See your new task in the task menu!";
                return;
            }
            else if (dialogueIndex > task.dialogue.Length)
            {
                gameManager.playing = true;
                uiManager.HideNPCPopup();

                return;
            }
            dialogueText.text = task.dialogue[dialogueIndex];
        }
    }
}
