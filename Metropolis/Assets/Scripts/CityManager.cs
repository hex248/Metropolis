using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Cell
{
    public int x;
    public int y;
    public bool occupied;
    public GameBuilding building;
    public int occupants = 0;

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.occupied = false;
    }
}
public class CityManager : MonoBehaviour
{
    GameManager gameManager;
    AudioManager audioManager;
    UIManager uiManager;
    TaskManager taskManager;
    StatisticsManager statsManager;
    NPCManager npcManager;

    MapBuilder mapBuilder;
    public List<List<Cell>> map = new List<List<Cell>>();

    string saveDirectory;
    string savePath;

    [SerializeField][Range(0,1000)] int mapSize = 100;

    Dictionary<BuildingTypes, int> constructionCosts = new Dictionary<BuildingTypes, int>();

    bool levelLoaded;

    void Start()
    {
        mapBuilder = GetComponent<MapBuilder>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();
        statsManager = GameObject.Find("StatisticsManager").GetComponent<StatisticsManager>();
        npcManager = GameObject.Find("NPCManager").GetComponent<NPCManager>();
        saveDirectory = Application.persistentDataPath + $"/saves/{gameManager.saveName}";
        savePath = saveDirectory + $"/map.dat";
        // load level
        levelLoaded = false;

        constructionCosts.Add(BuildingTypes.House, 300);
        constructionCosts.Add(BuildingTypes.Flats, 1500);
        constructionCosts.Add(BuildingTypes.Factory, 1500);
        constructionCosts.Add(BuildingTypes.Supermarket, 600);
        constructionCosts.Add(BuildingTypes.School, 750);
        constructionCosts.Add(BuildingTypes.Hospital, 900);
        constructionCosts.Add(BuildingTypes.Tree, 100);
        constructionCosts.Add(BuildingTypes.Hedge, 80);
        constructionCosts.Add(BuildingTypes.Grass, 30);
        constructionCosts.Add(BuildingTypes.IntersectionRoad, 100);
        constructionCosts.Add(BuildingTypes.StraightRoad, 100);
        constructionCosts.Add(BuildingTypes.TurnRoad, 100);

    }

    void LateUpdate()
    {
        if (!levelLoaded)
        {
            levelLoaded = true;
            LoadLevel();
        }
    }

    void GenerateMap()
    {
        /*
            generate cells from 0, 0 to mapSize-1, mapSize-1
        */

        for (int x = 0; x < mapSize; x++) // for each x value
        {
            List<Cell> xList = new List<Cell>(); // create a list of y values
            for (int y = 0; y < mapSize; y++)
            {
                xList.Add(new Cell(x, y)); // add a cell for each y value
            }
            map.Add(xList); // add this list to the main map
        }

        mapBuilder.Build(map);
        SaveLevel(); // save new map
    }

    void SaveLevel()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        MapSaveFile data = new MapSaveFile(); // create a new data object to write to
        data.map = map; // export current map

        bf.Serialize(file, data);
        file.Close();
    }

    void LoadLevel()
    {
        // load previous game save if it exists

        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            MapSaveFile data = (MapSaveFile)bf.Deserialize(file);
            file.Close();

            map = data.map;
            mapBuilder.Build(map);
        }
        else
        {
            // start to generate a map as there is no previous save
            GenerateMap();
            npcManager.PopUp(npcManager.allNPCS.Find(npc => npc.npcName == "Mr. Blingman"), taskManager.taskPresets.Find(task=>task.taskName == "Build a house"));
        }
    }

    public GameBuilding PlaceBuilding(int x, int y, GameBuilding building)
    {
        if (gameManager.costOfAction > gameManager.balance)
        {
            audioManager.PlaySoundEffect("InsufficientFunds");
            // play insufficient funds sound effect

            return building;
        }

        int occupants = 0;
        switch (building.buildingType)
        {
            case BuildingTypes.House:
                occupants += Random.Range(1, 4);
                break;
            case BuildingTypes.Flats:
                occupants += Random.Range(15, 75);
                break;
        }
        

        building.x = x;
        building.y = y;
        map[x][y].building = building;
        map[x][y].occupants = occupants;
        map[x][y].building.rotation = gameManager.desiredRotation;
        map[x][y].occupied = true;

        mapBuilder.NewBuilding(x, y, building);
        taskManager.NewBuilding(building.buildingType);
        statsManager.BuildingChange();
        gameManager.ChangeBalance(-constructionCosts[building.buildingType]);
        SaveLevel();

        return building;
    }

    public void ShowBuildingPreview(int x, int y, GameBuilding building)
    {
        if (map[x][y].occupied)
        {
            mapBuilder.ClearPreview();
            return;
        }

        gameManager.costOfAction = constructionCosts[building.buildingType];
        mapBuilder.PreviewBuilding(x, y, building);
    }

    public GameBuilding ClearCell(int x, int y, GameObject buildingObj)
    {
        GameBuilding building = map[x][y].building;
        map[x][y].building = null;
        map[x][y].occupied = false;

        mapBuilder.DestroyBuilding(buildingObj);
        statsManager.BuildingChange();
        SaveLevel();
        return building;
    }

}

[System.Serializable]
class MapSaveFile
{
    public List<List<Cell>> map = new List<List<Cell>>();
}