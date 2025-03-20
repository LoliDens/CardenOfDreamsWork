using System.Collections.Generic;
using UnityEngine;

public interface ISaveLoadBuildingService : IService
{
    bool LoadData(out BuildingGridData data);
    void SaveData(List<Building> buildings, Vector2Int size);
}