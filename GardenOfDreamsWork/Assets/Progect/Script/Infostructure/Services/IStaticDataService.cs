public interface IStaticDataService : IService
{
    public Building ForBuilding(string BuildingId);
    public UIBuildingManipulator GetUIBuilding();
    public LevelVisual GetLevelVisual();
    public void LoadBuildings();
}
