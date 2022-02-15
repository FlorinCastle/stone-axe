using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelUpData)), CanEditMultipleObjects]
public class LevelUpDataEditor : Editor
{
    public SerializedProperty
        levelNumber,
        levelName,
        itemRecipes,
        partReceipes,
        materials;

    private void OnEnable()
    {
        levelNumber = serializedObject.FindProperty("_levelNumber");
        levelName = serializedObject.FindProperty("_levelName");
        itemRecipes = serializedObject.FindProperty("_itemRecipes");
        partReceipes = serializedObject.FindProperty("_partRecipes");
        materials = serializedObject.FindProperty("_materials");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(levelNumber);
        if (levelNumber.intValue > 0)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(levelName);
            levelName.stringValue = "Level " + levelNumber.intValue;
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(itemRecipes, new GUIContent("Item Recipe Unlocks"));
            EditorGUILayout.PropertyField(partReceipes, new GUIContent("Part Recipe Unlocks"));
            EditorGUILayout.PropertyField(materials, new GUIContent("Material Unlocks"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
