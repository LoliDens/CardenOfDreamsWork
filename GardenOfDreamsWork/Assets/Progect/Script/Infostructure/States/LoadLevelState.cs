using UnityEngine;

public class LoadLevelState : IPayloadState<BuildingGridData>
{
    private const string NAME_SCENE = "Main";

    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IPlayerInputService _playerInputService;
    private readonly ISaveLoadBuildingService _saveLoadBuildingService;

    private BuildingGridData _buildingGridData;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        LoadingCurtain loadingCurtain,IPlayerInputService playerInputService,ISaveLoadBuildingService saveLoadBuildingService)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _loadingCurtain = loadingCurtain;
        _playerInputService = playerInputService;
        _saveLoadBuildingService = saveLoadBuildingService;
    }

    public void Enter(BuildingGridData data)
    {
        _buildingGridData = data;
        _loadingCurtain.Show();
        _sceneLoader.Load(NAME_SCENE, OnLoaded);
    }

    public void Exit() { }


    private void OnLoaded()
    {
        BuildingGrid buildingGrid = GameObject.FindAnyObjectByType<BuildingGrid>();
        buildingGrid.Constrict(_playerInputService, _saveLoadBuildingService, _buildingGridData); 
        _loadingCurtain.Hide();
    }
}