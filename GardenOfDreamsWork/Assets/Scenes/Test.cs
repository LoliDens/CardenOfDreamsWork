using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Test : MonoBehaviour
{
    public Dictionary<string, Building> _buildings = new();

    private void Awake()
    {
        Addressables.LoadAssetsAsync<GameObject>("Building", OnBuildingLoaded).Completed += AllLoaded;
    }

    public void AllLoaded(AsyncOperationHandle<IList<GameObject>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("All monsters loaded!" + $"{_buildings.Count}");
        }
        else
        {
            Debug.LogError("Failed to load monsters.");
        }
    }

    public void OnBuildingLoaded(GameObject prefab)
    {
        var building = prefab.GetComponent<Building>();

        if (building != null)
        {
            _buildings[building.GetInfo().BuildingData.Id] = building;
        }
    }
}
