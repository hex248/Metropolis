using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

public class LoadSaves : MonoBehaviour
{
    string saveDirectory;
    string savePath;

    [SerializeField] GameObject savesPanel;
    [SerializeField] GameObject savePrefab;
    [SerializeField] TextMeshProUGUI saveNameTextArea;

    Scenes sceneManager;

    void Start()
    {
        saveDirectory = Application.persistentDataPath + "/saves";
        savePath = saveDirectory + $"/ChosenSave.txt";
        if (!Directory.Exists(saveDirectory)) Directory.CreateDirectory(saveDirectory);
        if (!File.Exists(savePath)) File.Create(savePath);

        sceneManager = GameObject.Find("Scenes").GetComponent<Scenes>();

        AddSavesToDisplay();
    }

    public void ChooseSave(GameSave save)
    {
        Debug.Log(30);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);

        bf.Serialize(file, save.saveName);
        file.Close();

        sceneManager.SwitchToMainScene();
    }

    public void AddSavesToDisplay()
    {
        foreach (Transform child in savesPanel.transform)
        {
            Destroy(child.gameObject);
        }
        float lastY = 0.0f;

        string[] saves = Directory.GetDirectories(saveDirectory);
        
        foreach (string save in saves)
        {
            GameObject taskObject = Instantiate(savePrefab, savesPanel.transform);
            taskObject.GetComponent<GameSave>().saveName = save.Split('\\').Last();
            lastY = taskObject.transform.position.y;
        }

        RectTransform rt = savesPanel.GetComponent<RectTransform>();
        float height = (215.0f * saves.Length) + 15.0f;
        if (height < 700.0f) height = 700.0f;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
    }

    public void NewSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);

        bf.Serialize(file, saveNameTextArea.text);
        file.Close();

        sceneManager.SwitchToMainScene();
    }
}