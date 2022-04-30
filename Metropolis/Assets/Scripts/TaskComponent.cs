using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskComponent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI taskNameText;
    [SerializeField] TextMeshProUGUI npcNameText;
    [SerializeField] TextMeshProUGUI taskDescriptionText;
    [SerializeField] TextMeshProUGUI taskDifficultyText;

    public Task task;

    void Update()
    {
        taskNameText.text = task.taskName;
        npcNameText.text = task.npcName;
        taskDescriptionText.text = task.description;
        taskDifficultyText.text = $"Difficulty: {task.difficulty}";
    }
}
