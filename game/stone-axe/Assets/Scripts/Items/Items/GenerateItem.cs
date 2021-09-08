using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GenerateItem : MonoBehaviour
{
    [SerializeField] Text itemText;
    [SerializeField] Item itemScript;
    [SerializeField] InventoryScript _inventoryRef;
    [SerializeField] GameObject _inventoryControl;

    //[SerializeField] InventoryScriptableObject inventoryStorage;


    [SerializeField] private ItemData _generatedItem;
    public void GenerateRandomItem()
    {
        _generatedItem = itemScript.chooseItem();

        generateItemText();
        itemText.text = _generatedText;
        _inventoryRef.InsertItem(_generatedItem);
    }

    private string _generatedText;
    private void generateItemText()
    {
        _generatedText = itemScript.silence();
    }
}
