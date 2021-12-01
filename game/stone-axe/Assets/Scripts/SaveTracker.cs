using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveTracker : MonoBehaviour
{
    [SerializeField] private string _saveRef;
    [SerializeField] private int _indexRef;
    [Header("UI Elements")]
    [SerializeField] private Text _shopNameText;
    [SerializeField] private Text _dayTimeText;

    public string SaveReference { get => _saveRef; set => _saveRef = value; }
}
