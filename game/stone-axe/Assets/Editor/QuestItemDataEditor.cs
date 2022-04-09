using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestItemData)), CanEditMultipleObjects]
public class QuestItemDataEditor : Editor
{
    public SerializedProperty
        qItemName,
        qItemLore,
        qItemUnlockLevel,
        qItemPart1,
        qItemPart2,
        qItemPart3,
        baseCost,
        baseStr,
        baseDex,
        baseInt;

    private void OnEnable()
    {
        qItemName = serializedObject.FindProperty("_qItemName");
        qItemLore = serializedObject.FindProperty("_qItemLore");
        qItemUnlockLevel = serializedObject.FindProperty("_qItemUnlockLevel");
        qItemPart1 = serializedObject.FindProperty("_qItemPart1");
        qItemPart2 = serializedObject.FindProperty("_qItemPart2");
        qItemPart3 = serializedObject.FindProperty("_qItemPart3");
        baseCost = serializedObject.FindProperty("_baseCost");
        baseStr = serializedObject.FindProperty("_baseStrength");
        baseDex = serializedObject.FindProperty("_baseDextarity");
        baseInt = serializedObject.FindProperty("_baseIntelligence");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorStyles.textField.wordWrap = true;

        EditorGUILayout.PropertyField(qItemName, new GUIContent("Quest Item Name"));
        EditorGUILayout.PropertyField(qItemLore, new GUIContent("Quest Item Lore"));
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(qItemUnlockLevel, new GUIContent("Unlock Level"));
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(qItemPart1, new GUIContent("Part 1"));
        EditorGUILayout.PropertyField(qItemPart2, new GUIContent("Part 2"));
        EditorGUILayout.PropertyField(qItemPart3, new GUIContent("Part 3"));
        EditorGUILayout.PropertyField(baseCost, new GUIContent("Base Cost"));
        EditorGUILayout.PropertyField(baseStr, new GUIContent("Base Strength"));
        EditorGUILayout.PropertyField(baseDex, new GUIContent("Base Dextarity"));
        EditorGUILayout.PropertyField(baseInt, new GUIContent("Base Intelligence"));

        // Calculating level from parts
        ItemData part1refItem = null;
        PartData part1refpart = null;
        if (qItemPart1.objectReferenceValue.GetType().ToString() == "ItemData")
            part1refItem = (ItemData)qItemPart1.objectReferenceValue;
        else if (qItemPart1.objectReferenceValue.GetType().ToString() == "PartData")
            part1refpart = (PartData)qItemPart1.objectReferenceValue;

        ItemData part2refItem = null;
        PartData part2refpart = null;
        if (qItemPart2.objectReferenceValue.GetType().ToString() == "ItemData")
            part2refItem = (ItemData)qItemPart2.objectReferenceValue;
        else if (qItemPart2.objectReferenceValue.GetType().ToString() == "PartData")
            part2refpart = (PartData)qItemPart2.objectReferenceValue;

        ItemData part3refItem = null;
        PartData part3refpart = null;
        if (qItemPart3.objectReferenceValue.GetType().ToString() == "ItemData")
            part3refItem = (ItemData)qItemPart3.objectReferenceValue;
        else if (qItemPart3.objectReferenceValue.GetType().ToString() == "PartData")
            part3refpart = (PartData)qItemPart3.objectReferenceValue;

        if (part1refItem != null && qItemUnlockLevel.intValue <= part1refItem.ItemLevel)
            qItemUnlockLevel.intValue = part1refItem.ItemLevel;
        if (part1refpart != null && qItemUnlockLevel.intValue <= part1refpart.PartLevelReq)
            qItemUnlockLevel.intValue = part1refpart.PartLevelReq;

        if (part2refItem != null && qItemUnlockLevel.intValue <= part2refItem.ItemLevel)
            qItemUnlockLevel.intValue = part2refItem.ItemLevel;
        if (part1refpart != null && qItemUnlockLevel.intValue <= part2refpart.PartLevelReq)
            qItemUnlockLevel.intValue = part2refpart.PartLevelReq;

        if (part3refItem != null && qItemUnlockLevel.intValue <= part3refItem.ItemLevel)
            qItemUnlockLevel.intValue = part3refItem.ItemLevel;
        if (part1refpart != null && qItemUnlockLevel.intValue <= part3refpart.PartLevelReq)
            qItemUnlockLevel.intValue = part3refpart.PartLevelReq;

        // calculating stats from parts



        serializedObject.ApplyModifiedProperties();
    }
}
