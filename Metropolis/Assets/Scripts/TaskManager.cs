using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Task : ScriptableObject
{
    public string npcName;
    public string taskName;
    public string description;

    public int difficulty; // 1-5, 1 being easy, 5 being difficult

    public Task(string npcName, string taskName, string description, int difficulty)
    {
        this.npcName = npcName;
        this.taskName = taskName;
        this.description = description;
        this.difficulty = difficulty;
    }
}


public class TaskManager : MonoBehaviour
{
    public TaskPreset[] taskPresets;

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

    public void CreateTask(string npcName, string name, string description, int difficulty)
    {
        tasks.Add(new Task(npcName, name, description, difficulty));
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