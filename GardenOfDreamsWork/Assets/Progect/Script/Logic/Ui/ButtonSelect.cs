using UnityEngine;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField] private GameObject _select;

    public void SetSelect(bool isSelect) =>
        _select.SetActive(isSelect);
}
