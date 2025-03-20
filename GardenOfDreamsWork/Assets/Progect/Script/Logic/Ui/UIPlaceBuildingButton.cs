using System;
using UnityEngine;

public class UIPlaceBuildingButton : MonoBehaviour
{
    public Action<Building> SelectBuilding;

    [SerializeField] private Building _building;

    public void InvokeSelectBuilding() =>
        SelectBuilding?.Invoke(_building);
}
