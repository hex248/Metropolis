using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    public List<NPC> allNPCS = new List<NPC>();

    UIManager uiManager;
    TaskManager taskManager;
    GameManager gameManager;
    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void PopUp(NPC chosenNPC=null, TaskPreset chosenTask=null)
    {
        gameManager.playing = false;

        NPCEncounter npcEncounter = uiManager.NPCPopup();
        NPC npc; 
        if (chosenNPC) npc = chosenNPC;
        else npc = allNPCS[Random.Range(0, allNPCS.Count)];

        TaskPreset task;
        if (chosenTask) task = chosenTask;
        else task = taskManager.taskPresets[Random.Range(0, taskManager.taskPresets.FindAll(t => t.taskType == TaskTypes.construction && !t.isTutorial).Count)];

        for (int i = 0; i < task.dialogue.Length; i++)
        {
            if (task.dialogue[i].Contains("{NPC-NAME}"))
            {
                task.dialogue[i] = task.dialogue[i].Replace("{NPC-NAME}", npc.npcName);
            }
        }

        npcEncounter.npcNameText.text = npc.npcName;
        npcEncounter.dialogueText.text = task.dialogue[0];
        npcEncounter.task = task;
        npcEncounter.isTask = true;
        npcEncounter.dialogue = task.dialogue;
        npcEncounter.npc = npc;
        npcEncounter.dialogueIndex = 0;
        npcEncounter.npcSprite.GetComponent<Image>().sprite = npc.npcIdleSprite;

        taskManager.CreateTask(npc.npcName, task.taskName, task.description, task.difficulty, task.buildingNeeded, task.amountNeeded, task.reward, task.taskType, task.emissionsTargetPercentage, task.isTutorial);
    }
}
