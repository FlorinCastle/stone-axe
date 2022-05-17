using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameMaster))]
public class GameMasterEditor : Editor
{
    public SerializedProperty
        playerName,
        shopName,
        currentCurrency,
        totalExperience,
        level,
        currentSkillPoints,
        shopLevel,
        marketLevel,
        shopSubUI,
        marketSubUI,
        toShopButton,
        toMarketButton,
        invScript,
        saveTrackerParent,
        saveHolderPrefab,
        skipTutorialQuestsToggle,
        saveTrackerScripts,
        saveGameList,
        selectedSave,
        mostRecentSave;

    private void OnEnable()
    {
        playerName = serializedObject.FindProperty("_playerName");
        shopName = serializedObject.FindProperty("_shopName");
        currentCurrency = serializedObject.FindProperty("_currentCurrency");
        totalExperience = serializedObject.FindProperty("_totalExperience");
        level = serializedObject.FindProperty("_level");
        currentSkillPoints = serializedObject.FindProperty("_currentSkillPoints");
        shopLevel = serializedObject.FindProperty("_shopLevel");
        marketLevel = serializedObject.FindProperty("_marketLevel");
        shopSubUI = serializedObject.FindProperty("_shopSubUI");
        marketSubUI = serializedObject.FindProperty("_marketSubUI");
        toShopButton = serializedObject.FindProperty("_toShopButton");
        toMarketButton = serializedObject.FindProperty("_toMarketButton");
        invScript = serializedObject.FindProperty("_invScript");
        saveTrackerParent = serializedObject.FindProperty("saveTrackerParent");
        saveHolderPrefab = serializedObject.FindProperty("_saveHolderPrefab");
        skipTutorialQuestsToggle = serializedObject.FindProperty("_skipTutorialQuestsToggle");
        saveTrackerScripts = serializedObject.FindProperty("_saveTrackerScripts");
        saveGameList = serializedObject.FindProperty("_saveGameList");
        selectedSave = serializedObject.FindProperty("_selectedSave");
        mostRecentSave = serializedObject.FindProperty("_mostRecentSave");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(playerName);
        EditorGUILayout.PropertyField(shopName);
        EditorGUILayout.PropertyField(currentCurrency);
        EditorGUILayout.PropertyField(totalExperience);

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(level);
        EditorGUILayout.PropertyField(currentSkillPoints);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(shopLevel);
        EditorGUILayout.PropertyField(marketLevel);
        EditorGUILayout.PropertyField(shopSubUI);
        EditorGUILayout.PropertyField(marketSubUI);
        EditorGUILayout.PropertyField(toShopButton);
        EditorGUILayout.PropertyField(toMarketButton);
        EditorGUILayout.PropertyField(invScript);
        EditorGUILayout.PropertyField(saveTrackerParent);
        EditorGUILayout.PropertyField(saveHolderPrefab);
        EditorGUILayout.PropertyField(skipTutorialQuestsToggle);


        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(saveTrackerScripts);
        EditorGUILayout.PropertyField(saveGameList);
        EditorGUILayout.PropertyField(selectedSave);
        EditorGUILayout.PropertyField(mostRecentSave);
        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
