using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GamePlayModes
{
    none,
    construction,
    destruction
}

enum RotationTypes
{
    clockwise,
    anticlockwise
}

public class GameManager : MonoBehaviour
{
    public string saveName;

    public GamePlayModes currentMode;

    [SerializeField] Transform buildingsParent;
    [SerializeField] Transform pavementParent;
    [SerializeField] Color oldPavementColor;
    [SerializeField] Color constructionBlockedColor;
    [SerializeField] Color destroyableColor;
    [SerializeField] GameObject gameOverPanel;
    public BuildingTypes chosenBuilding;
    public int desiredRotation;

    CityManager cityManager;
    MapBuilder mapBuilder;
    TaskManager taskManager;
    NPCManager npcManager;
    StatisticsManager statsManager;

    UIManager ui;

    public bool playing;

    public int balance = 30000;
    public int costOfAction = 0;

    string saveDirectory;
    string savePath;

    void Start()
    {
        saveDirectory = Application.persistentDataPath + $"/saves/";
        saveName = GetSaveName();
        saveDirectory = Application.persistentDataPath + $"/saves/{saveName}";
        savePath = saveDirectory + $"/balance.dat";
        LoadBalance();

        desiredRotation = 0;
        cityManager = GameObject.Find("CityManager").GetComponent<CityManager>();
        mapBuilder = GameObject.Find("CityManager").GetComponent<MapBuilder>();
        taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();
        npcManager = GameObject.Find("NPCManager").GetComponent<NPCManager>();
        statsManager = GameObject.Find("StatisticsManager").GetComponent<StatisticsManager>();

        ui = GameObject.Find("Canvas").GetComponent<UIManager>();
        playing = true;
    }

    void LateUpdate()
    {
        if (statsManager.stats.emissions < statsManager.stats.maximumEmissions)
        {
            if (Input.GetKeyDown(KeyCode.F8) && playing)
            {
                npcManager.PopUp();
            }
            if (currentMode == GamePlayModes.construction || currentMode == GamePlayModes.destruction)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentMode = GamePlayModes.none;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    currentMode = GamePlayModes.construction;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    mapBuilder.ClearPreview();
                    currentMode = GamePlayModes.destruction;
                }
            }
            if (currentMode == GamePlayModes.construction)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    desiredRotation = Rotate(desiredRotation, 90, RotationTypes.clockwise);
                }
            }
        }
        else
        {
            playing = false;
            StartCoroutine(GameOver());
        }
    }

    public void ChangeBalance(int change)
    {
        balance += change;
        if (change > 0) StartCoroutine(ui.ChangeBalance(change));
        SaveBalance();
    }

    #region
    public void ConstructionButtonPressed()
    {
        currentMode = GamePlayModes.construction;
        ui.HidePopulationMenu();
        ui.HideTaskMenu();
        ui.ActionTaken();
    }

    public void PopulationButtonPressed()
    {
        currentMode = GamePlayModes.none;
        ui.ShowPopulationMenu();
        ui.ActionTaken();
    }
    public void TaskButtonPressed()
    {
        currentMode = GamePlayModes.none;
        ui.ShowTaskMenu();
        ui.ActionTaken();
    }
    public void ExitConstructionMode()
    {
        currentMode = GamePlayModes.none;
    }

    public void ExitPopulationMenu()
    {
        ui.HidePopulationMenu();
    }
    public void ExitTaskMenu()
    {
        ui.HideTaskMenu();
    }

    public void ConstructionOptionSelected(ConstructionOption option)
    {
        chosenBuilding = option.buildingType;
    }

    #endregion

    int Rotate(int originalRotation, int amountToRotate, RotationTypes rotationType)
    {
        int newRotation = originalRotation + amountToRotate;

        while (newRotation > 360)
        {
            newRotation -= 360;
        }
        if (newRotation > 180)
        {
            newRotation = -1 * (360 - newRotation);
        }
        return newRotation;
    }


    void LoadBalance()
    {
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            BalanceSaveFile data = (BalanceSaveFile)bf.Deserialize(file);
            file.Close();

            balance = data.balance;
        }
        else
        {
            SaveBalance();
        }
    }

    void SaveBalance()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        BalanceSaveFile data = new BalanceSaveFile(); // create a new data object to write to
        data.balance = balance; // export current balance
        
        bf.Serialize(file, data);
        file.Close();
    }

    string GetSaveName()
    {
        if (File.Exists(saveDirectory + "/ChosenSave.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveDirectory + "/ChosenSave.txt", FileMode.Open);
            string data = (string)bf.Deserialize(file);
            file.Close();

            return data;
        }
        return "";
    }

    IEnumerator GameOver()
    {
        gameOverPanel.SetActive(true);
        ui.HideActionButton();
        ui.HideStatistics();
        ui.HideArrows();
        ui.HideNPCPopup();
        ui.HidePopulationMenu();
        ui.HideTaskMenu();
        currentMode = GamePlayModes.none;

        yield return new WaitForSeconds(7.5f);

        TitleScreen();

        yield return null;
    }

    public void TitleScreen()
    {
        SceneManager.LoadScene("Title Screen");
    }
}

[System.Serializable]
class BalanceSaveFile
{
    public int balance;
}