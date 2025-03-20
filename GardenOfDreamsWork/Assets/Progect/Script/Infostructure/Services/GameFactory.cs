using UnityEngine;

public class GameFactory : IGameFactory
{
    private readonly IStaticDataService _staticDataService;

    public GameFactory(IStaticDataService staticData)
    {
        _staticDataService = staticData;
    }

    public Building CreateBuilding(BuildingInfo buildingInfo, Building[,] grid)
    {
        var building = Object.Instantiate(_staticDataService.ForBuilding(buildingInfo.BuildingData.Id));
        building.SetToBuildingInfo(buildingInfo, grid);
        return building;
    }

    public UIBuildingManipulator CreateUI() =>
        Object.Instantiate(_staticDataService.GetUIBuilding());

    public LevelVisual CreteLevelVisual() =>
        Object.Instantiate(_staticDataService.GetLevelVisual());
}
