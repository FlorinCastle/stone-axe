using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemData)), CanEditMultipleObjects]
public class ItemDataEditor : Editor
{
    public SerializedProperty
        itemName,
        levelReq,
        validPart1,
        part1,
        validPart2,
        part2,
        validPart3,
        part3,
        baseCost,
        baseStr,
        baseDex,
        baseInt,
        isEnch;

    private void OnEnable()
    {
        itemName = serializedObject.FindProperty("_itemName");
        levelReq = serializedObject.FindProperty("_levelRequirement");
        validPart1 = serializedObject.FindProperty("_validPart1");
        part1 = serializedObject.FindProperty("_part1");
        validPart2 = serializedObject.FindProperty("_validPart2");
        part2 = serializedObject.FindProperty("_part2");
        validPart3 = serializedObject.FindProperty("_validPart3");
        part3 = serializedObject.FindProperty("_part3");
        baseCost = serializedObject.FindProperty("_baseCost");
        baseStr = serializedObject.FindProperty("_baseStrenght");
        baseDex = serializedObject.FindProperty("_baseDextarity");
        baseInt = serializedObject.FindProperty("_baseIntelligence");
        isEnch = serializedObject.FindProperty("_isEnchanted");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(itemName);
        EditorGUILayout.PropertyField(levelReq, new GUIContent("Level Requirement"));

        EditorGUILayout.PropertyField(validPart1);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(part1);
        if (part1.objectReferenceValue == null && validPart1.arraySize > 0)
            part1 = validPart1.GetArrayElementAtIndex(0);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(validPart2);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(part2);
        if (part2.objectReferenceValue == null && validPart2.arraySize > 0)
            part2 = validPart2.GetArrayElementAtIndex(0);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(validPart3);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(part3);
        if (part3.objectReferenceValue == null && validPart3.arraySize > 0)
            part3 = validPart3.GetArrayElementAtIndex(0);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(baseCost);
        EditorGUILayout.PropertyField(baseStr, new GUIContent("Base Strength"));
        EditorGUILayout.PropertyField(baseDex, new GUIContent("Base Dextarity"));
        EditorGUILayout.PropertyField(baseInt, new GUIContent("Base Intelligence"));
        EditorGUILayout.PropertyField(isEnch, new GUIContent("Is Enchanted"));

        serializedObject.ApplyModifiedProperties();
    }
}