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
        EditorGUILayout.PropertyField(qItemPart1, new GUIContent("Part 1"));
        EditorGUILayout.PropertyField(qItemPart2, new GUIContent("Part 2"));
        EditorGUILayout.PropertyField(qItemPart3, new GUIContent("Part 3"));
        EditorGUILayout.PropertyField(baseCost, new GUIContent("Base Cost"));
        EditorGUILayout.PropertyField(baseStr, new GUIContent("Base Strength"));
        EditorGUILayout.PropertyField(baseDex, new GUIContent("Base Dextarity"));
        EditorGUILayout.PropertyField(baseInt, new GUIContent("Base Intelligence"));

        serializedObject.ApplyModifiedProperties();
    }
}
