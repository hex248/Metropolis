using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public int population = 0;
    public float emissions;
    public float maximumEmissions = 1000.0f;


    public float timeOfDay = 9.0f;

    public Stats(int population, float emissions)
    {
        this.population = population;
        this.emissions = emissions;
    }
}

public class StatisticsManager : MonoBehaviour
{
    public Stats stats;

    string saveDirectory;
    string savePath;

    GameManager gameManager;
    CityManager cityManager;
    TaskManager taskManager;
    NPCManager npcManager;
    UIManager uiManager;
    List<BuildingTypes> generateRevenue = new List<BuildingTypes>();

    float timePassed = 0.0f;

    Dictionary<BuildingTypes, int> emissions = new Dictionary<BuildingTypes, int>();

    void Start()
    {
        generateRevenue.Add(BuildingTypes.Factory);
        generateRevenue.Add(BuildingTypes.Hospital);
        generateRevenue.Add(BuildingTypes.Supermarket);
        emissions.Add(BuildingTypes.House, 20);
        emissions.Add(BuildingTypes.Flats, 75);
        emissions.Add(BuildingTypes.Factory, 100);
        emissions.Add(BuildingTypes.Supermarket, 20);
        emissions.Add(BuildingTypes.School, 50);
        emissions.Add(BuildingTypes.Hospital, 80);
        emissions.Add(BuildingTypes.Tree, -10);
        emissions.Add(BuildingTypes.Hedge, -5);
        emissions.Add(BuildingTypes.Grass, -2);
        emissions.Add(BuildingTypes.IntersectionRoad, 0);
        emissions.Add(BuildingTypes.StraightRoad, 0);
        emissions.Add(BuildingTypes.TurnRoad, 0);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cityManager = GameObject.Find("CityManager").GetComponent<CityManager>();
        taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();
        npcManager = GameObject.Find("NPCManager").GetComponent<NPCManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        saveDirectory = Application.persistentDataPath + $"/saves/{gameManager.saveName}";
        savePath = saveDirectory + $"/stats.dat";

        LoadStats();
    }

    void Update()
    {
        timePassed += Time.deltaTime;

        // gradually increase balance based on buildings in map
        if (timePassed >= 5.0f)
        {
            timePassed = 0.0f;
            int earnings = 0;
            for (int x = 0; x < cityManager.map.Count; x++)
            {
                for (int y = 0; y < cityManager.map[x].Count; y++)
                {
                    Cell cell = cityManager.map[x][y];
                    if (cell.occupied)
                    {
                        earnings += GetMoneyGenerated(cell.building.buildingType);
                        if (generateRevenue.Contains(cell.building.buildingType))
                        {
                            earnings += 5;
                        }
                    }
                }
            }
            gameManager.ChangeBalance(earnings);
        }

        TaskCreationMonitor();
        TaskCompletionMonitor();

        stats.timeOfDay += Time.deltaTime / 60; // one minute is equal to one hour in game
        stats.timeOfDay %= 24;



        uiManager.UpdateStats();
    }

    private void OnApplicationQuit()
    {
        SaveStats();
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
            stats = new Stats(1, 0.0f);
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

    public void BuildingChange()
    {
        CalculateEmissions();
        CalculatePopulation();
    }

    void CalculateEmissions()
    {
        float emissionsAmount = 0.0f;
        for (int x = 0; x < cityManager.map.Count; x++)
        {
            for (int y = 0; y < cityManager.map[x].Count; y++)
            {
                Cell cell = cityManager.map[x][y];
                if (cell.occupied)
                {
                    emissionsAmount += emissions[cell.building.buildingType];
                }
            }
        }
        if (emissionsAmount < 0.0f) emissionsAmount = 0.0f;
        stats.emissions = emissionsAmount;

        SaveStats();
    }

    void TaskCreationMonitor()
    {
        if (stats.emissions >= stats.maximumEmissions / 1.05) // roughly 95%
        {
            // critically high

            if (taskManager.tasks.Find(task => task.taskName == "Critically High Emissions") != null) return; // forget if they have already been made aware

            npcManager.PopUp(
                npcManager.allNPCS.Find(npc => npc.npcName == "Mr. Blingman"),
                taskManager.taskPresets.Find(task => task.taskName == "Critically High Emissions")
                ); // otherwise, create npc encounter to warn player
        }
        else if (stats.emissions >= stats.maximumEmissions / 1.17) // roughly 85%
        {
            // too high

            if (taskManager.tasks.Find(task => task.taskName == "Very High Emissions") != null) return; // forget if they have already been made aware

            npcManager.PopUp(
                npcManager.allNPCS.Find(npc => npc.npcName == "Mr. Blingman"),
                taskManager.taskPresets.Find(task => task.taskName == "Very High Emissions")
                ); // otherwise, create npc encounter to warn player
        }
        else if (stats.emissions >= stats.maximumEmissions / 1.33) // roughly 75%
        {
            // very high

            if (taskManager.tasks.Find(task => task.taskName == "High Emissions") != null) return; // forget if player has already been made aware

            npcManager.PopUp(
                npcManager.allNPCS.Find(npc => npc.npcName == "Mr. Blingman"),
                taskManager.taskPresets.Find(task => task.taskName == "High Emissions")
                ); // otherwise, create npc encounter to warn player
        }

        int hospitalCount = CountHospitals();
        if (taskManager.tasks.Find(task => task.taskName == "New Hospital") == null)
        {
            if (hospitalCount > 0)
            {
                if (stats.population > 20 && stats.population / hospitalCount > 30) // 30 people per hospital
                {
                    // too many people for the hospital

                    npcManager.PopUp(null, taskManager.taskPresets.Find(t => t.taskName == "New Hospital"));
                }
            }
            else
            {
                if (stats.population > 20)
                {
                    npcManager.PopUp(null, taskManager.taskPresets.Find(t => t.taskName == "New Hospital"));
                }
            }
        }
        int schoolCount = CountSchools();
        if (taskManager.tasks.Find(task => task.taskName == "New School") == null)
        {
            if (schoolCount > 0)
            {
                if (stats.population > 10 && stats.population / schoolCount > 75) // 75 people per school
                {
                    // too many people for the school

                    npcManager.PopUp(null, taskManager.taskPresets.Find(t => t.taskName == "New School"));
                }
            }
            else
            {
                if (stats.population > 10)
                {
                    npcManager.PopUp(null, taskManager.taskPresets.Find(t => t.taskName == "New School"));
                }
            }
        }
    }

    void TaskCompletionMonitor()
    {
        List<Task> emissionTasks = new List<Task>();

        emissionTasks = taskManager.tasks.FindAll(task => task.taskType == TaskTypes.emissions);

        foreach (Task task in emissionTasks)
        {
            if ((stats.emissions / stats.maximumEmissions) * 100 <= task.emissionsTargetPercentage)
            {
                // task complete
                taskManager.TaskCompleted(task);
            }
        }
    }

    void CalculatePopulation()
    {
        int population = 0;
        for (int x = 0; x < cityManager.map.Count; x++)
        {
            for (int y = 0; y < cityManager.map[x].Count; y++)
            {
                Cell cell = cityManager.map[x][y];
                if (cell.occupied)
                {
                    population += cell.occupants;
                }
            }
        }
        stats.population = population;

        SaveStats();
    }

    int CountHospitals()
    {
        int num = 0;
        for (int x = 0; x < cityManager.map.Count; x++)
        {
            for (int y = 0; y < cityManager.map[x].Count; y++)
            {
                Cell cell = cityManager.map[x][y];
                if (cell.occupied && cell.building.buildingType == BuildingTypes.Hospital)
                {
                    num++;
                }
            }
        }

        return num;
    }
    int CountSchools()
    {
        int num = 0;
        for (int x = 0; x < cityManager.map.Count; x++)
        {
            for (int y = 0; y < cityManager.map[x].Count; y++)
            {
                Cell cell = cityManager.map[x][y];
                if (cell.occupied && cell.building.buildingType == BuildingTypes.School)
                {
                    num++;
                }
            }
        }

        return num;
    }

    public int GetMoneyGenerated(BuildingTypes type)
    {
        int money = 0;
        switch (type)
        {
            case BuildingTypes.Hospital:
                money = 50;
                break;
            case BuildingTypes.Supermarket:
                money = 100;
                break;
            case BuildingTypes.Factory:
                money = 200;
                break;
            default:
                money = 0;
                break;
        }

        return money;
    }
}

[System.Serializable]
class StatsSaveFile
{
    public Stats stats;
}