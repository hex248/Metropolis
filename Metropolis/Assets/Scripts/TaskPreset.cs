using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Task", menuName = "Scriptables/Task", order = 1)]
public class TaskPreset : ScriptableObject
{
    public string taskName;
    public string description;
    public string[] dialogue;

    public BuildingTypes buildingNeeded;
    public int amountNeeded;

    public int reward;

    public int difficulty; // 1-5, 1 being easy, 5 being difficult

    public  TaskTypes taskType;

    public float emissionsTargetPercentage;

    public bool isTutorial = false;

    public TaskPreset(string taskName, string description, string[] dialogue, int difficulty, BuildingTypes buildingNeeded, int amountNeeded, int reward, TaskTypes taskType, float emissionsTargetPercentage, bool isTutorial)
    {
        this.taskName = taskName;
        this.description = description;
        this.dialogue = dialogue;
        this.difficulty = difficulty;
        this.buildingNeeded = buildingNeeded;
        this.amountNeeded = amountNeeded;
        this.reward = reward;
        this.taskType = taskType;
        this.emissionsTargetPercentage = emissionsTargetPercentage;
        this.isTutorial = isTutorial;
    }
}
