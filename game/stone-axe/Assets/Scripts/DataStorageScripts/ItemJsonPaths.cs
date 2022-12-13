using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ItemJsonPaths", menuName = "ScriptableObjects/ItemJsonPaths", order = 70)]
public class ItemJsonPaths : ScriptableObject
{
    [SerializeField]
    private List<TextAsset> _itemJsons;
    [SerializeField]
    private List<string> _jsonPaths;

    public string getItemJsonPath(TextAsset json)
    {
        return _jsonPaths[_itemJsons.IndexOf(json)];
    }
}

/*[CustomEditor(typeof(ItemJsonPaths))]
public class ItemJsonPathsEditor : Editor
{
    public SerializedProperty
        itemJsons,
        jsonPaths;

    private void OnEnable()
    {
        itemJsons = serializedObject.FindProperty("_itemJsons");

        jsonPaths.ClearArray();

        jsonPaths = serializedObject.FindProperty("_jsonPaths");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(itemJsons);

        EditorGUILayout.PropertyField(jsonPaths);

        serializedObject.ApplyModifiedProperties();
    }
} */