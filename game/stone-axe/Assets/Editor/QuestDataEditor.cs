using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestData)), CanEditMultipleObjects]
public class QuestDataEditor : Editor
{
    public SerializedProperty
        questName,
        questType,
        reqLevel,
        questDiscription,
        questStages,
        reqItem,
        reqMat,
        reqQItem,
        reqCount,
        questComplete,
        nextQuest,
        questUnlocks,
        rewardCurrency,
        rewardEXP;

    private void OnEnable()
    {
        questName = serializedObject.FindProperty("_questName");
        questType = serializedObject.FindProperty("_questType");
        reqLevel = serializedObject.FindProperty("_requiredPlayerLevel");
        questDiscription = serializedObject.FindProperty("_questDiscription");
        questStages = serializedObject.FindProperty("_questStages");

        reqItem = serializedObject.FindProperty("_requiredItem");
        reqMat = serializedObject.FindProperty("_requiredMaterial");
        reqQItem = serializedObject.FindProperty("_requiredQuestItem");
        reqCount = serializedObject.FindProperty("_requiredCount");
        questComplete = serializedObject.FindProperty("_storyQuestComplete");
        nextQuest = serializedObject.FindProperty("_nextQuest");
        questUnlocks = serializedObject.FindProperty("_unlocksQuests");

        rewardCurrency = serializedObject.FindProperty("_rewardCurrency");
        rewardEXP = serializedObject.FindProperty("_rewardEXP");
    }

    /* Disregard this
    reqItem.objectReferenceValue = null;
    reqMat.objectReferenceValue = null;
    reqQItem.objectReferenceValue = null;
    reqCount.intValue = 0;
    questComplete.boolValue = false;
    nextQuest.objectReferenceValue = null;
    */
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //EditorStyles.textArea.wordWrap = true;

        ItemData itemRef = (ItemData)reqItem.objectReferenceValue;
        MaterialData matRef = (MaterialData)reqMat.objectReferenceValue;
        QuestItemData qItemRef = (QuestItemData)reqQItem.objectReferenceValue;

        EditorGUILayout.PropertyField(questName);
        EditorGUILayout.PropertyField(questType);
        EditorGUILayout.PropertyField(reqLevel, new GUIContent("Unlock Level"));

        QuestData.questTypeEnum PHQuestType = (QuestData.questTypeEnum)questType.enumValueIndex;

        EditorGUILayout.PropertyField(questDiscription);
        switch(PHQuestType)
        {
            case QuestData.questTypeEnum.Not_Set:
                break;

            case QuestData.questTypeEnum.OCC_Item:
                EditorGUILayout.PropertyField(reqItem, new GUIContent("Required Item"));
                if (itemRef != null) reqLevel.intValue = itemRef.ItemLevel;
                else reqLevel.intValue = 0;
                reqMat.objectReferenceValue = null;
                reqQItem.objectReferenceValue = null;
                reqCount.intValue = 0;
                questComplete.boolValue = false;
                nextQuest.objectReferenceValue = null;
                questUnlocks.ClearArray();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Quest Rewards", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(rewardEXP, new GUIContent("EXP Reward"));
                EditorGUILayout.PropertyField(rewardCurrency, new GUIContent("Currency Reward"));
                rewardCurrency.intValue = ((int)(itemRef.TotalValue * (1.5f)));
                break;

            case QuestData.questTypeEnum.OCC_QuestItem:
                reqItem.objectReferenceValue = null;
                reqMat.objectReferenceValue = null;
                EditorGUILayout.PropertyField(reqQItem, new GUIContent("Required Item"));
                if (qItemRef != null) reqLevel.intValue = qItemRef.QuestItemUnlockLevel;
                else reqLevel.intValue = 0;
                reqCount.intValue = 0;
                questComplete.boolValue = false;
                nextQuest.objectReferenceValue = null;
                questUnlocks.ClearArray();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Quest Rewards", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(rewardEXP, new GUIContent("EXP Reward"));
                EditorGUILayout.PropertyField(rewardCurrency, new GUIContent("Currency Reward"));
                if (qItemRef != null && rewardCurrency.intValue <= 1000)
                    rewardCurrency.intValue = qItemRef.BaseCost + 1000;
                break;

            case QuestData.questTypeEnum.OD_Material:
                reqItem.objectReferenceValue = null;
                EditorGUILayout.PropertyField(reqMat, new GUIContent("Required Material"));
                if (matRef != null) reqLevel.intValue = matRef.LevelRequirement;
                else reqLevel.intValue = 0;
                reqQItem.objectReferenceValue = null;
                EditorGUILayout.PropertyField(reqCount, new GUIContent("Material Count"));
                questComplete.boolValue = false;
                nextQuest.objectReferenceValue = null;
                questUnlocks.ClearArray();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Quest Rewards", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(rewardEXP, new GUIContent("EXP Reward"));
                EditorGUILayout.PropertyField(rewardCurrency, new GUIContent("Currency Reward"));
                rewardCurrency.intValue = matRef.BaseCostPerUnit * (reqCount.intValue + 3);
                break;

            case QuestData.questTypeEnum.OCC_TotalCrafted:
                EditorGUILayout.PropertyField(reqItem, new GUIContent("Required Item"));
                reqLevel.intValue = itemRef.ItemLevel;
                reqMat.objectReferenceValue = null;
                reqQItem.objectReferenceValue = null;
                EditorGUILayout.PropertyField(reqCount, new GUIContent("Item Count"));
                questComplete.boolValue = false;
                nextQuest.objectReferenceValue = null;
                questUnlocks.ClearArray();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Quest Rewards", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(rewardEXP, new GUIContent("EXP Reward"));
                EditorGUILayout.PropertyField(rewardCurrency, new GUIContent("Currency Reward"));
                if (reqCount.intValue == 0)
                    reqCount.intValue = 3;

                rewardCurrency.intValue = itemRef.TotalValue * (reqCount.intValue + 2);
                break;

            case QuestData.questTypeEnum.Tutorial:
                reqItem.objectReferenceValue = null;
                reqMat.objectReferenceValue = null;
                reqQItem.objectReferenceValue = null;
                reqCount.intValue = 0;
                EditorGUILayout.PropertyField(questStages);
                EditorGUILayout.PropertyField(questComplete, new GUIContent("This Quest Complete"));
                EditorGUILayout.PropertyField(nextQuest);
                EditorGUILayout.PropertyField(questUnlocks);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Quest Rewards", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(rewardEXP, new GUIContent("EXP Reward"));
                EditorGUILayout.PropertyField(rewardCurrency, new GUIContent("Currency Reward"));
                break;

            case QuestData.questTypeEnum.Story:
                reqItem.objectReferenceValue = null;
                reqMat.objectReferenceValue = null;
                reqQItem.objectReferenceValue = null;
                reqCount.intValue = 0;
                EditorGUILayout.PropertyField(questStages);
                EditorGUILayout.PropertyField(questComplete, new GUIContent("This Quest Complete"));
                EditorGUILayout.PropertyField(nextQuest);
                EditorGUILayout.PropertyField(questUnlocks, new GUIContent("Quest Unlocks"));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Quest Rewards", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(rewardEXP, new GUIContent("EXP Reward"));
                EditorGUILayout.PropertyField(rewardCurrency, new GUIContent("Currency Reward"));
                break;
        }
        /*
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Quest Rewards", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(rewardEXP, new GUIContent("EXP Reward"));
        EditorGUILayout.PropertyField(rewardCurrency, new GUIContent("Currency Reward"));
        */
        serializedObject.ApplyModifiedProperties();
    }
}
