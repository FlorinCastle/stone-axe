using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(MaterialData)), CanEditMultipleObjects]
public class MaterialDataEditor : Editor
{
    public SerializedProperty
        baseMatType,
        subMatType,
        subMetalType,
        subClothType,
        matName,
        levelRequirement,
        baseCostPerUnit,
        validFilters,
        addedStr,
        addedDex,
        addedInt;

    private void OnEnable()
    {
        baseMatType = serializedObject.FindProperty("_materialType");
        subMatType = serializedObject.FindProperty("_subMatType");

        matName = serializedObject.FindProperty("_materialName");
        levelRequirement = serializedObject.FindProperty("_levelRequirement");
        baseCostPerUnit = serializedObject.FindProperty("_baseCostPerUnit");
        validFilters = serializedObject.FindProperty("_validFilters");
        addedStr = serializedObject.FindProperty("_addedStrength");
        addedDex = serializedObject.FindProperty("_addedDextarity");
        addedInt = serializedObject.FindProperty("_addedIntelligence");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(matName, new GUIContent("Material Name"));

        EditorGUILayout.PropertyField(baseMatType);

        MaterialData.matTypeEnum matType = (MaterialData.matTypeEnum)baseMatType.enumValueIndex;

        switch(matType)
        {
            case MaterialData.matTypeEnum.Metal:
                EditorGUILayout.PropertyField(subMatType, new GUIContent("Sub-Mat Type"));
                break;

            case MaterialData.matTypeEnum.Cloth:
                EditorGUILayout.PropertyField(subMatType, new GUIContent("Sub-Mat Type"));
                break;
        }

        EditorGUILayout.PropertyField(levelRequirement, new GUIContent("Level Requirement"));
        //EditorGUILayout.PropertyField(quantity, new GUIContent("Quantity"));
        EditorGUILayout.PropertyField(baseCostPerUnit, new GUIContent("Base Cost Per Unit"));
        EditorGUILayout.PropertyField(validFilters, new GUIContent("Valid Filters"));
        EditorGUILayout.LabelField("Material Stats", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(addedStr, new GUIContent("Added Strength"));
        EditorGUILayout.PropertyField(addedDex, new GUIContent("Added Dextarity"));
        EditorGUILayout.PropertyField(addedInt, new GUIContent("Added Intelligence"));

        serializedObject.ApplyModifiedProperties();
    }
    
    /*
    private List<MaterialData.subMatTypeEnum> MetalFilter()
    {
        List<MaterialData.subMatTypeEnum> displayList;

        return displayList;
    }
    */
}
