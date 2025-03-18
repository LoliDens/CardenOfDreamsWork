using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelectController : MonoBehaviour
{
    [SerializeField] ButtonSelect[] _buttons;

    public void SetCurrentButtonSelect(ButtonSelect buttonSelect)
    {
        foreach (var button in _buttons)
            button.SetSelect(false);

        buttonSelect.SetSelect(true);
    }
}
