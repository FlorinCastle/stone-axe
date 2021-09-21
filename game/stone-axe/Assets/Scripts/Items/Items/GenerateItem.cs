using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GenerateItem : MonoBehaviour
{
    [SerializeField] Item itemScript;
    [SerializeField] InventoryScript _inventoryRef;
    [SerializeField] GameObject _inventoryControl;
    [SerializeField] GameObject _gameMaster;
    [Header("UI")]
    [SerializeField] Text itemText;
    [SerializeField] Button buyButton;

    //[SerializeField] InventoryScriptableObject inventoryStorage;

    private void Awake()
    {
        _gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
    }

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

    public void forceInsertItem()
    {
        if (_generatedItem != null)
        {
            _inventoryRef.InsertItem(_generatedItem);
        }
        _generatedItem = null;
        buyButton.interactable = false;
        itemText.text = "item text";
    }

    public void forceDisassembleItem()
    {
        if (_generatedItem != null)
        {
            int index = _inventoryRef.InsertItem(_generatedItem);
            //Debug.Log("inserted item to disassemble at index: " + index);
            _inventoryRef.setSelectedItem(index);
            _gameMaster.GetComponent<DisassembleItemControl>().selectItem();
            _gameMaster.GetComponent<DisassembleItemControl>().disassembleItem();
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
