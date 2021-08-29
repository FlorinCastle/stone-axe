using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] Item _itemScriptReference;
    [SerializeField] Part _partScriptReference;
    [SerializeField] Material _materialScriptReference;
    [Header("UI")]
    [SerializeField] Dropdown _itemDropdown;
    [SerializeField] Dropdown _part1Select;
    [SerializeField] Dropdown _part1Material;
    [SerializeField] Dropdown _part2Select;
    [SerializeField] Dropdown _part2Material;
    [SerializeField] Dropdown _part3Select;
    [SerializeField] Dropdown _part3Material;


    [SerializeField] List<string> itemDropOptions;
    [SerializeField] List<string> part1PartDropOptions;
    [SerializeField] List<string> part2PartDropOptions;
    [SerializeField] List<string> part3PartDropOptions;
    [SerializeField] List<string> part1MaterialDropOptions;
    [SerializeField] List<string> part2MaterialDropOptions;
    [SerializeField] List<string> part3MaterialDropOptions;
    private void Awake()
    {
        // item dropdown
        _itemDropdown.ClearOptions();
        List<ItemData> itemData = _itemScriptReference.getItemDataRef();
        itemDropOptions.Add("Choose Item"); // store this somewhere to allow for localizaion??
        foreach (ItemData item in itemData)
            itemDropOptions.Add(item.ItemName);
        _itemDropdown.AddOptions(itemDropOptions);

        // clear dropdowns
        clearPartOptions();
        clearMaterialOptions();

    }


    public void checkItemSelection()
    {
        // Debug.Log(_itemDropdown.options[_itemDropdown.value].text);
        if (_itemDropdown.options[_itemDropdown.value].text == "Choose Item")
        {
            // Debug.Log(_itemDropdown.options[_itemDropdown.value].text);
            _part1Select.interactable = false;
            _part1Material.interactable = false;
            _part2Select.interactable = false;
            _part2Material.interactable = false;
            _part3Select.interactable = false;
            _part3Material.interactable = false;

            // clear dropdowns
            clearPartOptions();
            clearMaterialOptions();
        }
        else
        {
            //Debug.Log(_itemDropdown.options[_itemDropdown.value].text);
            
            //  TODO: set up code so only certain parts can be selected
            //  TODO: set up code so only certain materials can be selected

            _part1Select.value = 0;
            _part1Select.interactable = true;
            _part1Material.value = 0;
            _part1Material.interactable = true;
            _part2Select.value = 0;
            _part2Select.interactable = true;
            _part2Material.value = 0;
            _part2Material.interactable = true;
            _part3Select.value = 0;
            _part3Select.interactable = true;
            _part3Material.value = 0;
            _part3Material.interactable = true;
        }
    }

    private void clearPartOptions()
    {
        //Debug.Log("Clear Part Options has been called");
        // part 1 options
        _part1Select.ClearOptions();
        part1PartDropOptions.Add("Choose Item");
        _part1Select.AddOptions(part1PartDropOptions);

        // part 2 options
        _part2Select.ClearOptions();
        part2PartDropOptions.Add("Choose Item");
        _part2Select.AddOptions(part2PartDropOptions);

        // part 3 options
        _part3Select.ClearOptions();
        part3PartDropOptions.Add("Choose Item");
        _part3Select.AddOptions(part3PartDropOptions);

    }

    private void clearMaterialOptions()
    {
        //Debug.Log("Clear Material Options has been called");
        // material 1 options
        _part1Material.ClearOptions();
        part1MaterialDropOptions.Add("Choose Item");
        _part1Material.AddOptions(part1MaterialDropOptions);

        // material 2 options
        _part2Material.ClearOptions();
        part2MaterialDropOptions.Add("Choose Item");
        _part2Material.AddOptions(part2MaterialDropOptions);

        // material 3 options
        _part3Material.ClearOptions();
        part3MaterialDropOptions.Add("Choose Item");
        _part3Material.AddOptions(part3MaterialDropOptions);

    }

}
