using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelVisual : MonoBehaviour
{
    [SerializeField] private Tilemap _mainTilemap;
    [SerializeField] private Tilemap _gridTilemap;
    [SerializeField] private GameObject _startPointTilemap;

    public Transform GetStartPoint() => 
        _startPointTilemap.transform;

    public Tilemap GetMainTilemap() =>
        _mainTilemap;

    public void SetVisualTilemap(bool isVisible)=>
        _gridTilemap.gameObject.SetActive(isVisible);
}
