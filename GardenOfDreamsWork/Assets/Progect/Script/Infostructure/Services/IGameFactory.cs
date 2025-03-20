public interface IGameFactory : IService
{
    Building CreateBuilding(BuildingInfo buildingInfo, Building[,] grid);
    UIBuildingManipulator CreateUI();
    LevelVisual CreteLevelVisual();
}