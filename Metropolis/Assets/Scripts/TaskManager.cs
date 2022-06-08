using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum TaskTypes
{
    construction,
    emissions
}

[System.Serializable]
public class Task
{
    public string npcName;
    public string taskName;
    public string description;

    public int difficulty; // 1-5, 1 being easy, 5 being difficult

    public BuildingTypes buildingNeeded;
    public int completed;
    public int amountNeeded;

    public int reward;

    public TaskTypes taskType;

    public float emissionsTargetPercentage;

    public bool seen = false;

    public bool isTutorial;



    public Task(string npcName, string taskName, string description, int difficulty, BuildingTypes buildingNeeded, int amountNeeded, int reward, TaskTypes taskType, float emissionsTargetPercentage, bool isTutorial)
    {
        this.npcName = npcName;
        this.taskName = taskName;
        this.description = description;
        this.difficulty = difficulty;
        this.buildingNeeded = buildingNeeded;
        this.amountNeeded = amountNeeded;
        this.reward = reward;
        this.taskType = taskType;
        this.emissionsTargetPercentage = emissionsTargetPercentage;
        this.isTutorial = isTutorial;
    }

    public bool Add(int amount=1)
    {
        completed += amount;
        if (completed >= amountNeeded)
        {
            return true;
        }
        return false;
    }
}


public class TaskManager : MonoBehaviour
{
    public List<TaskPreset> taskPresets = new List<TaskPreset>();

    public List<Task> tasks = new List<Task>();

    string saveDirectory;
    string savePath;

    GameManager gameManager;
    NPCManager npcManager;
    AudioManager audioManager;
    UIManager uiManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        npcManager = GameObject.Find("NPCManager").GetComponent<NPCManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        saveDirectory = Application.persistentDataPath + $"/saves/{gameManager.saveName}";
        savePath = saveDirectory + $"/tasks.dat";

        LoadTasks();
    }


    public void NewBuilding(BuildingTypes buildingType)
    {
        if (tasks.Count > 0)
        {
            foreach (Task task in tasks)
            {
                if (task.buildingNeeded == buildingType)
                {
                    bool taskCompleted = task.Add();
                    if (taskCompleted) TaskCompleted(task);
                    break;
                }
            }
        }
    }

    public void CreateTask(string npcName, string name, string description, int difficulty, BuildingTypes buildingNeeded, int amountNeeded, int reward, TaskTypes taskType, float emissionsTargetPercentage, bool isTutorial)
    {
        tasks.Add(new Task(npcName, name, description, difficulty, buildingNeeded, amountNeeded, reward, taskType, emissionsTargetPercentage, isTutorial));
        SaveTasks();
    }

    public void TaskCompleted(Task task)
    {
        tasks.Remove(task);
        gameManager.ChangeBalance(task.reward);
        SaveTasks();
        audioManager.PlaySoundEffect("TaskComplete");

        if (task.taskName == "Build a house")
        {
            // create a follow up task
            string[] dialogue = {"Hey! I'm {NPC-NAME}, I just moved into the city!", "It's great here, but it needs some more nature.", "Could you plant some trees?"};
            List<NPC> npcs = npcManager.allNPCS.FindAll(npc => npc.npcName != "Mr. Blingman");
            npcManager.PopUp(npcs[Random.Range(0, npcs.Count)], new TaskPreset("Decorate the city", "Use the construction menu to plant 10 trees in order to decorate the city.", dialogue, 3, BuildingTypes.Tree, 10, 1500, TaskTypes.construction, 0, false));
        }
        else if (task.taskName == "Decorate the city")
        {
            gameManager.playing = false;
            NPCEncounter npcEncounter = uiManager.NPCPopup();

            string[] dialogue = { $"The trees look amazing! Thank you very much. (+${task.reward})" };
            npcEncounter.npcNameText.text = task.npcName;
            npcEncounter.isTask = false;
            npcEncounter.dialogue = dialogue;
            npcEncounter.dialogueIndex = 0;
            npcEncounter.dialogueText.text = dialogue[0];
            npcEncounter.npcSprite.GetComponent<Image>().sprite = npcManager.allNPCS.Find(npc=>npc.npcName == task.npcName).npcIdleSprite;
        }
        else
        {
            gameManager.playing = false;
            NPCEncounter npcEncounter = uiManager.NPCPopup();

            string[] dialogue = { $"Thank you very much, Mayor! (+${task.reward})" };
            npcEncounter.npcNameText.text = task.npcName;
            npcEncounter.isTask = false;
            npcEncounter.dialogue = dialogue;
            npcEncounter.dialogueIndex = 0;
            npcEncounter.dialogueText.text = dialogue[0];
            npcEncounter.npcSprite.GetComponent<Image>().sprite = npcManager.allNPCS.Find(npc => npc.npcName == task.npcName).npcIdleSprite;
        }
    }

    void LoadTasks()
    {
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            TaskSaveFile data = (TaskSaveFile)bf.Deserialize(file);
            file.Close();

            tasks = data.tasks;
        }
        else
        {
            SaveTasks();
        }
    }

    void SaveTasks()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        TaskSaveFile data = new TaskSaveFile(); // create a new data object to write to
        data.tasks = tasks; // export current tasks

        bf.Serialize(file, data);
        file.Close();
    }
}

[System.Serializable]
class TaskSaveFile
{
    public List<Task> tasks = new List<Task>();
}