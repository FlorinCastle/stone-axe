using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisassembleItemControl : MonoBehaviour
{
    [SerializeField] private InventoryScript _invScriptRef;
    private InventoryData _invDataRef;
    [SerializeField] private Quest _questRef;
    [SerializeField] private QuestControl _questControl;
    [SerializeField] private UIControl _uIControlRef;
    [SerializeField] private GameObject _selectedObject;
    [SerializeField] private DIS_DisassembleChance disassembleSkill;
    [SerializeField] private CFT_ReduceMaterialCost reducedMatSkill;
    [SerializeField] private DIS_EnchantRemoval enchantRemovalSkill;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _itemText;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private Button _disassembleButton;

    private void Awake()
    {
        if (_invScriptRef == null)
            _invScriptRef = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();
        if (_uIControlRef == null)
            _uIControlRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIControl>();
        if (_invDataRef == null)
            _invDataRef = _invScriptRef.gameObject.GetComponent<InventoryData>();
        if (_questRef == null)
            _questRef = gameObject.GetComponent<Quest>();
        if (_questControl == null)
            _questControl = gameObject.GetComponent<QuestControl>();
        _disassembleButton.interactable = false;
    }

    public void chooseItem()
    {
        _invScriptRef.setupItemInventory();
    }

    public void selectItem()
    {
        _selectedObject = _invScriptRef.getSelectedItem();
        selectItem(_selectedObject);
    }

    public void selectPart()
    {
        _selectedObject = _invScriptRef.getSelectedPart();
        selectPart(_selectedObject);
    }

    public void selectItem(GameObject item)
    {
        _selectedObject = item;
        if (_selectedObject != null)
        {
            setupTexts();
            _disassembleButton.interactable = true;
        }
        else
        {
            Debug.LogError("No Item selected!");
            _disassembleButton.interactable = false;
        }
    }

    public void selectPart(GameObject part)
    {
        _selectedObject = part;
        if (_selectedObject != null)
        {
            setupTexts();
            _disassembleButton.interactable = true;
        }
        else
        {
            Debug.LogError("No Part Selected!");
            _disassembleButton.interactable = false;
        }
    }

    private string header;
    private string part1;
    private string part2;
    private string part3;
    private string body;
    private string ench;
    private void setupTexts()
    {

        if (_selectedObject.GetComponent<ItemDataStorage>() != null)
        {
            ItemDataStorage itemData = _selectedObject.GetComponent<ItemDataStorage>();
            header = "parts gained\n";
            _itemNameText.text = "item chosen: " + itemData.ItemName;

            part1 = itemData.Part1.MaterialName + " " + itemData.Part1.PartName + "\n";
            part2 = itemData.Part2.MaterialName + " " + itemData.Part2.PartName + "\n";
            part3 = itemData.Part3.MaterialName + " " + itemData.Part3.PartName + "\n";
            if (itemData.IsEnchanted)
            {
                ench = "\n" + itemData.Enchantment.EnchantName + " +" + itemData.Enchantment.AmountOfBuff;
            }
            else
                ench = "";

            _itemText.text = header + part1 + part2 + part3 + ench;
        }
        else if (_selectedObject.GetComponent<PartDataStorage>() != null)
        {
            PartDataStorage partData = _selectedObject.GetComponent<PartDataStorage>();
            header = "materials ";
            _itemNameText.text = "part chosen: " + partData.PartName;

            body = partData.MaterialName + " " + partData.RecipeData.UnitsOfMaterialNeeded + "\n";
            if (partData.IsHoldingEnchant)
            {
                ench = "\n" + partData.Enchantment.EnchantName + " +" + partData.Enchantment.AmountOfBuff;
                header += "and enchant ";
            }
            else
                ench = "";

            header += "gained\n";

            _itemText.text = header + body + ench;
        }
    }
    
    public void disassebleObject()
    {
        if (_selectedObject.GetComponent<ItemDataStorage>() != null)
            disassembleItem();
        else if (_selectedObject.GetComponent<PartDataStorage>() != null)
            disassemblePart();
    }

    public void disassembleItem()
    {
        gameObject.GetComponent<MiniGameControl>().resetHitPoints();
        int ran = Random.Range(0, 100);
        
        if (ran >= Mathf.RoundToInt(disassembleSkill.getFullDisassembleChance()))
        {
            // move part1 transform parent to inventory
            GameObject part1 = _selectedObject.GetComponent<ItemDataStorage>().Part1.gameObject;
            _invScriptRef.InsertPart(part1);
            part1.transform.parent = _invScriptRef.gameObject.transform;

            // move part2 
            GameObject part2 = _selectedObject.GetComponent<ItemDataStorage>().Part2.gameObject;
            _invScriptRef.InsertPart(part2);
            part2.transform.parent = _invScriptRef.gameObject.transform;

            // move part 3
            GameObject part3 = _selectedObject.GetComponent<ItemDataStorage>().Part3.gameObject;
            _invScriptRef.InsertPart(part3);
            part3.transform.parent = _invScriptRef.gameObject.transform;

            // move enchantment, if enchanted
            if (_selectedObject.GetComponent<ItemDataStorage>().IsEnchanted)
            {
                int ranEnc = Random.Range(0, 100);
                //Debug.Log("ranEnc: " + ranEnc.ToString());

                if (ranEnc >= Mathf.RoundToInt(enchantRemovalSkill.getEnchantRemovalChance()))
                {
                    int chosenEnchantedPart = Random.Range(0, 3);

                        GameObject enc = _selectedObject.GetComponent<ItemDataStorage>().Enchantment.gameObject;
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
                    GameObject enc = _selectedObject.GetComponent<ItemDataStorage>().Enchantment.gameObject;

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
            GameObject part1 = _selectedObject.GetComponent<ItemDataStorage>().Part1.gameObject;
            string ph1 = part1.GetComponent<PartDataStorage>().Material.Material;
            _invDataRef.getMaterial(ph1).AddMat(Mathf.RoundToInt(part1.GetComponent<PartDataStorage>().RecipeData.UnitsOfMaterialNeeded * reducedMatSkill.getModifiedMatAmount()));

            // part 2
            GameObject part2 = _selectedObject.GetComponent<ItemDataStorage>().Part2.gameObject;
            string ph2 = part2.GetComponent<PartDataStorage>().Material.Material;
            _invDataRef.getMaterial(ph2).AddMat(Mathf.RoundToInt(part2.GetComponent<PartDataStorage>().RecipeData.UnitsOfMaterialNeeded * reducedMatSkill.getModifiedMatAmount()));

            // part 3
            GameObject part3 = _selectedObject.GetComponent<ItemDataStorage>().Part3.gameObject;
            string ph3 = part3.GetComponent<PartDataStorage>().Material.Material;
            _invDataRef.getMaterial(ph3).AddMat(Mathf.RoundToInt(part3.GetComponent<PartDataStorage>().RecipeData.UnitsOfMaterialNeeded * reducedMatSkill.getModifiedMatAmount()));

            // move enchantment if enchanted
            if (_selectedObject.GetComponent<ItemDataStorage>().IsEnchanted)
            {
                GameObject enc = _selectedObject.GetComponent<ItemDataStorage>().Enchantment.gameObject;

                _invScriptRef.InsertEnchatment(enc);
                enc.transform.parent = _invScriptRef.gameObject.transform;
            }
        }
        // remove item from inventory
        //_invScriptRef.RemoveItem(_selectedItem.GetComponent<ItemDataStorage>().InventoryIndex);
        _invScriptRef.RemoveItem(_selectedObject);

        gameObject.GetComponent<ExperienceManager>().addExperience(3);

        clearDisassembleMenu();
        gameObject.GetComponent<SellItemControl>().clearSellMenu();
        gameObject.GetComponent<SellItemControl>().clearSellMenu();

        if (_questControl.QuestChosen() && // WHY TF ARE YOU THROWING A NULL REF?!? IF YOU HIT A NULL, YOU SHOULD THROW FALSE, DO F-ALL AND MOVE ON
            (_questRef.QuestType(_questControl.CurrentQuest) == "Tutorial" ||
            _questRef.QuestType(_questControl.CurrentQuest) == "Story"))
        {
            //Debug.Log("TODO re-implement this");
            if (gameObject.GetComponent<QuestControl>().CurrentStage.questStageType == "Disassemble_Item")
            {
                Debug.LogWarning("Quest Notif - Disassemble Minigame Done");
                gameObject.GetComponent<QuestControl>().nextStage();

            }
        }
        else {/* do fuck all */}

        gameObject.GetComponent<MiniGameControl>().stopDisassembleMiniGame();
    }
    public void disassemblePart()
    {
        gameObject.GetComponent<MiniGameControl>().resetHitPoints();

        // get reference to material and add to appropriate material ref
        string ph1 = _selectedObject.GetComponent<PartDataStorage>().Material.Material;
        _invDataRef.getMaterial(ph1).AddMat(Mathf.RoundToInt(_selectedObject.GetComponent<PartDataStorage>().RecipeData.UnitsOfMaterialNeeded * reducedMatSkill.getModifiedMatAmount()));
        // move enchantment if enchanted
        if (_selectedObject.GetComponent<PartDataStorage>().IsHoldingEnchant)
        {
            GameObject enc = _selectedObject.GetComponent<PartDataStorage>().Enchantment.gameObject;

            _invScriptRef.InsertEnchatment(enc);
            enc.transform.parent = _invScriptRef.gameObject.transform;
        }

        // remove part from inventory
        _invScriptRef.RemovePart(_selectedObject, true);
        _invScriptRef.setupMatInventory();

        gameObject.GetComponent<ExperienceManager>().addExperience(1);

        clearDisassembleMenu();
        gameObject.GetComponent<SellItemControl>().clearSellMenu();
        gameObject.GetComponent<MiniGameControl>().stopDisassembleMiniGame();
    }

    public void clearDisassembleMenu()
    {
        _selectedObject = null;

        _itemNameText.text = "item chosen: none";
        _itemText.text = "choose item";

        _disassembleButton.interactable = false;
    }
}
