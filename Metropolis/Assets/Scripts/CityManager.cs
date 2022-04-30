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
    StatisticsManager statsManager;

    MapBuilder mapBuilder;
    public List<List<Cell>> map = new List<List<Cell>>();

    string saveDirectory;
    string savePath;

    [SerializeField][Range(0,1000)] int mapSize = 100;

    bool levelLoaded;

    void Start()
    {
        mapBuilder = GetComponent<MapBuilder>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        statsManager = GameObject.Find("StatisticsManager").GetComponent<StatisticsManager>();
        saveDirectory = Application.persistentDataPath + $"/saves/{gameManager.saveName}";
        savePath = saveDirectory + $"/map.dat";
        // load level
        levelLoaded = false;
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
        }
    }

    public GameBuilding PlaceBuilding(int x, int y, GameBuilding building)
    {
        building.x = x;
        building.y = y;
        map[x][y].building = building;
        map[x][y].building.rotation = gameManager.desiredRotation;
        map[x][y].occupied = true;

        mapBuilder.NewBuilding(x, y, building);
        statsManager.BuildingChange(building.buildingType, true);
        SaveLevel();

        return building;
    }

    public void ShowBuildingPreview(int x, int y, GameBuilding building)
    {
        if (map[x][y].occupied) return;
        mapBuilder.PreviewBuilding(x, y, building);
    }

    public GameBuilding ClearCell(int x, int y, GameObject buildingObj)
    {
        GameBuilding building = map[x][y].building;
        map[x][y].building = null;
        map[x][y].occupied = false;

        mapBuilder.DestroyBuilding(buildingObj);
        statsManager.BuildingChange(building.buildingType, false);
        SaveLevel();
        return building;
    }

}

[System.Serializable]
class MapSaveFile
{
    public List<List<Cell>> map = new List<List<Cell>>();
}