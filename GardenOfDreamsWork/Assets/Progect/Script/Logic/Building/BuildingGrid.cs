using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BuildingGrid : MonoBehaviour
{
    private const string NAME_LAYER_BULDING = "Building";

    private readonly ISaveLoadBuildingService _saveLoadBuilding;
    private readonly IGameFactory _gameFactory;
    private readonly IPlayerInputService _playerInputService;

    private UIBuildingManipulator _manipulator;
    private LevelVisual _levelVisual;

    private Vector2Int _gridSize;
    private Building[,] _grid;
    private Transform _startPoint;
    private Tilemap _tilemap;
    private List<Building> _allBuildings = new();

    private Building _selectBuilding;
    private Building _flyingBuilding;
    private Vector3 _mousePositionToTilemap;

    public BuildingGrid(IPlayerInputService playerInput,ISaveLoadBuildingService saveLoadBuilding,
       IGameFactory gameFactory ,BuildingGridData data)
    {
        _playerInputService = playerInput;
        _saveLoadBuilding = saveLoadBuilding;
        _gameFactory = gameFactory;

        CreateLevelToData(data);
    }

    private void SelectBuilding(Building building) =>
       _selectBuilding = building;

    private Vector2Int GetVectorTwoMousePos() => Vector2Int.RoundToInt(
            (Vector2)(_mousePositionToTilemap - _startPoint.position));

    private void CreateLevelToData(BuildingGridData data)
    {
        _gridSize = data.GridSize;
        _grid = new Building[_gridSize.x, _gridSize.y];

        _manipulator = _gameFactory.CreateUI();
        _levelVisual = _gameFactory.CreteLevelVisual();

        _manipulator.StartPlaceBuilding += StartPlaceBuilding;
        _manipulator.StartDeletedBuilding += StartDeletedBuilding;
        _manipulator.SubscribeOnSelectBuilding(SelectBuilding);

        _startPoint = _levelVisual.GetStartPoint();
        _tilemap = _levelVisual.GetMainTilemap();

        foreach (var buildingInfo in data.AllBuildings)
        {
            var building = _gameFactory.CreateBuilding(buildingInfo, _grid);
            _allBuildings.Add(building);
        }
    }

    private void StartPlaceBuilding()
    {
        GameBootstrapper.Print(_selectBuilding);

        if (_selectBuilding == null)
            return;

        if (_flyingBuilding != null)      
            Destroy(_flyingBuilding.gameObject);

        _playerInputService.RegisterActionMouseLeftClick(PlaceBuilding);
        _playerInputService.RegisterActionMouseMove(MovingBuilding);

        _flyingBuilding = Instantiate(_selectBuilding, _mousePositionToTilemap,Quaternion.identity);

        _levelVisual.SetVisualTilemap(true);
    }

    private void StartDeletedBuilding()
    {
        _playerInputService.RegisterActionMouseLeftClick(DeletedBuilding);
        _levelVisual.SetVisualTilemap(true);
    }

    private void DeletedBuilding()
    {
        var plane = new Plane(Vector3.forward, _startPoint.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_playerInputService.MouseMove), Vector2.zero, LayerMask.GetMask(NAME_LAYER_BULDING));

        if (hit.collider != null)
        {
            Building building = hit.collider.GetComponent<Building>();
            _allBuildings.Remove(building);
            building.DeletedBuilding(_grid);

            Destroy(building.gameObject);

            _playerInputService.UnregisterActionMouseLeftClick(DeletedBuilding);
            _levelVisual.SetVisualTilemap(false);
            _saveLoadBuilding.SaveData(_allBuildings, _gridSize);
        }
    }

    private void MovingBuilding(Vector2 mousePosition)
    {
        var plane = new Plane(Vector3.forward, _startPoint.transform.position);
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
        var endPosition = new Vector3(_startPoint.position.x + _gridSize.x, _startPoint.position.y + _gridSize.y, 0);
        var mousePositionToGrid = GetVectorTwoMousePos(); 

        if (CanPlaceFlyingBuilding())
        {
            _flyingBuilding.PlaceOnGrid(mousePositionToGrid, _grid);
            _allBuildings.Add(_flyingBuilding);
            _flyingBuilding = null;
            _levelVisual.SetVisualTilemap(false);

            _playerInputService.UnregisterActionMouseLeftClick(PlaceBuilding);
            _playerInputService.UnregisterActionMouseMove(MovingBuilding);

            _saveLoadBuilding.SaveData(_allBuildings, _gridSize);
        }        
    }

    private bool CanPlaceFlyingBuilding()
    {
        var endPosition = new Vector3(_startPoint.position.x + _gridSize.x, _startPoint.position.y + _gridSize.y, 0);
        var mousePositionToGrid = GetVectorTwoMousePos();

        return _flyingBuilding.CanBePlace(_mousePositionToTilemap, mousePositionToGrid,
                _startPoint.position, endPosition, _grid);
    }
}
