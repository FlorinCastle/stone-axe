using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestItemData", menuName = "ScriptableObjects/QuestItemData", order = 59)]
public class QuestItemData : ScriptableObject
{
    [SerializeField]
    private string _qItemName;
    [TextArea(1,10)]
    [SerializeField]
    private string _qItemLore;
    [SerializeField]
    private ScriptableObject _qItemPart1;
    [SerializeField]
    private ScriptableObject _qItemPart2;
    [SerializeField]
    private ScriptableObject _qItemPart3;
    /*
    public ItemData _qItemItem1;
    public PartData _qItemPart1;
    public ItemData _qItemItem2;
    public PartData _qItemPart2;
    public ItemData _qItemItem3;
    public PartData _qItemPart3;
    */
    [SerializeField]
    private int _baseCost;
    [SerializeField]
    private int _baseStrength;
    [SerializeField]
    private int _baseDextarity;
    [SerializeField]
    private int _baseIntelligence;


    public string QuestItemName { get => _qItemName; }
    public string QuestItemLore { get => _qItemLore; }

    public ScriptableObject ItemPart1 { get => _qItemPart1; }
    public ScriptableObject ItemPart2 { get => _qItemPart2; }
    public ScriptableObject ItemPart3 { get => _qItemPart3; }

    public int BaseCost { get => _baseCost; }
    public int BaseStrenght { get => _baseStrength; }
    public int BaseDextarity { get => _baseDextarity; }
    public int BaseIntelligence { get => _baseIntelligence; }
}
