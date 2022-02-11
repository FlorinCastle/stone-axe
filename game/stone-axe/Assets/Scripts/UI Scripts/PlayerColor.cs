using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColor : MonoBehaviour
{
    [SerializeField] private Image _colorPreview;
    [SerializeField] private Color _colorRef;
    private int colorIndexRef;

    private MaterialPropertyBlock _propBlock;
    public void setupColor(Color32 input, int index)
    {
        _colorRef = input;
        _colorPreview.color = _colorRef;
        colorIndexRef = index;
    }

    public void selectColor()
    {
        GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIControl>().updatePlayerColor(colorIndexRef, _colorRef);
        GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIControl>().checkInputData();
    }
    public void setButtonInteractable() { this.GetComponentInChildren<Button>().interactable = true; }
    public void setButtonNotInteractable() { this.GetComponentInChildren<Button>().interactable = false; }
    public int ColorIndexRef { get => colorIndexRef; }
}
