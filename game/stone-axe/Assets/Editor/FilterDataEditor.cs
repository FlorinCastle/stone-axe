using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(FilterData)), CanEditMultipleObjects]
public class FilterDataEditor : Editor
{
    public SerializedProperty
        filterName,
        hasSubFilters,
        subFilters;

    private void OnEnable()
    {
        filterName = serializedObject.FindProperty("_filterName");
        hasSubFilters = serializedObject.FindProperty("_hasSubFilters");
        subFilters = serializedObject.FindProperty("_subFilters");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(filterName);
        EditorGUILayout.PropertyField(hasSubFilters);

        if (hasSubFilters.boolValue == true)
        {
            EditorGUILayout.PropertyField(subFilters);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
