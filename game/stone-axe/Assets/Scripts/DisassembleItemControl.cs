using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisassembleItemControl : MonoBehaviour
{
    [SerializeField] private InventoryScript _invScriptRef;
    [SerializeField] private UIControl _uIControlRef;
    [SerializeField] private GameObject _selectedItem;
    [SerializeField] private DIS_DisassembleChance disassembleSkill;
    [SerializeField] private CFT_ReduceMaterialCost reducedMatSkill;
    [SerializeField] private DIS_EnchantRemoval enchantRemovalSkill;
    [Header("UI")]
    [SerializeField] private Text _itemText;
    [SerializeField] private Text _itemNameText;
    [SerializeField] private Button _disassembleButton;

    private void Awake()
    {
        if (_invScriptRef == null)
            _invScriptRef = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();

        if (_uIControlRef == null)
            _uIControlRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIControl>();
    }

    public void chooseItem()
    {
        _invScriptRef.setupItemInventory(true,2);
    }

    public void selectItem()
    {
        _selectedItem = _invScriptRef.getSelectedItem();
        if (_selectedItem != null)
        {
            setupTexts();
            _disassembleButton.interactable = true;
        }
        else
        {
            Debug.Log("No Item selected!");
            _disassembleButton.interactable = false;
        }
    }

    private string header = "parts gained\n";
    private string part1;
    private string part2;
    private string part3;
    private string ench;
    private void setupTexts()
    {
        ItemDataStorage itemData = _selectedItem.GetComponent<ItemDataStorage>();
        _itemNameText.text = "item chosen: " + itemData.ItemName;

        part1 = itemData.Part1.MaterialName + " " + itemData.Part1.PartName + "\n";
        part2 = itemData.Part2.MaterialName + " " + itemData.Part2.PartName + "\n";
        part3 = itemData.Part3.MaterialName + " " + itemData.Part3.PartName;
        if (itemData.IsEnchanted)
        {
            ench = "\n" + itemData.Enchantment.EnchantName + " +" + itemData.Enchantment.AmountOfBuff;
        }
        else
            ench = "";

        _itemText.text = header + part1 + part2 + part3 + ench;

    }

    
    public void disassembleItem()
    {
        int ran = Random.Range(0, 100);
        
        if (ran >= Mathf.RoundToInt(disassembleSkill.getFullDisassembleChance()))
        {
            // move part1 transform parent to inventory
            GameObject part1 = _selectedItem.GetComponent<ItemDataStorage>().Part1.gameObject;
            _invScriptRef.InsertPartData(part1);
            part1.transform.parent = _invScriptRef.gameObject.transform;

            // move part2 
            GameObject part2 = _selectedItem.GetComponent<ItemDataStorage>().Part2.gameObject;
            _invScriptRef.InsertPartData(part2);
            part2.transform.parent = _invScriptRef.gameObject.transform;

            // move part 3
            GameObject part3 = _selectedItem.GetComponent<ItemDataStorage>().Part3.gameObject;
            _invScriptRef.InsertPartData(part3);
            part3.transform.parent = _invScriptRef.gameObject.transform;

            // move enchantment, if enchanted
            if (_selectedItem.GetComponent<ItemDataStorage>().IsEnchanted)
            {
                int ranEnc = Random.Range(0, 100);
                //Debug.Log("ranEnc: " + ranEnc.ToString());

                if (ranEnc >= Mathf.RoundToInt(enchantRemovalSkill.getEnchantRemovalChance()))
                {
                    int chosenEnchantedPart = Random.Range(0, 3);

                        GameObject enc = _selectedItem.GetComponent<ItemDataStorage>().Enchantment.gameObject;
                    if (chosenEnchantedPart == 0) // part 1 selected
                    {
                        part1.GetComponent<PartDataStorage>().setEnchantment(enc.GetComponent<EnchantDataStorage>());
                        part1.GetComponent<PartDataStorage>().setIsHoldingEnchanted(true);
                        part2.GetComponent<PartDataStorage>().setIsHoldingEnchanted(false);
                        part3.GetComponent<PartDataStorage>().setIsHoldingEnchanted(false);
                        enc.transform.parent = part1.transform;
                    }
                    else if (chosenEnchantedPart == 1)  // part 2 selected
                    {
                        part2.GetComponent<PartDataStorage>().setEnchantment(enc.GetComponent<EnchantDataStorage>());
                        part1.GetComponent<PartDataStorage>().setIsHoldingEnchanted(false);
                        part2.GetComponent<PartDataStorage>().setIsHoldingEnchanted(true);
                        part3.GetComponent<PartDataStorage>().setIsHoldingEnchanted(false);
                        enc.transform.parent = part2.transform;
                    }
                    else if (chosenEnchantedPart == 2)  // part 3 selected
                    {
                        part3.GetComponent<PartDataStorage>().setEnchantment(enc.GetComponent<EnchantDataStorage>());
                        part1.GetComponent<PartDataStorage>().setIsHoldingEnchanted(false);
                        part2.GetComponent<PartDataStorage>().setIsHoldingEnchanted(false);
                        part3.GetComponent<PartDataStorage>().setIsHoldingEnchanted(true);
                        enc.transform.parent = part3.transform;
                    }

                }
                else if (ranEnc < Mathf.RoundToInt(enchantRemovalSkill.getEnchantRemovalChance()))
                {
                    //Debug.Log("adding enchant to inventory");
                    GameObject enc = _selectedItem.GetComponent<ItemDataStorage>().Enchantment.gameObject;

                    _invScriptRef.InsertEnchatment(enc);
                    enc.transform.parent = _invScriptRef.gameObject.transform;
                }
                // _invScriptRef.InsertEnchatment(enc);
                //enc.transform.parent = _invScriptRef.gameObject.transform;
            }
        }
        else if (ran < Mathf.RoundToInt(disassembleSkill.getFullDisassembleChance()))
        {
            //Debug.LogWarning("TODO: re-setup code so item can be fully disassembled");
            // get reference to part 1, get material composition and add to appropriate material ref
            GameObject part1 = _selectedItem.GetComponent<ItemDataStorage>().Part1.gameObject;
            part1.GetComponent<PartDataStorage>().Material.AddMat(Mathf.RoundToInt(part1.GetComponent<PartDataStorage>().RecipeData.UnitsOfMaterialNeeded * reducedMatSkill.getModifiedMatAmount()));

            // part 2
            GameObject part2 = _selectedItem.GetComponent<ItemDataStorage>().Part2.gameObject;
            part2.GetComponent<PartDataStorage>().Material.AddMat(Mathf.RoundToInt(part2.GetComponent<PartDataStorage>().RecipeData.UnitsOfMaterialNeeded * reducedMatSkill.getModifiedMatAmount()));

            // part 3
            GameObject part3 = _selectedItem.GetComponent<ItemDataStorage>().Part3.gameObject;
            part3.GetComponent<PartDataStorage>().Material.AddMat(Mathf.RoundToInt(part3.GetComponent<PartDataStorage>().RecipeData.UnitsOfMaterialNeeded * reducedMatSkill.getModifiedMatAmount()));

            // move enchantment if enchanted
            if (_selectedItem.GetComponent<ItemDataStorage>().IsEnchanted)
            {
                GameObject enc = _selectedItem.GetComponent<ItemDataStorage>().Enchantment.gameObject;

                _invScriptRef.InsertEnchatment(enc);
                enc.transform.parent = _invScriptRef.gameObject.transform;
            }
        }
        // remove item from inventory
        _invScriptRef.RemoveItem(_selectedItem.GetComponent<ItemDataStorage>().InventoryIndex);

        this.gameObject.GetComponent<ExperienceManager>().addExperience(3);

        _selectedItem = null;

        _itemNameText.text = "item chosen: [item name]";
        _itemText.text = "placeholder text";

        _disassembleButton.interactable = false;
    }
}
