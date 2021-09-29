using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GenerateItem : MonoBehaviour
{
    [SerializeField] private Item itemScript;
    [SerializeField] private Enchant enchantScript;
    [SerializeField] private InventoryScript _inventoryRef;
    [SerializeField] private GameObject _inventoryControl;
    [SerializeField] private GameObject _gameMaster;
    [Header("UI")]
    [SerializeField] private Text itemText;
    [SerializeField] private Button buyButton;

    //[SerializeField] InventoryScriptableObject inventoryStorage;

    private void Awake()
    {
        _gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
    }

    [SerializeField] private ItemData _generatedItem;
    [SerializeField] private EnchantData _generatedEnchant;
    public void GenerateRandomItem()
    {
        _generatedItem = itemScript.chooseItem();
        int ranEnchChance = Random.Range(0, 1000);
        if (ranEnchChance >= 700)
        {
            _generatedEnchant = enchantScript.chooseEnchant();
            _generatedItem.setIsEnchanted(true);
        }
        else
            _generatedItem.setIsEnchanted(false);

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
