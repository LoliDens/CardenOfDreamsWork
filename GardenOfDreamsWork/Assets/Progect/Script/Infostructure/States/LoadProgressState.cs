using UnityEngine;

public class LoadProgressState : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly ISaveLoadBuildingService _saveLoadBuilding;

    public LoadProgressState(GameStateMachine gameStateMachine, ISaveLoadBuildingService saveLoadBuilding)
    {
        _gameStateMachine = gameStateMachine;
        _saveLoadBuilding = saveLoadBuilding;
    }

    public void Enter()
    {
        var data = LoadProgressOrInitNew();
        _gameStateMachine.Enter<LoadLevelState, BuildingGridData>(data);
    }

    public void Exit() { }

    private BuildingGridData LoadProgressOrInitNew()
    {
        if (_saveLoadBuilding.Load(out BuildingGridData data))        
            return data;
      
        return CreateNewBuildingData();       
    }

    private BuildingGridData CreateNewBuildingData()
    {
        var size = new Vector2Int(6, 10);
        return new BuildingGridData(size);
    }
}
