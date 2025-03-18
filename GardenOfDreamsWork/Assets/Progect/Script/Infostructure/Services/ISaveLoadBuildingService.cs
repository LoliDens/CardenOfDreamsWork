using System.Collections.Generic;
using UnityEngine;

public interface ISaveLoadBuildingService : IService
{
    bool Load(out BuildingGridData data);
    void Save(List<Building> buildings, Vector2Int size);
}