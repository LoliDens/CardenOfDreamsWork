using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingView View;

    [SerializeField] private BuildingInfo _info;
    public BuildingInfo GetInfo() => _info;

    public void SetToBuildingInfo(BuildingInfo info, Building[,] buildingsGrid)//Затычка
    {
        _info = info;
        transform.position = _info.CurrentPosition;
        View.SetOrderLayer(buildingsGrid.GetLength(1) - _info.PalacePosition.y);

        foreach (var item in _info.BuildingData.Size)
        {
            var buildElementToGrid = new Vector2Int(item.x + info.PalacePosition.x, item.y + info.PalacePosition.y);
            buildingsGrid[buildElementToGrid.x, buildElementToGrid.y] = this;
        }
    }

    public void PlaceOnGrid(Vector2Int mousePositionOnGrid, Building[,] buildingsGrid)
    {               
        _info.PalacePosition = mousePositionOnGrid;
        _info.CurrentPosition = transform.position;

        foreach (var item in _info.BuildingData.Size)
        {
            var buildElementToGrid = new Vector2Int(item.x + mousePositionOnGrid.x, item.y + mousePositionOnGrid.y);
            buildingsGrid[buildElementToGrid.x, buildElementToGrid.y] = this;
        }

        View.SetNormal();
    }

    public void DeletedBuilding(Building[,] grid)
    {
        foreach (var item in _info.BuildingData.Size)
        {
            var buildElementToGrid = new Vector2Int(item.x + _info.PalacePosition.x, item.y + _info.PalacePosition.y);
            grid[buildElementToGrid.x, buildElementToGrid.y] = null;
        }
    }

    public bool CanBePlace(Vector3 mousePositionOnTilemap, Vector2Int mousePositionOnGrid,
    Vector3 startPositionGrid, Vector3 endPositionGrid, Building[,] buildingsGrid) => 
        CanBePlaceOnGrid(startPositionGrid, endPositionGrid, mousePositionOnTilemap) 
        && HaveFreeSpace(mousePositionOnGrid, buildingsGrid);

    private bool HaveFreeSpace(Vector2Int mouseToGridPosition,Building[,] buildingGrid)
    {
        foreach (var sizeElement in _info.BuildingData.Size)
        {
            var buildElementToGrid = new Vector2Int (sizeElement.x + mouseToGridPosition.x,
                sizeElement.y + mouseToGridPosition.y);
            if (buildingGrid[buildElementToGrid.x, buildElementToGrid.y] != null) return false;
        }

        return true;
    }

    private bool CanBePlaceOnGrid(Vector3 startPosition, Vector3 endPosition,Vector3 mousePosition)
    {
        var canBe = true;

        foreach (var sizeElement in _info.BuildingData.Size)
        {
            var OnX = CheckLimit(mousePosition.x + sizeElement.x, startPosition.x, endPosition.x);
            var OnY = CheckLimit(mousePosition.y + sizeElement.y, startPosition.y, endPosition.y);

            canBe = canBe && OnY && OnX;
        }

        return canBe;
    }

    private bool CheckLimit(float current, float min, float max) =>
        current >= min && current < max;
}
