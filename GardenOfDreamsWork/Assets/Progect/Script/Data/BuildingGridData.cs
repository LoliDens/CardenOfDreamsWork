using UnityEngine;
using System;

[Serializable]
public class BuildingGridData
{
    public Vector2Int GridSize;
    public BuildingInfo[] AllBuildings;

    public BuildingGridData(Vector2Int gridSize, BuildingInfo[] allBuildings = null)
    {
        GridSize = gridSize;
        AllBuildings = allBuildings;
    }
}