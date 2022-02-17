using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    //[SerializeField] private GameObject _inventoryButtonParent;
    [SerializeField]
    private GameObject _selectedItem;
    [SerializeField]
    private GameObject _selectedPart;
    [SerializeField]
    private MaterialData _selectedMat;
    [SerializeField]
    private GameObject _selectedEnchant;
    [SerializeField]
    private GameMaster _gameMaster;
    [SerializeField]
    private UIControl _UIControlRef;
    [SerializeField]
    private InventoryData _inventoryData;
    [SerializeField]
    private CraftControl _craftControlRef;

    private enum removingItemStatusEnum
    {
        NotRemoving,            // int 0
        RemovingToSell,         // int 1
        RemovingToDisassemble,  // int 2
        RemovingToCraft1,       // int 3
        RemovingToCraft2,       // int 4
        RemovingToCraft3,       // int 5
        RemovingToCraftMat,     // int 6
        RemovingToEnchant       // int 7
    }
    [Header("Data")]
    [SerializeField] private removingItemStatusEnum _removingStatus;

    // item inventory
    //[SerializeField] private List<GameObject> _itemInventoryData;
    [SerializeField] private List<GameObject> _itemButtonList;
    // part inventory
    //[SerializeField] private List<GameObject> _partInventoryData;
    [SerializeField] private List<GameObject> _partButtonList;
    // material inventory
    //[SerializeField] private List<MaterialData> materialInventory;
    [SerializeField] private List<GameObject> _materialButtonList;
    // enchant inventory
    //[SerializeField] private List<GameObject> _enchantInventoryData;
    [SerializeField] private List<GameObject> _enchantButtonList;

    [Header("UI References")]
    [SerializeField] private Button _headerButtonItems;
    [SerializeField] private Button _headerButtonParts;
    [SerializeField] private Button _headerButtonMaterials;
    [SerializeField] private Button _headerButtonEnchants;
    [SerializeField] private TextMeshProUGUI _descriptionText1;
    [SerializeField] private TextMeshProUGUI _descriptionText2;
    [SerializeField] private GameObject _itemsInvScroll;
    [SerializeField] private GameObject _itemsButtonParent;
    [SerializeField] private GameObject _partsInvScroll;
    [SerializeField] private GameObject _partsButtonParent;
    [SerializeField] private GameObject _matsInvScroll;
    [SerializeField] private GameObject _matsButtonParent;
    [SerializeField] private GameObject _enchInvScroll;
    [SerializeField] private GameObject _enchButtonParent;
    [SerializeField] private GameObject _selectItemButton;
    [SerializeField] private GameObject _selectPartButton;
    [SerializeField] private GameObject _selectMatButton;
    [SerializeField] private GameObject _selectEnchantButton;
    [Header("Prefabs")]
    [SerializeField] private GameObject _itemDataStoragePrefab;
    [SerializeField] private GameObject _partDataStoragePrefab;
    [SerializeField] private GameObject _enchantDataStoragePrefab;
    [SerializeField] private GameObject _itemInfoPrefab;
    [SerializeField] private GameObject _partInfoPrefab;
    [SerializeField] private GameObject _matInfoPrefab;
    [SerializeField] private GameObject _enchantInfoPrefab;

    private GameObject tempButtonList;

    private void Awake()
    {
        if (_gameMaster == null)
            _gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        if (_inventoryData == null)
            _inventoryData = this.gameObject.GetComponent<InventoryData>();

    }

    private void Start()
    {
        setupItemInventory();
        setupPartInventory();
        setupMatInventory();
        setupEnchantInventory();
    }
    public void setupItemInventory()
    {
        setupItemInventory(false, 0);
    }
    public void setupItemInventory(bool isRemoving, int state)
    {
        clearItemButtonList();
        _selectedItem = null;
        
        _selectItemButton.SetActive(isRemoving);
        _selectPartButton.SetActive(false);
        _selectMatButton.SetActive(false);
        _selectEnchantButton.SetActive(false);
        setStatus(state);
        setupHeader();

        int k = 0;
        foreach (GameObject item in _inventoryData.ItemInventory)
        {
            if (item != null)
            {
                // get reference to ItemDataStorage script
                ItemDataStorage itemData = item.GetComponent<ItemDataStorage>();

                // instatiate the button prefab
                tempButtonList = Instantiate(_itemInfoPrefab);
                tempButtonList.transform.SetParent(_itemsButtonParent.transform, false);
                // set up button text
                TextMeshProUGUI t = tempButtonList.GetComponentInChildren<TextMeshProUGUI>();
                t.text = itemData.ItemName;

                // add button to list
                InsertItemButton(tempButtonList, k);
            }
            k++;
        }
        _descriptionText1.text = "";
        _descriptionText2.text = "";
    }

    public void setupPartInventory()
    {
        setupPartInventory(false, 0);
    }
    public void setupPartInventory(bool isRemoving, int state)
    {
        clearPartButtonList();
        _selectedPart = null;
        
        _selectItemButton.SetActive(false);
        _selectPartButton.SetActive(isRemoving);
        _selectMatButton.SetActive(false);
        _selectEnchantButton.SetActive(false);
        setStatus(state);
        setupHeader();

        int k = 0;
        foreach (GameObject part in _inventoryData.PartInventory)
        {
            if (part != null)
            {
                // get reference to ItemDataStorage script
                PartDataStorage partData = part.GetComponent<PartDataStorage>();

                // instantiate the button prefab
                tempButtonList = Instantiate(_partInfoPrefab);
                tempButtonList.transform.SetParent(_partsButtonParent.transform, false);

                // set up button text
                TextMeshProUGUI t = tempButtonList.GetComponentInChildren<TextMeshProUGUI>();
                t.text = partData.PartName;
                
                // if removing part from inventory
                if (isRemoving == true)
                {
                    if (state == 3 || state == 4 || state == 5)
                    {
                        if (checkIfPartIsValid(state, partData))
                            tempButtonList.GetComponentInChildren<Button>().interactable = true;
                        else
                            tempButtonList.GetComponentInChildren<Button>().interactable = false;
                    }
                }

                // add button to list
                InsertPartButton(tempButtonList, k);
            }
            k++;
        }
        _descriptionText1.text = "new text";
    }

    public void setupMatInventory()
    {
        setupMatInventory(false, 0);
    }
    public void setupMatInventory(bool isRemoving, int state)
    {
        _selectedMat = null;

        _selectItemButton.SetActive(false);
        _selectPartButton.SetActive(false);
        _selectMatButton.SetActive(isRemoving);
        _selectEnchantButton.SetActive(false);
        setStatus(state);
        setupHeader();


        int m = 0;
        foreach (GameObject mat in _inventoryData.MaterialInventory)
        {
            if (mat.GetComponent<MaterialDataStorage>() != null)
            {
                MaterialDataStorage matStore = mat.GetComponent<MaterialDataStorage>();
                // if level req is met
                //Debug.LogWarning("InvScript - matStore.LevelRequirement of " + matStore.Material + ": " + matStore.LevelRequirement);
                //Debug.LogWarning("InvScript - _gameMaster.GetLevel: " + _gameMaster.GetLevel);
                if (matStore.LevelRequirement <= _gameMaster.GetLevel)
                {
                    // instantiate the button prefab
                    tempButtonList = Instantiate(_matInfoPrefab);
                    tempButtonList.transform.SetParent(_matsButtonParent.transform, false);

                    // set up the button text
                    tempButtonList.GetComponentInChildren<MaterialButton>().setMatInfoText(matStore);

                    // set material
                    tempButtonList.GetComponentInChildren<MaterialButton>().MaterialData = matStore.MatDataRef;

                    // if removing from inventory
                    if (isRemoving == true)
                    {
                        if (state == 6)
                        {
                            if (checkIfMatIsValid(state, matStore.MatDataRef))
                                tempButtonList.GetComponentInChildren<Button>().interactable = true;
                            else
                                tempButtonList.GetComponentInChildren<Button>().interactable = false;
                        }
                    }
                    // add button to list
                    InsertMatButton(tempButtonList, m);
                    m++;
                }
            }
            //m++;
        }
        _descriptionText1.text = "new text";

    }

    public void setupEnchantInventory()
    {
        setupEnchantInventory(false, 0);
    }
    public void setupEnchantInventory(bool isRemoving, int state)
    {
        _selectedEnchant = null;

        _selectItemButton.SetActive(false);
        _selectPartButton.SetActive(false);
        _selectMatButton.SetActive(false);
        _selectEnchantButton.SetActive(isRemoving);
        setStatus(state);

        setupHeader();


        int e = 0;
        foreach(GameObject ench in _inventoryData.EnchantInventory)
        {
            if (ench != null)
            {
                EnchantDataStorage enchData = ench.GetComponent<EnchantDataStorage>();
                // instantiate the prefab
                tempButtonList = Instantiate(_enchantInfoPrefab);
                tempButtonList.transform.SetParent(_enchButtonParent.transform, false);

                // set up button text
                TextMeshProUGUI t = tempButtonList.GetComponentInChildren<TextMeshProUGUI>();
                t.text = enchData.EnchantName + "\n+" + enchData.AmountOfBuff;


                // add to list
                InsertEnchantButton(tempButtonList, e);
            }
            e++;
        }
        _descriptionText1.text = "new text";
    }

    private ColorBlock ItemColorBlock;
    private ColorBlock PartColorBlock;
    private ColorBlock MaterialColorBlock;
    private ColorBlock EnchantColorBlock;
    private void setupHeader()
    {
        //Debug.Log("Setting up Inventory Header - current status: " + _removingStatus.ToString());

        ItemColorBlock = _headerButtonItems.colors;
        PartColorBlock = _headerButtonParts.colors;
        MaterialColorBlock = _headerButtonMaterials.colors;
        EnchantColorBlock = _headerButtonEnchants.colors;
        if (_removingStatus == removingItemStatusEnum.NotRemoving)
        {
            _headerButtonItems.interactable = true;
            _headerButtonParts.interactable = true;
            _headerButtonMaterials.interactable = true;
            _headerButtonEnchants.interactable = true; // for now

            ItemColorBlock.colorMultiplier = 1f;
            PartColorBlock.colorMultiplier = 1f;
            MaterialColorBlock.colorMultiplier = 1f;
            EnchantColorBlock.colorMultiplier = 1f;
        }
        else if (_removingStatus == removingItemStatusEnum.RemovingToSell || _removingStatus == removingItemStatusEnum.RemovingToDisassemble)
        {   // removing item from inventory
            _headerButtonItems.interactable = false;
            _headerButtonParts.interactable = false;
            _headerButtonMaterials.interactable = false;
            _headerButtonEnchants.interactable = false;

            ItemColorBlock.colorMultiplier = 2f;
            PartColorBlock.colorMultiplier = 1f;
            MaterialColorBlock.colorMultiplier = 1f;
        }
        else if (_removingStatus == removingItemStatusEnum.RemovingToCraft1 || _removingStatus == removingItemStatusEnum.RemovingToCraft2 || _removingStatus == removingItemStatusEnum.RemovingToCraft3)
        {   // removing part from inventory
            _headerButtonItems.interactable = false;
            _headerButtonParts.interactable = false;
            _headerButtonMaterials.interactable = false;
            _headerButtonEnchants.interactable = false;

            ItemColorBlock.colorMultiplier = 1f;
            PartColorBlock.colorMultiplier = 2f;
            MaterialColorBlock.colorMultiplier = 1f;
        }
        else if (_removingStatus == removingItemStatusEnum.RemovingToCraftMat)
        {
            _headerButtonItems.interactable = false;
            _headerButtonParts.interactable = false;
            _headerButtonMaterials.interactable = false;
            _headerButtonEnchants.interactable = false;

            ItemColorBlock.colorMultiplier = 1f;
            PartColorBlock.colorMultiplier = 1f;
            MaterialColorBlock.colorMultiplier = 2f;
        }
        _headerButtonItems.colors = ItemColorBlock;
        _headerButtonParts.colors = PartColorBlock;
        _headerButtonMaterials.colors = MaterialColorBlock;
        _headerButtonEnchants.colors = EnchantColorBlock;
    }

    private string _itemName;
    private string _materials;
    private string _totalStrength;
    private string _totalDex;
    private string _totalInt;
    private string _enchantment;
    private string _totalValue;
    public void setItemDetailText(int index)
    {
        if (index != -1)
        {
            if (_inventoryData.ItemInventory[index] != null)
            {
                // get reference to ItemDataStorage script
                ItemDataStorage itemDataRef = _inventoryData.ItemInventory[index].GetComponent<ItemDataStorage>();

                // set up text strings
                _itemName = "Item - " + itemDataRef.ItemName;

                _materials = "\nMaterials\n" + itemDataRef.Part1.Material.Material
                    + "\n" + itemDataRef.Part2.Material.Material
                    + "\n" + itemDataRef.Part3.Material.Material;
                _totalStrength = "\nStrength: " + itemDataRef.TotalStrength;
                _totalDex = "\nDextarity: " + itemDataRef.TotalDextarity;
                _totalInt = "\nIntelegence: " + itemDataRef.TotalIntelegence;

                _enchantment = "\n\nEnchantment:\n";
                if (itemDataRef.IsEnchanted)
                {
                    EnchantDataStorage enc = itemDataRef.Enchantment;
                    _enchantment += enc.EnchantName + " +" + enc.AmountOfBuff;

                    if (enc.EnchantType == "StatBuff_STR")
                        _totalStrength += (" (+" + enc.AmountOfBuff + ")");
                    else if (enc.EnchantType == "StatBuff_DEX")
                        _totalDex += (" (+" + enc.AmountOfBuff + ")");
                    else if (enc.EnchantType == "StatBuff_INT")
                        _totalInt += (" (+" + enc.AmountOfBuff + ")");
                }
                else
                    _enchantment += "none";

                _totalValue = "\n\nValue: " + itemDataRef.TotalValue;

                // organize the texts
                _descriptionText1.text = _itemName +
                    "\nStats" + _totalStrength + _totalDex + _totalInt + _enchantment;
                _descriptionText2.text = _materials + _totalValue;
            }
            else { _descriptionText1.text = ""; _descriptionText2.text = ""; }
            //Debug.Log("Item Detail Text set for - item index: " + index);
        }
        else { _descriptionText1.text = "";  _descriptionText2.text = ""; }
    }

    private string _partName;
    private string _material;
    private string _partStrength;
    private string _partDex;
    private string _partInt;
    private string _partValue;
    private string _enchant;
    public void setPartDetailText(int index)
    {
        if (index != -1)
        {
            if (_inventoryData.PartInventory[index] != null)
            {
                // get reference to PartDataStorage script
                PartDataStorage partDataRef = _inventoryData.PartInventory[index].GetComponent<PartDataStorage>();

                // set up text strings
                _partName = "Part - " + partDataRef.PartName;
                _material = "\nMaterial\n" + partDataRef.MaterialName;
                _partStrength = "\nStrenght: " + partDataRef.PartStr;
                _partDex = "\nDextarity: " + partDataRef.PartDex;
                _partInt = "\nIntelegence: " + partDataRef.PartInt;
                _partValue = "\n\nValue: " + partDataRef.Value;
                _enchant = "";

                if (partDataRef.IsHoldingEnchant == true)
                {
                    _enchant += "\n\nEnchantment:\n" + partDataRef.Enchantment.EnchantName + " +" + partDataRef.Enchantment.AmountOfBuff;
                }

                // organize the texts
                _descriptionText1.text = _partName +
                    "\nStats" + _partStrength + _partDex + _partInt + _enchant;
                _descriptionText2.text = _material + _partValue;
            }
            else
            {
                _descriptionText1.text = "";
                _descriptionText2.text = "";
            }
        }
        else
        {
            _descriptionText1.text = "";
            _descriptionText2.text = "";
        }
    }

    private string _matName;
    private string _matLevel;
    private string _matType;
    private string _matStrength;
    private string _matDex;
    private string _matInt;
    private string _matValue;

    public void setMatDetailText(int index)
    {
        if (index != -1)
        {
            if (_materialButtonList[index] != null)
            {
                MaterialData matDataRef = _materialButtonList[index].GetComponent<MaterialButton>().MaterialData;

                _matName = "Material - " + matDataRef.Material;
                _matLevel = "\nLevel: " + matDataRef.LevelRequirement;
                _matType = "\nType - " + matDataRef.MaterialType;
                _matStrength = "\nStrength: " + matDataRef.AddedStrength.ToString();
                _matDex = "\nDextarity: " + matDataRef.AddedDextarity.ToString();
                _matInt = "\nIntelegence: " + matDataRef.AddedIntelligence.ToString();
                _matValue = "\nValue: " + matDataRef.BaseCostPerUnit.ToString();

                _descriptionText1.text = _matName + _matType + _matLevel + _matValue + "\n" + _matStrength + _matDex + _matInt;
            }
            else
            {
                _descriptionText1.text = "";
                _descriptionText2.text = "";
            }
        }
        else
        {
            _descriptionText1.text = "";
            _descriptionText2.text = "";
        }
    }

    private string _enchantName;
    private string _valueOfBuff;
    public void setEnchantDetailText(int index)
    {
        if (index != -1)
        {
            if (_inventoryData.EnchantInventory[index] != null)
            {
                EnchantDataStorage enchantDataRef = _inventoryData.EnchantInventory[index].GetComponent<EnchantDataStorage>();

                _enchantName = "Enchantment - " + enchantDataRef.EnchantName;
                _valueOfBuff = "\nBuff Amount: +" + enchantDataRef.AmountOfBuff;

                _descriptionText1.text = _enchantName + _valueOfBuff;
            }
            else
                _descriptionText1.text = "new text";
        }
        else
            _descriptionText1.text = "new text";
    }

    private GameObject itemDataStorageTemp;
    private GameObject part1DataStorageTemp;
    private GameObject part2DataStorageTemp;
    private GameObject part3DataStorageTemp;
    private ItemDataStorage itemDataScriptRef;
    private PartDataStorage part1DataScriptRef;
    private PartDataStorage part2DataScriptRef;
    private PartDataStorage part3DataScriptRef;
    public GameObject convertItemData(ItemData item)
    {
        // get variables set up
        itemDataStorageTemp = Instantiate(_itemDataStoragePrefab);
        itemDataStorageTemp.transform.parent = this.gameObject.transform;
        itemDataScriptRef = itemDataStorageTemp.GetComponent<ItemDataStorage>();

        // convert data from scriptable object to gameobject
        //  stats
        itemDataStorageTemp.name = item.ItemName;
        itemDataScriptRef.setItemName(item.ItemName);
        itemDataScriptRef.setTotalValue(item.TotalValue);
        itemDataScriptRef.setTotalStrenght(item.TotalStrength);
        itemDataScriptRef.setTotalDex(item.TotalDextarity);
        itemDataScriptRef.setTotalInt(item.TotalIntelegence);

        //  parts
            // part 1
        part1DataStorageTemp = Instantiate(_partDataStoragePrefab);
        part1DataStorageTemp.transform.parent = itemDataStorageTemp.gameObject.transform;
        part1DataScriptRef = part1DataStorageTemp.GetComponent<PartDataStorage>();
        // store reference to scriptabe object
        part1DataScriptRef.setRecipeData(item.Part1);
        // convert data from scriptable object to gameobject
        part1DataStorageTemp.name = item.Part1.Material.Material + " " + item.Part1.PartName;
        part1DataScriptRef.setPartName(item.Part1.PartName);
        part1DataScriptRef.setMaterial(item.Part1.Material);
        part1DataScriptRef.setPartStr(item.Part1.PartStrenght);
        part1DataScriptRef.setPartDex(item.Part1.PartDextarity);
        part1DataScriptRef.setPartInt(item.Part1.PartIntelligence);
        part1DataScriptRef.setValue(item.Part1.TotalCurrentValue);
        // store ref of part 1 in item script
        itemDataScriptRef.setPart1(part1DataScriptRef);

            // part 2
        part2DataStorageTemp = Instantiate(_partDataStoragePrefab);
        part2DataStorageTemp.transform.parent = itemDataStorageTemp.gameObject.transform;
        part2DataScriptRef = part2DataStorageTemp.GetComponent<PartDataStorage>();
        // store reference to scriptabe object
        part2DataScriptRef.setRecipeData(item.Part2);
        // convert data from scriptable object to gameobject
        part2DataStorageTemp.name = item.Part2.Material.Material + " " + item.Part2.PartName;
        part2DataScriptRef.setPartName(item.Part2.PartName);
        part2DataScriptRef.setMaterial(item.Part2.Material);
        part2DataScriptRef.setPartStr(item.Part2.PartStrenght);
        part2DataScriptRef.setPartDex(item.Part2.PartDextarity);
        part2DataScriptRef.setPartInt(item.Part2.PartIntelligence);
        part2DataScriptRef.setValue(item.Part2.TotalCurrentValue);
        // store ref of part 2 in item script
        itemDataScriptRef.setPart2(part2DataScriptRef);

        // part 3
        part3DataStorageTemp = Instantiate(_partDataStoragePrefab);
        part3DataStorageTemp.transform.parent = itemDataStorageTemp.gameObject.transform;
        part3DataScriptRef = part3DataStorageTemp.GetComponent<PartDataStorage>();
        // store reference to scriptabe object
        part3DataScriptRef.setRecipeData(item.Part3);
        // convert data from scriptable object to gameobject
        part3DataStorageTemp.name = item.Part3.Material.Material + " " + item.Part3.PartName;
        part3DataScriptRef.setPartName(item.Part3.PartName);
        part3DataScriptRef.setMaterial(item.Part3.Material);
        part3DataScriptRef.setPartStr(item.Part3.PartStrenght);
        part3DataScriptRef.setPartDex(item.Part3.PartDextarity);
        part3DataScriptRef.setPartInt(item.Part3.PartIntelligence);
        part3DataScriptRef.setValue(item.Part3.TotalCurrentValue); 
        // store ref of part 3 in item script
        itemDataScriptRef.setPart3(part3DataScriptRef);

            // enchantment
        itemDataScriptRef.setIsEnchanted(item.IsEnchanted);
        if (item.IsEnchanted)
        {
            GameObject enc = convertEnchantData(GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GenerateItem>().getEnchantment);

            enc.transform.parent = itemDataStorageTemp.gameObject.transform;
            itemDataScriptRef.setEnchantment(enc.GetComponent<EnchantDataStorage>());

            itemDataScriptRef.setTotalValue(itemDataScriptRef.TotalValue + enc.GetComponent<EnchantDataStorage>().AddedValueOfEnchant);
        }

        return itemDataStorageTemp;
    }
    public GameObject convertItemData(SaveItemObject item)
    {
        itemDataStorageTemp = Instantiate(_itemDataStoragePrefab);
        itemDataStorageTemp.transform.parent = this.gameObject.transform;
        itemDataScriptRef = itemDataStorageTemp.GetComponent<ItemDataStorage>();

        // get data from object and insert into gameobject
        // stats
        itemDataStorageTemp.name = item.itemName;
        itemDataScriptRef.setItemName(item.itemName);
        itemDataScriptRef.setTotalValue(item.totalValue);
        itemDataScriptRef.setTotalStrenght(item.totalStrenght);
        itemDataScriptRef.setTotalDex(item.totalDextarity);
        itemDataScriptRef.setTotalInt(item.totalIntellegence);

        // parts
            // part 1
        part1DataStorageTemp = Instantiate(_partDataStoragePrefab);
        part1DataStorageTemp.transform.parent = itemDataStorageTemp.gameObject.transform;
        part1DataScriptRef = part1DataStorageTemp.GetComponent<PartDataStorage>();
        // setup reference to scriptabe object recipe
        part1DataScriptRef.setRecipeData(GameObject.FindGameObjectWithTag("RecipeBookControl").GetComponent<RecipeBook>().getPartRecipe(item.part1.partRecipeName));
        // convert data from object to gameobject
        part1DataStorageTemp.name = item.part1.materialName + " " + item.part1.partName;
        part1DataScriptRef.setPartName(item.part1.partName);
        part1DataScriptRef.setMaterial(_inventoryData.getMaterialData(item.part1.materialName));
        part1DataScriptRef.setPartStr(item.part1.partStrength);
        part1DataScriptRef.setPartDex(item.part1.partDextarity);
        part1DataScriptRef.setPartInt(item.part1.partIntellegence);
        part1DataScriptRef.setValue(item.part1.totalValue);
        // store ref of part 1 in item script
        itemDataScriptRef.setPart1(part1DataScriptRef);

            // part 2
        part2DataStorageTemp = Instantiate(_partDataStoragePrefab);
        part2DataStorageTemp.transform.parent = itemDataStorageTemp.gameObject.transform;
        part2DataScriptRef = part2DataStorageTemp.GetComponent<PartDataStorage>();
        // setup reference to scriptabe object recipe
        part2DataScriptRef.setRecipeData(GameObject.FindGameObjectWithTag("RecipeBookControl").GetComponent<RecipeBook>().getPartRecipe(item.part2.partRecipeName));
        // convert data from object to gameobject
        part2DataStorageTemp.name = item.part2.materialName + " " + item.part2.partName;
        part2DataScriptRef.setPartName(item.part2.partName);
        part2DataScriptRef.setMaterial(_inventoryData.getMaterialData(item.part2.materialName));
        part2DataScriptRef.setPartStr(item.part2.partStrength);
        part2DataScriptRef.setPartDex(item.part2.partDextarity);
        part2DataScriptRef.setPartInt(item.part2.partIntellegence);
        part2DataScriptRef.setValue(item.part2.totalValue);
        // store ref of part 1 in item script
        itemDataScriptRef.setPart2(part2DataScriptRef);

            // part 3
        part3DataStorageTemp = Instantiate(_partDataStoragePrefab);
        part3DataStorageTemp.transform.parent = itemDataStorageTemp.gameObject.transform;
        part3DataScriptRef = part3DataStorageTemp.GetComponent<PartDataStorage>();
        // setup reference to scriptabe object recipe
        part3DataScriptRef.setRecipeData(GameObject.FindGameObjectWithTag("RecipeBookControl").GetComponent<RecipeBook>().getPartRecipe(item.part3.partRecipeName));
        // convert data from object to gameobject
        part3DataStorageTemp.name = item.part3.materialName + " " + item.part3.partName;
        part3DataScriptRef.setPartName(item.part3.partName);
        part3DataScriptRef.setMaterial(_inventoryData.getMaterialData(item.part3.materialName));
        part3DataScriptRef.setPartStr(item.part3.partStrength);
        part3DataScriptRef.setPartDex(item.part3.partDextarity);
        part3DataScriptRef.setPartInt(item.part3.partIntellegence);
        part3DataScriptRef.setValue(item.part3.totalValue);
        // store ref of part 1 in item script
        itemDataScriptRef.setPart3(part3DataScriptRef);

        // enchantment
        itemDataScriptRef.setIsEnchanted(item.isEnchanted);
        if (item.isEnchanted)
        {
            GameObject enc = convertEnchantData(item.enchantment);

            enc.transform.parent = itemDataStorageTemp.gameObject.transform;
            itemDataScriptRef.setEnchantment(enc.GetComponent<EnchantDataStorage>());

            itemDataScriptRef.setTotalValue(itemDataScriptRef.TotalValue + enc.GetComponent<EnchantDataStorage>().AddedValueOfEnchant);
        }

        return itemDataStorageTemp;
    }

    private GameObject partDataStorageTemp;
    private PartDataStorage partDataScriptRef;
    public GameObject convertPartData(PartData part)
    {
        // get variables set up
        partDataStorageTemp = Instantiate(_partDataStoragePrefab);
        partDataStorageTemp.transform.parent = this.gameObject.transform;
        partDataScriptRef = partDataStorageTemp.GetComponent<PartDataStorage>();

        // convert data from scriptable object to gameobject
        //  stats
        partDataStorageTemp.name = part.PartName;
        partDataScriptRef.setPartName(part.PartName);
        partDataScriptRef.setMaterial(part.Material);
        partDataScriptRef.setPartStr(part.PartStrenght);
        partDataScriptRef.setPartDex(part.PartDextarity);
        partDataScriptRef.setPartInt(part.PartIntelligence);
        partDataScriptRef.setValue(part.TotalCurrentValue);

        return partDataStorageTemp;
    }
    public GameObject convertPartData(SavePartObject part)
    {
        // get variables set up
        partDataStorageTemp = Instantiate(_partDataStoragePrefab);
        partDataStorageTemp.transform.parent = this.gameObject.transform;
        partDataScriptRef = partDataStorageTemp.GetComponent<PartDataStorage>();

        // convert data from scriptable object to gameobject
        //  stats
        partDataStorageTemp.name = part.partName;
        partDataScriptRef.setPartName(part.partName);
        partDataScriptRef.setMaterial(_inventoryData.getMaterialData(part.materialName)); 
        partDataScriptRef.setPartStr(part.partStrength);
        partDataScriptRef.setPartDex(part.partDextarity);
        partDataScriptRef.setPartInt(part.partIntellegence);
        partDataScriptRef.setValue(part.totalValue);
        // recipe
        partDataScriptRef.setRecipeData(GameObject.FindGameObjectWithTag("RecipeBookControl").GetComponent<RecipeBook>().getPartRecipe(part.partRecipeName));
        // enchant
        partDataScriptRef.setIsHoldingEnchanted(part.isEnchanted);
        if (part.isEnchanted)
        {
            //partDataScriptRef.setEnchantment(part.enchantment);
            GameObject enc = convertEnchantData(part.enchantment);

            enc.transform.parent = partDataStorageTemp.gameObject.transform;
            partDataScriptRef.setEnchantment(enc.GetComponent<EnchantDataStorage>());

            partDataScriptRef.setValue(partDataScriptRef.Value + enc.GetComponent<EnchantDataStorage>().AddedValueOfEnchant);
        }

        return partDataStorageTemp;
    }

    private GameObject enchantDataStorageTemp;
    private EnchantDataStorage enchantDataScriptRef;
    public GameObject convertEnchantData(EnchantData enchant)
    {
        // get variables set up
        enchantDataStorageTemp = Instantiate(_enchantDataStoragePrefab);
        enchantDataStorageTemp.transform.parent = this.gameObject.transform;
        enchantDataScriptRef = enchantDataStorageTemp.GetComponent<EnchantDataStorage>();

        // convert data from scriptable object to gameobject
        enchantDataStorageTemp.name = enchant.EnchantName + " Enchantment";
        enchantDataScriptRef.setEnchantName(enchant.EnchantName);
        enchantDataScriptRef.setEnchantType(enchant.EnchantType);
        enchantDataScriptRef.setAmountOfBuff(enchant.GetRandomBuff);
        enchantDataScriptRef.setValueOfEnchant(enchant.BaseAddedValuePerLevel * enchantDataScriptRef.AmountOfBuff);

        return enchantDataStorageTemp;
    }
    public GameObject convertEnchantData(SaveEnchantObject enchant)
    {
        // get variables set up
        enchantDataStorageTemp = Instantiate(_enchantDataStoragePrefab);
        enchantDataStorageTemp.transform.parent = this.gameObject.transform;
        enchantDataScriptRef = enchantDataStorageTemp.GetComponent<EnchantDataStorage>();

        enchantDataStorageTemp.name = enchant.enchName + " Enchantment";
        enchantDataScriptRef.setEnchantName(enchant.enchName);
        enchantDataScriptRef.setEnchantType(enchant.enchBuffType);
        enchantDataScriptRef.setAmountOfBuff(enchant.valueOfBuff);
        enchantDataScriptRef.setValueOfEnchant(enchant.valueOfEnchant * enchantDataScriptRef.AmountOfBuff);

        return enchantDataStorageTemp;
    }

    private bool ItemSlotEmpty(int index)
    {
        if (_inventoryData.ItemInventory[index] == null)
            return true;

        return false;
    }
    private bool PartSlotEmpty(int index)
    {
        if (_inventoryData.PartInventory[index] == null)
            return true;

        return false;
    }
    private bool EnchantSlotEmpty (int index)
    {
        if (_inventoryData.EnchantInventory[index] == null)
            return true;

        return false;
    }
    private bool ItemButtonSlotEmpty(int index)
    {
        if (_itemButtonList[index] == null)
            return true;

        return false;
    }
    private bool PartButtonSlotEmpty(int index)
    {
        if (_partButtonList[index] == null)
            return true;

        return false;
    }
    private bool MaterialSlotEmpty(int index)
    {
        if (_inventoryData.MaterialInventory[index] == null || _inventoryData.MaterialInventory[index].GetComponent<MaterialDataStorage>().MaterialCount <= 0)
            return true;

        return false;
    }
    private bool MatButtonSlotEmpty(int index)
    {
        if (_materialButtonList[index] == null)
            return true;

        return false;
    }
    private bool EnchantButtonSlotEmpty(int index)
    {
        if (_enchantButtonList[index] == null)
            return true;

        return false;
    }

    // may or may not need this code
    private bool GetMaterial(int index, out MaterialData mat)
    {
        if (MaterialSlotEmpty(index))
        {
            mat = null;
            return false;
        }
        mat = _inventoryData.MaterialInventory[index].GetComponent<MaterialDataStorage>().MatDataRef;
        return true;
    }

    // insert an item/part/material, return the index where it was inserted. -1 if error.
    public int InsertItem(ItemData item)
    {
        //Debug.Log("insterting item: " + item.ItemName);
        GameObject temp = convertItemData(item);

        return _inventoryData.insertItemData(temp);

        /*
        for (int i = 0; i < _inventoryData.ItemInventory.Count; i++)
        {
            if (ItemSlotEmpty(i))
            {
                //Debug.Log("i: " + i);
                temp.GetComponent<ItemDataStorage>().setInventoryIndex(i);
                _inventoryData.insertItemData(temp, i);
                return i;
            }
        }
        return -1;
        */
    }
    public int InsertItem(GameObject item)
    {
        if (item.GetComponent<ItemDataStorage>() != null)
        {
            item.GetComponent<ItemDataStorage>().setInventoryIndex(_inventoryData.insertItemData(item));
            return item.GetComponent<ItemDataStorage>().InventoryIndex;
        }
        else if (item.GetComponent<ItemDataStorage>() == null)
            Debug.LogWarning("Item does not contain the ItemDataStorage component!");
        return -1;
        /*
        for (int i = 0; i < _inventoryData.ItemInventory.Count; i++)
        {
            if (ItemSlotEmpty(i))
            {
                item.GetComponent<ItemDataStorage>().setInventoryIndex(i);
                _inventoryData.ItemInventory[i] = item;
                return i;
            }
        }
        return -1;
        */
    }
    /*
    public int InsertPart(PartData part)
    {
        GameObject temp = convertPartData(part);

        return _inventoryData.insertPartData(temp);
        /*
        for (int i = 0; i < _inventoryData.PartInventory.Count; i++)
            if (PartSlotEmpty(i))
            {
                //_inventoryData.insertPartData(temp, i);
                _inventoryData.insertPartData(temp);
                return i;
            }

        return -1;
    }
    */
    public int InsertPart(GameObject part)
    {
        if (part.GetComponent<PartDataStorage>() != null)
        {
            return _inventoryData.insertPartData(part);
        }
        else if (part.GetComponent<PartDataStorage>() == null)
            Debug.LogWarning("Part does not contain the PartDataStorage component!");
        return -1;
    }
    /*
    public int InsertPartData(GameObject part)
    {
        //Debug.Log(part.name);
        if (part.GetComponent<PartDataStorage>() != null)
        {
            return _inventoryData.insertPartData(part);
        }
        else if (part.GetComponent<PartDataStorage>() == null)
            Debug.LogWarning("Part does not contain the PartDataStorage component!");
        return -1;
    }
    */
    public int InsertEnchatment(GameObject ench)
    {
        if (ench.GetComponent<EnchantDataStorage>() != null)
        {
            return _inventoryData.insertEnchantData(ench);
        }
        else if (ench.GetComponent<EnchantDataStorage>() == null)
            Debug.LogWarning("Enchant does not containt the EnchantDataStorage component!");
        return -1;
    }
    private int InsertItemButton(GameObject button, int j)
    {
        _itemButtonList.Add(button);
        for (int i = 0; i < _itemButtonList.Count; i++)
            if (_itemButtonList[i] == button)
                button.GetComponent<InventoryButton>().setMyIndex(i);
        button.GetComponent<InventoryButton>().setItemIndex(j);
        return button.GetComponent<InventoryButton>().MyIndex;
    }
    private int InsertPartButton(GameObject button, int j)
    {
        _partButtonList.Add(button);
        for (int i = 0; i < _partButtonList.Count; i++)
            if (_partButtonList[i] == button)
                button.GetComponent<InventoryButton>().setMyIndex(i);
        button.GetComponent<InventoryButton>().setPartIndex(j);
        return button.GetComponent<InventoryButton>().MyIndex;
    }
    private int InsertMatButton(GameObject button, int k)
    {
        _materialButtonList.Add(button);
        for (int i = 0; i < _materialButtonList.Count; i++)
            if (_materialButtonList[i] == button)
                button.GetComponent<MaterialButton>().setMyIndex(i);
        button.GetComponent<MaterialButton>().setMatIndex(k);
        return button.GetComponent<MaterialButton>().MyIndex;
    }
    private int InsertEnchantButton(GameObject button, int l)
    {
        _enchantButtonList.Add(button);
        for (int i = 0; i < _enchantButtonList.Count; i++)
            if (_enchantButtonList[i] == button)
                button.GetComponent<InventoryButton>().setMyIndex(i);
        button.GetComponent<InventoryButton>().setEnchantIndex(l);
        return button.GetComponent<InventoryButton>().MyIndex;
    }
    
    public void RemoveItem(GameObject item)
    {
        _inventoryData.removeItem(item);
        _descriptionText1.text = "item text";
        _selectedItem = null;

        setupItemInventory();
    }
    public void RemovePart(GameObject part, bool destroy)
    {
        _inventoryData.removePart(part, destroy);
    }
    
    private void clearItemButtonList()
    {
        foreach (GameObject go in _itemButtonList)
            Destroy(go);

        for (int j = _itemButtonList.Count - 1; j >= 0; j--)
            _itemButtonList.RemoveAt(j);
    }
    private void clearPartButtonList()
    {
        foreach (GameObject go in _partButtonList)
            Destroy(go);

        for (int j = _partButtonList.Count - 1; j >= 0; j--)
            _partButtonList.RemoveAt(j);
    }
    private void clearMatButtonList()
    {
        foreach (GameObject go in _materialButtonList)
            Destroy(go);

        for (int k = _materialButtonList.Count - 1; k >= 0; k--)
            _materialButtonList.RemoveAt(k);
    }
    private void clearEnchantButtonList()
    {
        foreach (GameObject go in _enchantButtonList)
            Destroy(go);

        for (int l = _enchantButtonList.Count - 1; l >= 0; l--)
            _enchantButtonList.RemoveAt(l);
    }

    public void forceClearItemInventory()
    {
        _inventoryData.removeAllItems();
    }
    public void forceClearPartInventory()
    {
        _inventoryData.removeAllParts();
    }
    public void forceClearEnchantInventory()
    {
        _inventoryData.removeAllEnchants();
    }

    public void setSelectedItem(int i)
    {
        if (i != -1)
        {
            _selectedItem = _inventoryData.ItemInventory[i];
            returnSelectedItem();
            //Debug.Log("Selected item is: " + _selectedItem.name + " at index: " + i); 
        }
        else
            Debug.Log("example button selected");
    }
    public void setSelectedPart(int i)
    {
        if (i != -1)
        {
            _selectedPart = _inventoryData.PartInventory[i];
            returnSeletedPart();
            //Debug.Log("Selected part is: " + _selectedPart.name + " at index: " + i);
        }
        else
            Debug.Log("example button selected");
    }
    public void setSelectedMat(MaterialData data)
    {
        if (data != null)
            _selectedMat = data;
        else
            Debug.LogWarning("No mat input!");
    }
    public void setSelectedMat(int i)
    {
        if (i != -1)
        {
            _selectedMat = _inventoryData.MaterialInventory[i].GetComponent<MaterialDataStorage>().MatDataRef;
        }
        else
            Debug.Log("example button selected");
    }
    public void setSelectedEnchant(int i)
    {
        if (i != -1)
        {
            _selectedEnchant = _inventoryData.EnchantInventory[i];
        }
        else
            Debug.Log("example button selected");
    }

    public GameObject getSelectedItem()
    {
        if (_selectedItem != null) 
            return _selectedItem;
         
        return null;
    }
    public GameObject getSelectedPart()
    {
        if (_selectedPart != null)
        {
            return _selectedPart;
        }

        return null;
    }
    public MaterialData getSelectedMat()
    {
        if (_selectedMat != null)
            return this.GetComponent<InventoryData>().getMaterialData(_selectedMat.Material);

        return null;
    }
    public GameObject getSelectedEnchant()
    {
        if (_selectedEnchant != null)
            return _selectedEnchant;
        return null;
    }

    private void setStatus(int value)
    {
        //Debug.Log("setStatus - value = " + value);
        if (value == 0) _removingStatus = removingItemStatusEnum.NotRemoving;
        else if (value == 1) _removingStatus = removingItemStatusEnum.RemovingToSell;
        else if (value == 2) _removingStatus = removingItemStatusEnum.RemovingToDisassemble;
        else if (value == 3) _removingStatus = removingItemStatusEnum.RemovingToCraft1;
        else if (value == 4) _removingStatus = removingItemStatusEnum.RemovingToCraft2;
        else if (value == 5) _removingStatus = removingItemStatusEnum.RemovingToCraft3;
        else if (value == 6) _removingStatus = removingItemStatusEnum.RemovingToCraftMat;
        else if (value == 7) _removingStatus = removingItemStatusEnum.RemovingToEnchant;
        else
        {
            _removingStatus = removingItemStatusEnum.NotRemoving;
            Debug.LogWarning("Invalid input status!");
        }
    }

    public void returnSelectedItem()
    {
        if (_UIControlRef.ShopUIEnabled == true)
        {
            //Debug.Log("shop ui enabled");
            if (_UIControlRef.ShopEcoUIEnabled == false && _UIControlRef.ShopDisUIEnabled == false)
            {
                //Debug.Log("shop eco and shop disassemble not enabled");
                _UIControlRef.ShopEcoUIEnabled = false;
                _UIControlRef.ShopDisUIEnabled = true;
                _UIControlRef.ShopCraftUIEnabled = false;
            }
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<SellItemControl>().selectItem();
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<DisassembleItemControl>().selectItem();
        }
        else if (_UIControlRef.MarketUIEnabled == true)
        {
            //Debug.Log("market ui enabled");
            if (!_UIControlRef.MarketEcoUIEnabled)
            {
                _UIControlRef.MarketEcoUIEnabled = true;
                _UIControlRef.MarketQuestUIEnabled = false;
            }
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<SellItemControl>().selectItem();
        }
    }

    public void selectRandomItem()
    {
        _removingStatus = removingItemStatusEnum.RemovingToSell;
        this.gameObject.GetComponent<InventoryData>().getRandomItem();

        returnSelectedItem();
    }

    [SerializeField] private int partLastFilled = 0;
    public void returnSeletedPart()
    {
        bool phb = false;
        if (_craftControlRef.anyItemRecipeSelected() == true)
        {
            //Debug.LogWarning("TODO setup code for selecting parts");
            foreach(PartData part1ref in _craftControlRef.checkItemRecipe().ValidParts1)
                if (_selectedPart.GetComponent<PartDataStorage>().RecipeData == part1ref
                    && (partLastFilled == 0 || partLastFilled == 2 || partLastFilled == 3 || _craftControlRef.Part1Set() == false || _craftControlRef.AllPartsSet() == true)
                    && phb == false)
                {
                    Debug.LogWarning("selected part matches recipe part 1");
                    _craftControlRef.SelectPart1();
                    partLastFilled = 1;
                    phb = true;
                }

            foreach(PartData part2ref in _craftControlRef.checkItemRecipe().ValidParts2)
                if (_selectedPart.GetComponent<PartDataStorage>().RecipeData == part2ref
                    && (partLastFilled == 0 || partLastFilled == 1 || partLastFilled == 3 || _craftControlRef.Part2Set() == false || _craftControlRef.AllPartsSet() == true)
                    && phb == false)
                {
                    Debug.LogWarning("selected part matches recipe part 2");
                    _craftControlRef.SelectPart2();
                    partLastFilled = 2;
                    phb = true;
                }

            foreach (PartData part3ref in _craftControlRef.checkItemRecipe().ValidParts3)
                if (_selectedPart.GetComponent<PartDataStorage>().RecipeData == part3ref
                    && (partLastFilled == 0 || partLastFilled == 1 || partLastFilled == 2 || _craftControlRef.Part3Set() == false || _craftControlRef.AllPartsSet() == true)
                    && phb == false)
                {
                    Debug.LogWarning("selected part matches recipe part 3");
                    _craftControlRef.SelectPart3();
                    partLastFilled = 3;
                    phb = true;
                }
        }
        phb = false;
    }

    public void returnSelectedMat()
    {
        if (_removingStatus == removingItemStatusEnum.RemovingToCraftMat)
            GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().SelectMat();
    }

    public void returnSelectedEnchant()
    {
        if (_removingStatus == removingItemStatusEnum.RemovingToEnchant)
            GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().SelectEnchant();
    }

    private bool checkIfPartIsValid(int i, PartDataStorage part) // incoming i should either be 3, 4, or 5
    {
        CraftControl ccRef = GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>();
        PartData validPart = part.RecipeData;

        if (i == 3)
        {
            foreach (PartData validPart1 in ccRef.checkItemRecipe().ValidParts1)
                if (validPart1.PartName == validPart.PartName)
                    return true;
        }
        else if (i == 4)
        {
            foreach (PartData validPart2 in ccRef.checkItemRecipe().ValidParts2)
                if (validPart2.PartName == validPart.PartName)
                    return true;
        }
        else if (i == 5)
        {
            foreach (PartData validPart3 in ccRef.checkItemRecipe().ValidParts3)
                if (validPart3.PartName == validPart.PartName)
                    return true;
        }

        return false;
    }

    private bool checkIfMatIsValid(int m, MaterialData mat) // incoming m should be 6
    {
        CraftControl ccRef = GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>();

        if (m == 6)
        {
            foreach (MaterialData validMat in ccRef.checkPartRecipe().ValidMaterialData)
                if (validMat.Material == mat.Material)
                    return true;
        }

        return false;
    }
}
