using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BuildingGrid : MonoBehaviour
{
    private const string NAME_LAYER_BULDING = "Building";

    private ISaveLoadBuildingService _saveLoadBuilding;
    private IPlayerInputService _playerInputService;
    private Vector2Int _gridSize;

    [SerializeField] private Transform _startCreatePoint;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private GameObject _gridVisual;

    [SerializeField] private List<Building> _buildingsVariant;//затычка 

    private Building[,] _grid;
    private List<Building> _allBuildings;
    private Building _selectBuilding;
    private Building _flyingBuilding;
    private Vector3 _mousePositionToTilemap;

    public void SelectBuilding(Building building) =>
        _selectBuilding = building;

    private Vector2Int GetVectorTwoMousePos() => Vector2Int.RoundToInt(
            (Vector2)(_mousePositionToTilemap - _startCreatePoint.position));

    public void Constrict(IPlayerInputService playerInput,ISaveLoadBuildingService saveLoadBuilding,BuildingGridData data)
    {
        _playerInputService = playerInput;
        _saveLoadBuilding = saveLoadBuilding;
        _gridSize = data.GridSize;
        _grid = new Building[_gridSize.x, _gridSize.y];
        _allBuildings = new();

        if (data.AllBuildings != null)//Затычка
        {
            foreach (var buildingInfo in data.AllBuildings)
            {
                foreach (var variant in _buildingsVariant)
                {
                    if (variant.GetInfo().BuildingData.Id == buildingInfo.BuildingData.Id)
                    {
                        Building building = Instantiate(variant);
                        building.SetToBuildingInfo(buildingInfo,_grid);
                        _allBuildings.Add(building);
                    }
                }
            }
        }
    }

    public void StartPlaceBuilding()
    {
        if (_selectBuilding == null)
            return;

        if (_flyingBuilding != null)      
            Destroy(_flyingBuilding.gameObject);

        _playerInputService.RegisterActionMouseLeftClick(PlaceBuilding);
        _playerInputService.RegisterActionMouseMove(MovingBuilding);

        _flyingBuilding = Instantiate(_selectBuilding, _mousePositionToTilemap,Quaternion.identity);

        _gridVisual.SetActive(true);
    }

    public void StartDeletedBuilding()
    {
        _playerInputService.RegisterActionMouseLeftClick(DeletedBuilding);
        _gridVisual.SetActive(true);
    }

    private void DeletedBuilding()
    {
        var plane = new Plane(Vector3.forward, _startCreatePoint.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_playerInputService.MouseMove), Vector2.zero, LayerMask.GetMask(NAME_LAYER_BULDING));

        if (hit.collider != null)
        {
            Building building = hit.collider.GetComponent<Building>();
            _allBuildings.Remove(building);
            building.DeletedBuilding(_grid);

            Destroy(building.gameObject);

            _playerInputService.UnregisterActionMouseLeftClick(DeletedBuilding);
            _gridVisual.SetActive(false);
            _saveLoadBuilding.Save(_allBuildings, _gridSize);
        }
    }

    private void MovingBuilding(Vector2 mousePosition)
    {
        var plane = new Plane(Vector3.forward, _startCreatePoint.transform.position);
        var ray = Camera.main.ScreenPointToRay(mousePosition);

        if (plane.Raycast(ray, out float position) && !EventSystem.current.IsPointerOverGameObject())
        {
            var mouseToWorldPosition = ray.GetPoint(position);
            var cellPosition = _tilemap.WorldToCell(mouseToWorldPosition);
            _mousePositionToTilemap = _tilemap.GetCellCenterWorld(cellPosition);

            if (_flyingBuilding.transform.position != _mousePositionToTilemap)
            {                
                _flyingBuilding.View.SetOrderLayer(_gridSize.y - GetVectorTwoMousePos().y);
                _flyingBuilding.View.IsPositionCorrectly(CanPlaceFlyingBuilding());
            }

            _flyingBuilding.transform.position = _mousePositionToTilemap;
        }
    }

    private void PlaceBuilding()
    {
        var endPosition = new Vector3(_startCreatePoint.position.x + _gridSize.x, _startCreatePoint.position.y + _gridSize.y, 0);
        var mousePositionToGrid = GetVectorTwoMousePos(); 

        if (CanPlaceFlyingBuilding())
        {
            _flyingBuilding.PlaceOnGrid(mousePositionToGrid, _grid);
            _allBuildings.Add(_flyingBuilding);
            _flyingBuilding = null;
            _gridVisual.SetActive(false);

            _playerInputService.UnregisterActionMouseLeftClick(PlaceBuilding);
            _playerInputService.UnregisterActionMouseMove(MovingBuilding);

            _saveLoadBuilding.Save(_allBuildings, _gridSize);
        }        
    }

    private bool CanPlaceFlyingBuilding()
    {
        var endPosition = new Vector3(_startCreatePoint.position.x + _gridSize.x, _startCreatePoint.position.y + _gridSize.y, 0);
        var mousePositionToGrid = GetVectorTwoMousePos();

        return _flyingBuilding.CanBePlace(_mousePositionToTilemap, mousePositionToGrid,
                _startCreatePoint.position, endPosition, _grid);
    }
}
