using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField] NPC[] allNPCS;

    UIManager uiManager;
    TaskManager taskManager;
    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();
        PopUp();
    }

    public void PopUp()
    {
        Debug.Log("a");
        NPCEncounter npcEncounter = uiManager.NPCPopup();
        Debug.Log(npcEncounter);
        NPC npc = allNPCS[Random.Range(0, allNPCS.Length-1)];
        Debug.Log(npc);

        TaskPreset task = taskManager.taskPresets[Random.Range(0, taskManager.taskPresets.Length - 1)];
        foreach (string line in task.dialogue)
        {
            if (line.Contains("{NPC-NAME}"))
            {
                line.Replace("{NPC-NAME}", npc.npcName);
            }
            Debug.Log(line);
        }

        npcEncounter.npcNameText.text = npc.npcName;
        npcEncounter.dialogueText.text = task.dialogue[0];

        taskManager.tasks.Add(new Task(npc.npcName, task.taskName, task.description, task.difficulty));
    }
}
