using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GenerateItem : MonoBehaviour
{
    [SerializeField] Item itemScript;
    [SerializeField] InventoryScript _inventoryRef;
    [SerializeField] GameObject _inventoryControl;
    [Header("UI")]
    [SerializeField] Text itemText;
    [SerializeField] Button buyButton;

    //[SerializeField] InventoryScriptableObject inventoryStorage;


    [SerializeField] private ItemData _generatedItem;
    public void GenerateRandomItem()
    {
        _generatedItem = itemScript.chooseItem();
        generateItemText();
        itemText.text = _generatedText;
        buyButton.interactable = true;
    }

    public void buyGeneratedItem()
    {
        if (_generatedItem != null)
        {
            if (this.gameObject.GetComponent<GameMaster>().removeCurrency(_generatedItem.TotalValue))
            {
                _inventoryRef.InsertItem(_generatedItem);
            }
        }
        _generatedItem = null;
        buyButton.interactable = false;
        itemText.text = "item text";
    }

    private string _generatedText;
    private void generateItemText()
    {
        _generatedText = itemScript.silence();
    }
}
