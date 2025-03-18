using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _outline;
    [SerializeField] private Color _correct;
    [SerializeField] private Color _incorrect;

    public void SetOrderLayer(int value) => _spriteRenderer.sortingOrder = value;
    public void SetNormal()
    {
        _spriteRenderer.color = Color.white;
        _outline.SetActive(false);
    } 
    public void IsPositionCorrectly(bool isCorrectly)
    {
        _outline.SetActive(true);

        if(isCorrectly)
            _spriteRenderer.color = _correct;
        else
            _spriteRenderer.color = _incorrect;
    }
}
