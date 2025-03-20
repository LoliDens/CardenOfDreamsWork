using System;
using UnityEngine;

public class UIBuildingManipulator : MonoBehaviour
{
    public Action StartPlaceBuilding;
    public Action StartDeletedBuilding;

    [SerializeField] private UIPlaceBuildingButton[] _buttons;
    
    public void PlaceBuilding() =>
        StartPlaceBuilding?.Invoke();

    public void DeletedBuilding() =>
        StartDeletedBuilding?.Invoke();

    public void SubscribeOnSelectBuilding(Action<Building> subscriber)
    {
        foreach (var button in _buttons) 
        {
            button.SelectBuilding += subscriber;
        }
    }

    public void UnsubscribeOnSelectBuilding(Action<Building> subscriber)
    {
        foreach (var button in _buttons)
        {
            button.SelectBuilding -= subscriber;
        }
    }
}
