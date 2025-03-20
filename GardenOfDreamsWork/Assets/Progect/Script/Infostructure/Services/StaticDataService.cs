using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StaticDataService : IStaticDataService
{
    private Dictionary<string, Building> _buildings = new();
    private UIBuildingManipulator _buildingManipulator;//Test
    private LevelVisual _levelVisual;//Test

    public void LoadBuildings()
    {
        Addressables.LoadAssetsAsync<GameObject>(AddressableConst.LABLE_TO_BUILDINGS, OnBuildingLoaded);
        Addressables.LoadAssetAsync<GameObject>(AddressableConst.UI_BUILDING_MONIPULATOR)
            .Completed += OnLoadUiBuildingManipulator;
        Addressables.LoadAssetAsync<GameObject>(AddressableConst.LEVEL_VISUAL)
            .Completed += OnLoadLevelVisual;
    }

    public UIBuildingManipulator GetUIBuilding() => //Test
        _buildingManipulator;

    public LevelVisual GetLevelVisual() => //Test
        _levelVisual;

    public Building ForBuilding(string BuildingId)
    {
       if(_buildings.TryGetValue(BuildingId,out Building building))
            return building;
        return null;
    }

    private void OnLoadUiBuildingManipulator(AsyncOperationHandle<GameObject> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
            _buildingManipulator = handle.Result.GetComponent<UIBuildingManipulator>();
    }  

    private void OnLoadLevelVisual(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
            _levelVisual = handle.Result.GetComponent<LevelVisual>();
    }


    private void OnBuildingLoaded(GameObject prefab)
    {
        var building = prefab.GetComponent<Building>();

        if (building != null)
        {
            _buildings.Add(building.GetInfo().BuildingData.Id, building);
        }
    }
}