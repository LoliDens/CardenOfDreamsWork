using UnityEngine;

public class AssetProvider : IAssetProvider
{
    public GameObject InstantiatePrefab(string path)
    {
        var playerPrefab = Resources.Load<GameObject>(path);
        return Object.Instantiate(playerPrefab);
    }

    public GameObject InstantiatePrefab(string path, Vector3 at)
    {
        var playerPrefab = Resources.Load<GameObject>(path);
        return Object.Instantiate(playerPrefab, at, Quaternion.identity);
    }
}