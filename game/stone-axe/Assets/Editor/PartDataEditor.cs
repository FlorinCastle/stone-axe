using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PartData)), CanEditMultipleObjects]
public class PartDataEditor : Editor
{
    public SerializedProperty
        partName,
        partLevelReq,
        material,
        validMatData,
        validMatTypes,
        unitsReq,
        baseCost,
        baseStr,
        baseDex,
        baseInt,
        filters;

    private void OnEnable()
    {
        partName = serializedObject.FindProperty("_partName");
        partLevelReq = serializedObject.FindProperty("_partLevelReq");
        material = serializedObject.FindProperty("_material");
        validMatData = serializedObject.FindProperty("_validMaterials");
        validMatTypes = serializedObject.FindProperty("_validMaterialTypes");
        unitsReq = serializedObject.FindProperty("_unitsOfMaterialNeeded");
        baseCost = serializedObject.FindProperty("_baseCost");
        baseStr = serializedObject.FindProperty("_baseStrenght");
        baseDex = serializedObject.FindProperty("_baseDextarity");
        baseInt = serializedObject.FindProperty("_baseIntelligence");
        filters = serializedObject.FindProperty("_filters");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(partName);
        EditorGUILayout.PropertyField(partLevelReq);
        EditorGUILayout.PropertyField(material);
        EditorGUILayout.PropertyField(validMatData);
        EditorGUILayout.PropertyField(validMatTypes);
        EditorGUILayout.PropertyField(unitsReq);
        EditorGUILayout.PropertyField(baseCost);
        EditorGUILayout.PropertyField(baseStr);
        EditorGUILayout.PropertyField(baseDex);
        EditorGUILayout.PropertyField(baseInt);
        EditorGUILayout.PropertyField(filters);
        if (material.objectReferenceValue == null && validMatData.arraySize > 0)
            material.objectReferenceValue = validMatData.GetArrayElementAtIndex(0).objectReferenceValue;

        serializedObject.ApplyModifiedProperties();
    }
}
