using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    [SerializeField] private List<Button> _buttonGroup;
    [SerializeField] private Button _selectedButton;

    public void ButtonSelected(Button value)
    {
        foreach (Button but in _buttonGroup)
            but.interactable = true;

        _selectedButton = value;
        if (_selectedButton != null)
            _selectedButton.interactable = false;
    }

    // TODO: set up code so buttons are selected when swapping ui
    /*
     * 
     */
}
