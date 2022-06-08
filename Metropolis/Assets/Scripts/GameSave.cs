using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSave : MonoBehaviour
{
    public string saveName;
    public TextMeshProUGUI saveNameText;

    void Update()
    {
        saveNameText.text = saveName;
    }
}
