using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

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



    public Task(string npcName, string taskName, string description, int difficulty, BuildingTypes buildingNeeded, int amountNeeded, int reward, TaskTypes taskType, float emissionsTargetPercentage)
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

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

    public void CreateTask(string npcName, string name, string description, int difficulty, BuildingTypes buildingNeeded, int amountNeeded, int reward, TaskTypes taskType, float emissionsTargetPercentage)
    {
        tasks.Add(new Task(npcName, name, description, difficulty, buildingNeeded, amountNeeded, reward, taskType, emissionsTargetPercentage));
        SaveTasks();
    }

    public void TaskCompleted(Task task)
    {
        tasks.Remove(task);
        gameManager.ChangeBalance(task.reward);
        SaveTasks();
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