using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveLoadBuildingService : ISaveLoadBuildingService
{
    private const string FILE_NAME = "buildingData";
    private const string FORMAT_NAME = ".json";

    public void SaveData(List<Building> buildings, Vector2Int size)
    {
        BuildingInfo[] buildingsInfo = new BuildingInfo[buildings.Count];

        for (int i = 0; i < buildings.Count; i++)
        {
            buildingsInfo[i] = buildings[i].GetInfo();
        }

        string json = JsonUtility.ToJson(new BuildingGridData(size, buildingsInfo), true);
        File.WriteAllText(GetFilePath(), json);
    }

    public bool LoadData(out BuildingGridData data)
    {
        string path = GetFilePath();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<BuildingGridData>(json);
            return true;
        }
       
        data = null;
        return false;
    }

    private string GetFilePath() => Path.Combine(Application.persistentDataPath, FILE_NAME + FORMAT_NAME);
}
