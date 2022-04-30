using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public float emissions;
    public float maximumEmissions = 1000.0f;

    public Stats(float emissions)
    {
        this.emissions = emissions;
    }
}

public class StatisticsManager : MonoBehaviour
{
    public Stats stats;

    string saveDirectory;
    string savePath;

    GameManager gameManager;
    UIManager uiManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        saveDirectory = Application.persistentDataPath + $"/saves/{gameManager.saveName}";
        savePath = saveDirectory + $"/stats.dat";

        LoadStats();
    }

    void Update()
    {
        uiManager.UpdateStats();
        if (stats.emissions / stats.maximumEmissions >= 1.0f)
        {
            // FAIL

            Debug.Log("FAILED");
        }
    }

    void LoadStats()
    {
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            StatsSaveFile data = (StatsSaveFile)bf.Deserialize(file);
            file.Close();

            stats = data.stats;
        }
        else
        {
            stats = new Stats(0.0f);
            SaveStats();
        }
    }

    void SaveStats()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        StatsSaveFile data = new StatsSaveFile(); // create a new data object to write to
        data.stats = stats; // export current stats

        bf.Serialize(file, data);
        file.Close();
    }

    public void BuildingChange(BuildingTypes buildingType, bool construction)
    {
        float multiplier = -0.8f;
        if (construction) multiplier = 1.0f;

        switch (buildingType)
        {
            case BuildingTypes.Factory:
                // 100 points
                stats.emissions += 100 * multiplier;
                break;
            case BuildingTypes.Hospital:
                // 80 points
                stats.emissions += 80 * multiplier;
                break;
            case BuildingTypes.School:
                // 50 points
                stats.emissions += 50 * multiplier;
                break;
            case BuildingTypes.Office:
                // 40 points
                stats.emissions += 40 * multiplier;
                break;
            case BuildingTypes.House:
                // 20 points
                stats.emissions += 20 * multiplier;
                break;
            case BuildingTypes.Shop:
                // 10 points
                stats.emissions += 10 * multiplier;
                break;
            case BuildingTypes.Tree:
                // -10 points
                stats.emissions += -10 * multiplier;
                break;
            case BuildingTypes.Hedge:
                // -5 points
                stats.emissions += -5 * multiplier;
                break;
            case BuildingTypes.Grass:
                // -2 points
                stats.emissions += -2 * multiplier;
                break;
            /*
                50 plots of grass makes up for one factory's emissions
                2 hedges make up for one shop
            */
        }
        SaveStats();
    }
}

[System.Serializable]
class StatsSaveFile
{
    public Stats stats;
}