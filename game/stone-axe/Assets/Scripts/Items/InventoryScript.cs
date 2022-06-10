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
    [SerializeField]
    private RecipeBook _recipeBookRef;
    /*
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
    */
    [Header("Data")]
    //[SerializeField] private removingItemStatusEnum _removingStatus;

    // item inventory
    [SerializeField] private List<GameObject> _itemButtonList;
    // part inventory
    [SerializeField] private List<GameObject> _partButtonList;
    // material inventory
    [SerializeField] private List<GameObject> _materialButtonList;
    // enchant inventory
    [SerializeField] private List<GameObject> _enchantButtonList;


    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _descriptionText1;
    [SerializeField] private TextMeshProUGUI _descriptionText2;
    [Space(5)]
    [SerializeField] private GameObject _itemsInvScroll;
    [SerializeField] private GameObject _itemsButtonParent;
    [SerializeField] private List<FilterData> _itemsFilterData;
    [SerializeField] private FilterData _currentItemFilter;
    [Space(5)]
    [SerializeField] private GameObject _partsInvScroll;
    [SerializeField] private GameObject _partsButtonParent;
    [SerializeField] private List<FilterData> _partsFilterData;
    [SerializeField] private FilterData _currentPartFilter;
    [Space(5)]
    [SerializeField] private GameObject _matsInvScroll;
    [SerializeField] private GameObject _matsButtonParent;
    [SerializeField] private List<FilterData> _matsFilterData;
    [SerializeField] private FilterData _currentMatFilter;
    [Space(5)]
    [SerializeField] private GameObject _enchInvScroll;
    [SerializeField] private GameObject _enchButtonParent;
    [SerializeField] private List<FilterData> _enchFilterData;
    [SerializeField] private FilterData _currentEnchFilter;
    [Header("Prefabs")]
    [SerializeField] private GameObject _itemDataStoragePrefab;
    [SerializeField] private GameObject _partDataStoragePrefab;
    [SerializeField] private GameObject _enchantDataStoragePrefab;
    [Space(5)]
    [SerializeField] private GameObject _itemInfoPrefab;
    [SerializeField] private GameObject _partInfoPrefab;
    [SerializeField] private GameObject _matInfoPrefab;
    [SerializeField] private GameObject _enchantInfoPrefab;
    [SerializeField] private GameObject _enchantCancelPrefab;

    private GameObject tempButtonList;

    [SerializeField, HideInInspector] private List<GameObject> holderList;
    [SerializeField, HideInInspector] private List<GameObject> setupList;

    private void Awake()
    {
        if (_gameMaster == null)
            _gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        if (_inventoryData == null)
            _inventoryData = this.gameObject.GetComponent<InventoryData>();
        if (_recipeBookRef == null)
            _recipeBookRef = GameObject.FindGameObjectWithTag("RecipeBookControl").GetComponent<RecipeBook>();

        _currentItemFilter = _itemsFilterData[0];
        _currentPartFilter = _partsFilterData[0];
        _currentMatFilter = _matsFilterData[0];
        _currentEnchFilter = _enchFilterData[0];
        _UIControlRef.setupInvFilterUI();
    }

    private void Start()
    {
        setupInventory();
    }

    public void setupInventory()
    {
        setupItemInventory();
        setupPartInventory();
        setupMatInventory();
        setupEnchantInventory();
    }
    public void setupItemInventory()
    {
        clearItemButtonList();
        //_selectedItem = null;

        if (_currentItemFilter.Equals(_itemsFilterData[0]))
        {
            int k = 0;
            foreach (GameObject item in _inventoryData.ItemInventory)
            {
                if (item != null)
                {
                    // get reference to ItemDataStorage script
                    ItemDataStorage itemData = item.GetComponent<ItemDataStorage>();

                    itemStorageSetup(itemData, k, true);
                }
                k++;
            }
        }
        else
        {
            int k = 0;
            foreach (GameObject item in _inventoryData.ItemInventory)
            {
                if (item != null)
                {
                    ItemDataStorage itemDat = item.GetComponent<ItemDataStorage>();

                    if (itemDat.ItemRecipeRef.ValidFilters.Contains(_currentItemFilter))
                    {
                        Debug.Log("InventoryScript.setupItemInventory(): " + itemDat.ItemName + " has the filter: " + _currentItemFilter.FilterName);
                        itemStorageSetup(itemDat, k, true);
                        setupList.Add(item);
                        k++;
                    }
                    else if (itemDat.Part1.Material.ValidFilters.Contains(_currentItemFilter))
                    {
                        Debug.Log("InventoryScript.setupItemInventory(): " + itemDat.ItemName + " Part1.Material has the filter: " + _currentItemFilter.FilterName);
                        itemStorageSetup(itemDat, k, true);
                        setupList.Add(item);
                        k++;
                    }
                    else
                        holderList.Add(item);
                }
            }
            foreach (GameObject item in holderList)
            {
                if (item != null)
                {
                    ItemDataStorage itemDat = item.GetComponent<ItemDataStorage>();
                    itemStorageSetup(itemDat, k, false);
                }
                k++;
            }
            holderList.Clear();
            setupList.Clear();
        }
        
        _descriptionText1.text = "";
        _descriptionText2.text = "";
    }

    private void itemStorageSetup(ItemDataStorage dat, int k, bool isFilterValid)
    {
        // instatiate the button prefab
        tempButtonList = Instantiate(_itemInfoPrefab);
        tempButtonList.transform.SetParent(_itemsButtonParent.transform, false);
        // set up button text
        TextMeshProUGUI t = tempButtonList.GetComponentInChildren<TextMeshProUGUI>();
        t.text = dat.Part1.MaterialName + " " + dat.ItemName;
        // set button data
        tempButtonList.GetComponent<InventoryButton>().setIsNew(dat.IsNew);
        tempButtonList.GetComponent<InventoryButton>().setIsForQuest(dat.IsForQuest);
        tempButtonList.GetComponent<InventoryButton>().setIsEnchanted(dat.IsEnchanted);

        if (isFilterValid == true)
            tempButtonList.GetComponent<InventoryButton>().setAlpha(1f);
        else if (isFilterValid == false)
            tempButtonList.GetComponent<InventoryButton>().setAlpha(.6f);

        int j = _inventoryData.ItemInventory.IndexOf(dat.gameObject);

        // add button to list
        InsertItemButton(tempButtonList, j);
    }
    /* Old Code for itemStorageSetup()
    // instatiate the button prefab
    tempButtonList = Instantiate(_itemInfoPrefab);
    tempButtonList.transform.SetParent(_itemsButtonParent.transform, false);
    // set up button text
    TextMeshProUGUI t = tempButtonList.GetComponentInChildren<TextMeshProUGUI>();
    t.text = itemData.Part1.MaterialName + " " + itemData.ItemName;
    // set button data
    tempButtonList.GetComponent<InventoryButton>().setIsNew(itemData.IsNew);
    tempButtonList.GetComponent<InventoryButton>().setIsForQuest(itemData.IsForQuest);
    tempButtonList.GetComponent<InventoryButton>().setIsEnchanted(itemData.IsEnchanted);

    // add button to list
    InsertItemButton(tempButtonList, k);
    */

    public void setupPartInventory()
    {
        clearPartButtonList();
        //_selectedPart = null;

        if (_currentPartFilter.Equals(_partsFilterData[0])) // if current filter is Defualt
        {
            int k = 0;
            foreach (GameObject part in _inventoryData.PartInventory)
            {
                if (part != null)
                {
                    // get reference to ItemDataStorage script
                    PartDataStorage partData = part.GetComponent<PartDataStorage>();
                    partStorageSetup(partData, k, true);
                }
                k++;
            }
        }
        else if (_currentPartFilter.Equals(_partsFilterData[1])) // if current filter is for craft
        {
            int k = 0;
            if (_craftControlRef.checkItemRecipe() != null)
            {
                Debug.Log("recipe selected!");
                foreach (GameObject part in _inventoryData.PartInventory)
                {
                    PartDataStorage partData = part.GetComponent<PartDataStorage>();
                    if (_craftControlRef.checkItemRecipe().ValidParts1.Contains(partData.RecipeData))
                    {
                        partStorageSetup(partData, k, true);
                        k++;
                    }
                    else if (_craftControlRef.checkItemRecipe().ValidParts2.Contains(partData.RecipeData))
                    {
                        partStorageSetup(partData, k, true);
                        k++;
                    }
                    else if (_craftControlRef.checkItemRecipe().ValidParts3.Contains(partData.RecipeData))
                    {
                        partStorageSetup(partData, k, true);
                        k++;
                    }
                    else
                        holderList.Add(part);
                }
                foreach (GameObject part in holderList)
                {
                    if (part != null)
                    {
                        PartDataStorage partData = part.GetComponent<PartDataStorage>();
                        partStorageSetup(partData, k, false);
                    }
                    k++;
                }
            }
            else if (_craftControlRef.checkQuestRecipe() != null)
            {
                Debug.Log("quest recipe selected!");
                foreach (GameObject part in _inventoryData.PartInventory)
                {
                    PartDataStorage partData = part.GetComponent<PartDataStorage>();
                    if (_craftControlRef.checkQuestRecipe().ItemPart1.Equals(partData.RecipeData))
                    {
                        partStorageSetup(partData, k, true);
                        k++;
                    }
                    else if (_craftControlRef.checkQuestRecipe().ItemPart2.Equals(partData.RecipeData))
                    {
                        partStorageSetup(partData, k, true);
                        k++;
                    }
                    else if (_craftControlRef.checkQuestRecipe().ItemPart3.Equals(partData.RecipeData))
                    {
                        partStorageSetup(partData, k, true);
                        k++;
                    }
                    else
                        holderList.Add(part);
                }
                foreach (GameObject part in holderList)
                {
                    if (part != null)
                    {
                        PartDataStorage partData = part.GetComponent<PartDataStorage>();
                        partStorageSetup(partData, k, false);
                    }
                    k++;
                }
            }
            else
            {
                Debug.Log("InventoryScript.setupPartInventory(): no receipe selected");
                foreach (GameObject part in _inventoryData.PartInventory)
                {
                    if (part != null)
                    {
                        // get reference to ItemDataStorage script
                        PartDataStorage partData = part.GetComponent<PartDataStorage>();
                        partStorageSetup(partData, k, true);
                    }
                    k++;
                }
            }
            holderList.Clear();
            setupList.Clear();
        }
        else
        {
            int k = 0;
            foreach (GameObject part in _inventoryData.PartInventory)
            {
                if (part != null)
                {
                    PartDataStorage partDat = part.GetComponent<PartDataStorage>();
                    if (partDat.Material.ValidFilters.Contains(_currentPartFilter))
                    {
                        //Debug.Log("InventoryScript.setupPartInventory(): " + partDat.PartName + ".Material has the filter: " + _currentPartFilter.FilterName);
                        partStorageSetup(partDat, k, true);
                        setupList.Add(part);
                    }
                    else
                        holderList.Add(part);
                }
                k++;
            }
            foreach (GameObject part in holderList)
            {
                if (part != null)
                {
                    PartDataStorage partDat = part.GetComponent<PartDataStorage>();
                    partStorageSetup(partDat, k, false);
                }
                k++;
            }
            holderList.Clear();
            setupList.Clear();
        }

        _descriptionText1.text = "";
        _descriptionText2.text = "";
    }

    private void partStorageSetup(PartDataStorage dat, int k, bool isFilterValid)
    {
        // get reference to ItemDataStorage script
        PartDataStorage partData = dat.GetComponent<PartDataStorage>();

        // instantiate the button prefab
        tempButtonList = Instantiate(_partInfoPrefab);
        tempButtonList.transform.SetParent(_partsButtonParent.transform, false);

        // set up button text
        TextMeshProUGUI t = tempButtonList.GetComponentInChildren<TextMeshProUGUI>();
        t.text = partData.MaterialName + " " + partData.PartName;

        // set button data
        //tempButtonList.GetComponent<InventoryButton>().setIsNew(partData.IsNew);
        tempButtonList.GetComponent<InventoryButton>().setIsEnchanted(partData.IsHoldingEnchant);

        if (isFilterValid == true)
            tempButtonList.GetComponent<InventoryButton>().setAlpha(1f);
        else if (isFilterValid == false)
            tempButtonList.GetComponent<InventoryButton>().setAlpha(.6f);

        int j = _inventoryData.PartInventory.IndexOf(dat.gameObject);

        // add button to list
        InsertPartButton(tempButtonList, j);
    }
    /* Old code for partStorageSetup()
    // get reference to ItemDataStorage script
    PartDataStorage partData = part.GetComponent<PartDataStorage>();

    // instantiate the button prefab
    tempButtonList = Instantiate(_partInfoPrefab);
    tempButtonList.transform.SetParent(_partsButtonParent.transform, false);

    // set up button text
    TextMeshProUGUI t = tempButtonList.GetComponentInChildren<TextMeshProUGUI>();
    t.text = partData.MaterialName + " " + partData.PartName;

    // set button data
    //tempButtonList.GetComponent<InventoryButton>().setIsNew(partData.IsNew);
    tempButtonList.GetComponent<InventoryButton>().setIsEnchanted(partData.IsHoldingEnchant);

    // add button to list
    //InsertPartButton(tempButtonList, k);
    */

    public void setupMatInventory()
    {
        clearMatButtonList();
        _selectedMat = null;

        if (_currentMatFilter.Equals(_matsFilterData[0])) // if current filter is Default
        {
            int m = 0;
            foreach (GameObject mat in _inventoryData.MaterialInventory)
            {
                if (mat.GetComponent<MaterialDataStorage>() != null)
                {
                    MaterialDataStorage matStore = mat.GetComponent<MaterialDataStorage>();
                    // if level req is met
                    if (matStore.LevelRequirement <= _gameMaster.GetLevel)
                    {
                        matStorageSetup(matStore, m, true);
                        m++;
                    }
                }
            }
        }
        else if (_currentMatFilter.Equals(_matsFilterData[1]))
        {
            int m = 0;
            if (_craftControlRef.checkPartRecipe() != null)
            {
                foreach (GameObject mat in _inventoryData.MaterialInventory)
                {
                    MaterialDataStorage matStore = mat.GetComponent<MaterialDataStorage>();
                    if (_craftControlRef.checkPartRecipe().ValidMaterialData.Contains(matStore.MatDataRef))
                    {
                        if (matStore.LevelRequirement <= _gameMaster.GetLevel)
                        {
                            matStorageSetup(matStore, m, true);
                            m++;
                        }
                    }
                    else
                        holderList.Add(mat);
                }
                foreach (GameObject mat in holderList)
                {
                    if (mat != null)
                    {
                        MaterialDataStorage matStore = mat.GetComponent<MaterialDataStorage>();
                        if (matStore.LevelRequirement <= _gameMaster.GetLevel)
                        {
                            matStorageSetup(matStore, m, false);
                            m++;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("InventoryScript.setupMatInventory(): no receipe selected");
                foreach (GameObject mat in _inventoryData.MaterialInventory)
                {
                    if (mat.GetComponent<MaterialDataStorage>() != null)
                    {
                        MaterialDataStorage matStore = mat.GetComponent<MaterialDataStorage>();
                        // if level req is met
                        if (matStore.LevelRequirement <= _gameMaster.GetLevel)
                        {
                            matStorageSetup(matStore, m, true);
                            m++;
                        }
                    }
                }

            }
            holderList.Clear();
            setupList.Clear();
        }
        else
        {
            int m = 0;
            foreach (GameObject mat in _inventoryData.MaterialInventory)
            {
                MaterialDataStorage matStore = mat.GetComponent<MaterialDataStorage>();
                if (matStore.MatDataRef.ValidFilters.Contains(_currentMatFilter))
                {
                    // if level req is met
                    if (matStore.LevelRequirement <= _gameMaster.GetLevel)
                    {
                        Debug.Log("InventoryScript.setupMatInventory(): " + matStore.Material + ".Material has the filter: " + _currentMatFilter.FilterName);
                        matStorageSetup(matStore, m, true);
                        m++;
                    }
                }
                else
                    holderList.Add(mat);
            }
            foreach (GameObject mat in holderList)
            {
                if (mat != null)
                {
                    MaterialDataStorage matStore = mat.GetComponent<MaterialDataStorage>();
                    if (matStore.LevelRequirement <= _gameMaster.GetLevel)
                    {
                        matStorageSetup(matStore, m, false);
                        m++;
                    }
                }
            }

            holderList.Clear();
            setupList.Clear();
        }

        _descriptionText1.text = "";
        _descriptionText2.text = "";
    }

    private void matStorageSetup(MaterialDataStorage dat, int m, bool isFilterValid)
    {
        // instantiate the button prefab
        tempButtonList = Instantiate(_matInfoPrefab);
        tempButtonList.transform.SetParent(_matsButtonParent.transform, false);

        // set up the button text
        tempButtonList.GetComponentInChildren<MaterialButton>().setMatInfoText(dat);

        if (isFilterValid == true)
            tempButtonList.GetComponent<MaterialButton>().setAlpha(1f);
        else if (isFilterValid == false)
            tempButtonList.GetComponent<MaterialButton>().setAlpha(.6f);

        // set material
        tempButtonList.GetComponentInChildren<MaterialButton>().MaterialData = dat.MatDataRef;

        int j = _inventoryData.MaterialInventory.IndexOf(dat.gameObject);

        // add button to list
        InsertMatButton(tempButtonList, j);
    }
    /* Old code for matStorageSetup()
    // instantiate the button prefab
    tempButtonList = Instantiate(_matInfoPrefab);
    tempButtonList.transform.SetParent(_matsButtonParent.transform, false);

    // set up the button text
    tempButtonList.GetComponentInChildren<MaterialButton>().setMatInfoText(matStore);

    // set material
    tempButtonList.GetComponentInChildren<MaterialButton>().MaterialData = matStore.MatDataRef;

    // add button to list
    InsertMatButton(tempButtonList, m);
    */
    public void setupEnchantInventory()
    {
        setupEnchantInventory(false, 0);
    }
    public void setupEnchantInventory(bool isRemoving, int state)
    {
        clearEnchantButtonList();
        _selectedEnchant = null;

        int e = 0;
        if (_UIControlRef.CraftPartUIEnabled == true)
        {
            tempButtonList = Instantiate(_enchantCancelPrefab);
            tempButtonList.transform.SetParent(_enchButtonParent.transform, false);

            InsertEnchantButton(tempButtonList, -1);
        }

        foreach (GameObject ench in _inventoryData.EnchantInventory)
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
        _descriptionText1.text = "";
        _descriptionText2.text = "";
    }

    private ColorBlock ItemColorBlock;
    private ColorBlock PartColorBlock;
    private ColorBlock MaterialColorBlock;
    private ColorBlock EnchantColorBlock;
    /* old code
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
    */

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
        else { _descriptionText1.text = ""; _descriptionText2.text = ""; }
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
            else { _descriptionText1.text = ""; _descriptionText2.text = ""; }
        }
        else { _descriptionText1.text = ""; _descriptionText2.text = "";
        }
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
        itemDataStorageTemp.name = item.Part1.Material.Material + " " + item.ItemName;
        itemDataScriptRef.setItemName(item.ItemName);
        itemDataScriptRef.setTotalValue(item.TotalValue);
        itemDataScriptRef.ItemRecipeRef = item;
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
        itemDataScriptRef.ItemRecipeRef = _recipeBookRef.getItemRecipe(item.recipeName);
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
    /* old code
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
    private bool EnchantSlotEmpty(int index)
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
    */
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
        if (button.GetComponent<InventoryButton>() != null)
        {
            for (int i = 0; i < _enchantButtonList.Count; i++)
                if (_enchantButtonList[i] == button)
                {
                    button.GetComponent<InventoryButton>().setMyIndex(i);
                    break;
                }
            button.GetComponent<InventoryButton>().setEnchantIndex(l);
            return button.GetComponent<InventoryButton>().MyIndex;
        }
        else if (button.GetComponent<EnchantCancelButton>() != null)
        {
            for (int i = 0; i < _enchantButtonList.Count; i++)
                if (_enchantButtonList[i] == button)
                {
                    button.GetComponent<EnchantCancelButton>().setMyIndex(i);
                    break;
                }
            button.GetComponent<EnchantCancelButton>().setEnchantIndex(l);
            return button.GetComponent<EnchantCancelButton>().MyIndex;
        }
        return -1;
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

        setupPartInventory();
    }
    public void RemoveMatAmount(MaterialDataStorage mat, int amount)
    {
        mat.RemoveMat(amount);
    }

    public void RemoveQuestItems()
    {
        for (int i = _inventoryData.ItemInventory.Count - 1; i >= 0; i--)
        {
            if (_inventoryData.ItemInventory[i] != null)
                if (_inventoryData.ItemInventory[i].GetComponent<ItemDataStorage>().IsForQuest == true)
                {
                    Debug.Log(_inventoryData.ItemInventory[i].GetComponent<ItemDataStorage>().ItemName + " is for quest!");
                    _inventoryData.removeItem(_inventoryData.ItemInventory[i]);
                }
        }
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
        //Debug.Log("InventoryScript.setSelectedItem(int i) - i=" + i + " i corelates to: " + _inventoryData.ItemInventory[i].name);
        if (i != -1)
        {
            //Debug.Log("InventoryScript.setSelectedItem(int i) - i=" + i + " i corelates to: " + _inventoryData.ItemInventory[i].name);
            _selectedItem = _inventoryData.ItemInventory[i];
            _selectedPart = null;
            _selectedMat = null;
            _selectedEnchant = null;
            if (_selectedItem.GetComponent<ItemDataStorage>().IsForQuest == false)
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
            _selectedItem = null;
            _selectedMat = null;
            _selectedEnchant = null;
            returnSeletedPart();
            //Debug.Log("Selected part is: " + _selectedPart.name + " at index: " + i);
        }
        else
            Debug.Log("example button selected");
    }
    public void setSelectedMat(MaterialData data)
    {
        if (data != null)
        {
            _selectedMat = data;
            _selectedItem = null;
            _selectedPart = null;
            _selectedEnchant = null;
            returnSelectedMat();
        }
        else
            Debug.LogWarning("No mat input!");
    }
    public void setSelectedMat(int i)
    {
        if (i != -1)
        {
            _selectedMat = _inventoryData.MaterialInventory[i].GetComponent<MaterialDataStorage>().MatDataRef;
            _selectedItem = null;
            _selectedPart = null;
            _selectedEnchant = null;
        }
        else
            Debug.Log("example button selected");
    }
    public void setSelectedEnchant(int i)
    {
        if (i != -1)
        {
            _selectedEnchant = _inventoryData.EnchantInventory[i];
            _selectedItem = null;
            _selectedPart = null;
            _selectedMat = null;
            returnSelectedEnchant();
        }
        else if (i == -1)
        {
            _selectedEnchant = null;
            _selectedPart = null;
            _selectedMat = null;
            _selectedEnchant = null;
            returnSelectedEnchant();
        }
        else
            Debug.Log("example button selected");
    }

    public void selectRandomItem()
    {
        int r = Random.Range(0, _inventoryData.ItemInventory.Count - 1);
        setSelectedItem(r);
    }

    public GameObject getSelectedItem()
    {
        Debug.Log("_selectedItem: " + _selectedItem.name);
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
    /* old code
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
    */
    public void returnSelectedItem()
    {
        //Debug.Log("_selectedItem: " + _selectedItem.name);
        // for quest item crafting
        if (_UIControlRef.ShopCraftUIEnabled == true)
        {
            if (_craftControlRef.checkQuestRecipe() != null)
            {
                // check 'part 1'
                questPart1();
                // check 'part 2'
                questPart2();
                // check 'part 3'
                questPart3();
            }
            else
            {
                Debug.Log("selectedItem: " + _selectedItem.name);
                if (_craftControlRef.anyRecipeSelected() == false && _UIControlRef.ShopEcoUIEnabled == false && _UIControlRef.ShopDisUIEnabled == false)
                {
                    _gameMaster.loadDisassembleMenu();
                    _UIControlRef.ShopEcoUIEnabled = false;
                    _UIControlRef.ShopDisUIEnabled = true;
                    _UIControlRef.ShopCraftUIEnabled = false;
                }
                Debug.Log("selectedItem: " + _selectedItem.name);
                GameObject.FindGameObjectWithTag("GameMaster").GetComponent<SellItemControl>().selectItem(_selectedItem);
                GameObject.FindGameObjectWithTag("GameMaster").GetComponent<DisassembleItemControl>().selectItem(_selectedItem);
            }
        }
        // for selling in shop
        else if (_UIControlRef.ShopUIEnabled == true)
        {
            //Debug.Log("shop ui enabled");
            if (_UIControlRef.ShopEcoUIEnabled == false && _UIControlRef.ShopDisUIEnabled == false)
            {
                //Debug.Log("shop eco and shop disassemble not enabled");
                _UIControlRef.ShopEcoUIEnabled = false;
                _UIControlRef.ShopDisUIEnabled = true;
                _UIControlRef.ShopCraftUIEnabled = false;
            }
            Debug.Log("selectedItem: " + _selectedItem.name);
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<SellItemControl>().selectItem(_selectedItem);
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<DisassembleItemControl>().selectItem(_selectedItem);
        }
        // for selling in market
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
    /*
    public void selectRandomItem()
    {
        _removingStatus = removingItemStatusEnum.RemovingToSell;
        this.gameObject.GetComponent<InventoryData>().getRandomItem();

        returnSelectedItem();
    }
    */
    [SerializeField] private int partLastFilled = 0;
    public void returnSeletedPart()
    {
        bool phb = false;
        if (_craftControlRef.anyItemRecipeSelected() == true)
        {
            Debug.Log("Selected part is valid for recipe!");
            //Debug.LogWarning("TODO setup code for selecting parts");
            foreach (PartData part1ref in _craftControlRef.checkItemRecipe().ValidParts1)
                if (_selectedPart.GetComponent<PartDataStorage>().RecipeData == part1ref
                    && (partLastFilled == 0 || partLastFilled == 2 || partLastFilled == 3 || _craftControlRef.Part1Set() == false || _craftControlRef.AllPartsSet() == true)
                    && phb == false)
                {
                    Debug.Log("selected part matches recipe part 1");
                    _craftControlRef.SelectPart1();
                    partLastFilled = 1;
                    phb = true;
                }

            foreach (PartData part2ref in _craftControlRef.checkItemRecipe().ValidParts2)
                if (_selectedPart.GetComponent<PartDataStorage>().RecipeData == part2ref
                    && (partLastFilled == 0 || partLastFilled == 1 || partLastFilled == 3 || _craftControlRef.Part2Set() == false || _craftControlRef.AllPartsSet() == true)
                    && phb == false)
                {
                    Debug.Log("selected part matches recipe part 2");
                    _craftControlRef.SelectPart2();
                    partLastFilled = 2;
                    phb = true;
                }

            foreach (PartData part3ref in _craftControlRef.checkItemRecipe().ValidParts3)
                if (_selectedPart.GetComponent<PartDataStorage>().RecipeData == part3ref
                    && (partLastFilled == 0 || partLastFilled == 1 || partLastFilled == 2 || _craftControlRef.Part3Set() == false || _craftControlRef.AllPartsSet() == true)
                    && phb == false)
                {
                    Debug.Log("selected part matches recipe part 3");
                    _craftControlRef.SelectPart3();
                    partLastFilled = 3;
                    phb = true;
                }
        }
        else if (_craftControlRef.anyQuestRecipeSelected() == true)
        {
            Debug.Log("Selected part is valid for quest recipe!");
            if (_selectedPart.GetComponent<PartDataStorage>().RecipeData == _craftControlRef.checkQuestRecipe().ItemPart1
                && (partLastFilled == 0 || partLastFilled == 2 || partLastFilled == 3 || _craftControlRef.Part1Set() == false || _craftControlRef.AllPartsSet() == true)
                && phb == false)
            {
                Debug.Log("selected part matches recipe part 1");
                _craftControlRef.SelectQPart1();
                partLastFilled = 1;
                phb = true;
            }
            if (_selectedPart.GetComponent<PartDataStorage>().RecipeData == _craftControlRef.checkQuestRecipe().ItemPart2
                && (partLastFilled == 0 || partLastFilled == 1 || partLastFilled == 3 || _craftControlRef.Part2Set() == false || _craftControlRef.AllPartsSet() == true)
                && phb == false)
            {
                Debug.Log("selected part matches recipe part 2");
                _craftControlRef.SelectQPart2();
                partLastFilled = 2;
                phb = true;
            }
            if (_selectedPart.GetComponent<PartDataStorage>().RecipeData == _craftControlRef.checkQuestRecipe().ItemPart3
                && (partLastFilled == 0 || partLastFilled == 1 || partLastFilled == 2 || _craftControlRef.Part3Set() == false || _craftControlRef.AllPartsSet() == true)
                && phb == false)
            {
                Debug.Log("selected part matches recipe part 3");
                _craftControlRef.SelectQPart3();
                partLastFilled = 3;
                phb = true;
            }
        }
        else if (_UIControlRef.ShopDisUIEnabled == true)
        {
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<DisassembleItemControl>().selectPart();
        }
        phb = false;
    }

    private void questPart1()
    {
        Debug.Log("checking quest part 1");
        bool phb = false;
        if (_selectedItem != null)
        {
            ItemData item = getItemData();
            if (item == _craftControlRef.checkQuestRecipe().ItemPart1 && phb == false)
            {
                Debug.LogWarning("selected item matches recipe part 1");
                _craftControlRef.SelectQPart1();

                phb = true;
            }
        }
        else if (_selectedPart != null)
        {
            if (_selectedPart.GetComponent<PartDataStorage>().RecipeData == _craftControlRef.checkQuestRecipe().ItemPart1 && phb == false)
            {
                Debug.LogWarning("selected item matches recipe part 1");
                _craftControlRef.SelectQPart1();

                phb = true;
            }
        }
    }
    private void questPart2()
    {
        Debug.Log("checking quest part 2");
        bool phb = false;
        if (_selectedItem != null)
        {
            ItemData item = getItemData();
            if (item == _craftControlRef.checkQuestRecipe().ItemPart2 && phb == false)
            {
                Debug.LogWarning("selected item matches recipe part 2");
                _craftControlRef.SelectQPart2();

                phb = true;
            }
        }
        else if (_selectedPart != null)
        {
            if (_selectedPart.GetComponent<PartDataStorage>().RecipeData == _craftControlRef.checkQuestRecipe().ItemPart2 && phb == false)
            {
                Debug.LogWarning("selected item matches recipe part 2");
                _craftControlRef.SelectQPart2();

                phb = true;
            }
        }
    }
    private void questPart3()
    {
        Debug.Log("checking quest part 3");

        bool phb = false;
        if (_selectedItem != null)
        {
            ItemData item = getItemData();
            if (item == _craftControlRef.checkQuestRecipe().ItemPart3 && phb == false)
            {
                Debug.LogWarning("selected item matches recipe part 3");
                _craftControlRef.SelectQPart3();

                phb = true;
            }
        }
        else if (_selectedPart != null)
        {
            if (_selectedPart.GetComponent<PartDataStorage>().RecipeData == _craftControlRef.checkQuestRecipe().ItemPart3 && phb == false)
            {
                Debug.LogWarning("selected item matches recipe part 3");
                _craftControlRef.SelectQPart3();

                phb = true;
            }
        }
    }

    private ItemData getItemData()
    {
        foreach (ItemData item in GameObject.FindGameObjectWithTag("ItemData").GetComponent<Item>().getItemDataRef())
            if (item.ItemName == _selectedItem.GetComponent<ItemDataStorage>().ItemName)
                return item;

        return null;
    }

    public void nextItemFilter()
    {
        int i = _itemsFilterData.IndexOf(_currentItemFilter);
        if ((i + 1) > _itemsFilterData.Count - 1)
            i = 0;
        else
            i += 1;
        _currentItemFilter = _itemsFilterData[i];

        setupItemInventory();
    }
    public void nextPartFilter()
    {
        int p = _partsFilterData.IndexOf(_currentPartFilter);
        if ((p + 1) > _partsFilterData.Count - 1)
            p = 0;
        else
            p += 1;
        _currentPartFilter = _partsFilterData[p];

        setupPartInventory();
    }
    public void nextMatFilter()
    {
        int m = _matsFilterData.IndexOf(_currentMatFilter);
        if ((m + 1) > _matsFilterData.Count - 1)
            m = 0;
        else
            m += 1;
        _currentMatFilter = _matsFilterData[m];

        setupMatInventory();
    }
    public void nextEnchFilter()
    {
        int e = _enchFilterData.IndexOf(_currentEnchFilter);
        if ((e + 1) > _enchFilterData.Count - 1)
            e = 0;
        else
            e += 1;
        _currentEnchFilter = _enchFilterData[e];

    }

    public void returnSelectedMat()
    {
        //if (_removingStatus == removingItemStatusEnum.RemovingToCraftMat)
        if (_craftControlRef.anyPartRecipeSelected() == true)
        {
            GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().SelectMat();

        }
    }

    public void returnSelectedEnchant()
    {
        //if (_removingStatus == removingItemStatusEnum.RemovingToEnchant)
        if (_craftControlRef.anyPartRecipeSelected() == true)
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

    public int ItemInvSize { get => _inventoryData.ItemInventory.Count; }
    public List<FilterData> ItemFilterData { get => _itemsFilterData; }
    public FilterData CurrentItemFilter { get => _currentItemFilter; set => _currentItemFilter = value; }
    public List<FilterData> PartFilterData { get => _partsFilterData; }
    public FilterData CurrentPartFilter { get => _currentPartFilter; set => _currentPartFilter = value; }
    public List<FilterData> MatFilterData { get => _matsFilterData; }
    public FilterData CurrentMatFilter { get => _currentMatFilter; set => _currentMatFilter = value; }
    public List<FilterData> EnchFilterData { get => _enchFilterData; }
    public FilterData CurrentEnchFilter { get => _currentEnchFilter; set => _currentEnchFilter = value; }
}
