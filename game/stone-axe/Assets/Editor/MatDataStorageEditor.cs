using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaterialDataStorage)), CanEditMultipleObjects]
public class MatDataStorageEditor : Editor
{
    public SerializedProperty
        matDataRef,
        matName,
        matType,
        subMatType,
        currAmount,
        levelReq,
        baseCost,
        addedStr,
        addedDex,
        addedInt;

    private void OnEnable()
    {
        matDataRef = serializedObject.FindProperty("_matDataRef");
        matName = serializedObject.FindProperty("_materialName");
        matType = serializedObject.FindProperty("_materialType");
        subMatType = serializedObject.FindProperty("_subMatType");
        currAmount = serializedObject.FindProperty("_quantity");
        levelReq = serializedObject.FindProperty("_levelRequirement");
        baseCost = serializedObject.FindProperty("_baseCostPerUnit");
        addedStr = serializedObject.FindProperty("_addedStrength");
        addedDex = serializedObject.FindProperty("_addedDextarity");
        addedInt = serializedObject.FindProperty("_addedIntelligence");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(matDataRef, new GUIContent("Material Data Reference"));
        if (matDataRef.objectReferenceValue != null)
        {
            MaterialData matRef = (MaterialData)matDataRef.objectReferenceValue;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(matName, new GUIContent("Material Name"));
            matName.stringValue = matRef.Material;

            EditorGUILayout.PropertyField(matType, new GUIContent("Material Type"));
            if (matRef.MaterialType == "Metal")
                matType.enumValueIndex = 0;
            else if (matRef.MaterialType == "Wood")
                matType.enumValueIndex = 1;
            else if (matRef.MaterialType == "Cloth")
                matType.enumValueIndex = 2;
            else if (matRef.MaterialType == "Gemstone")
                matType.enumValueIndex = 3;

            // code for sub material type
            MaterialDataStorage.matTypeEnum MaterialType = (MaterialDataStorage.matTypeEnum)matType.enumValueIndex;
            switch (MaterialType)
            {
                case MaterialDataStorage.matTypeEnum.Metal:
                    EditorGUILayout.PropertyField(subMatType, new GUIContent("Sub-Mat Type"));
                    if (matRef.SubMaterialType == "Basic")
                        subMatType.enumValueIndex = 0;
                    else if (matRef.SubMaterialType == "MagicMetal")
                        subMatType.enumValueIndex = 1;
                    break;

                case MaterialDataStorage.matTypeEnum.Wood:
                    subMatType.enumValueIndex = 0;
                    break;

                case MaterialDataStorage.matTypeEnum.Cloth:
                    EditorGUILayout.PropertyField(subMatType, new GUIContent("Sub-Mat Type"));
                    if (matRef.SubMaterialType == "Basic")
                        subMatType.enumValueIndex = 0;
                    else if (matRef.SubMaterialType == "Cloth")
                        subMatType.enumValueIndex = 2;
                    else if (matRef.SubMaterialType == "Leather")
                        subMatType.enumValueIndex = 3;
                    break;

                case MaterialDataStorage.matTypeEnum.Gemstone:
                    subMatType.enumValueIndex = 0;
                    break;

            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(currAmount, new GUIContent("Current Amount"));

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(levelReq, new GUIContent("Level Requirement"));
            levelReq.intValue = matRef.LevelRequirement;

            EditorGUILayout.PropertyField(baseCost);
            baseCost.intValue = matRef.BaseCostPerUnit;

            EditorGUILayout.PropertyField(addedStr, new GUIContent("Added Strength"));
            addedStr.intValue = matRef.AddedStrength;

            EditorGUILayout.PropertyField(addedDex, new GUIContent("Added Dextarity"));
            addedDex.intValue = matRef.AddedDextarity;

            EditorGUILayout.PropertyField(addedInt, new GUIContent("Added Intellegence"));
            addedInt.intValue = matRef.AddedIntelligence;
            EditorGUI.EndDisabledGroup();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
