using UnityEngine;

public interface IAssetProvider : IService
{
    GameObject InstantiatePrefab(string path);
    GameObject InstantiatePrefab(string path, Vector3 at);
}
