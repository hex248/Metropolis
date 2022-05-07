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
}
