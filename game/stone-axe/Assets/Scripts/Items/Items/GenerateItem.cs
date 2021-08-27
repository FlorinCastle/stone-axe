using UnityEngine;
using UnityEngine.UI;

public class GenerateItem : MonoBehaviour
{
    [SerializeField] Text itemText;
    [SerializeField] Item itemScript;

    [SerializeField] InventoryScriptableObject inventoryStorage;

    public void GenerateRandomItem()
    {


        itemText.text = itemScript.chooseItem();
    }
}
