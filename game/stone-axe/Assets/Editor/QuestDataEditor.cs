using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestData)), CanEditMultipleObjects]
public class QuestDataEditor : Editor
{
    public SerializedProperty
        questName,
        questType,
        questDiscription,
        questStages,
        reqItem,
        reqMat,
        reqQItem,
        reqCount,
        questComplete,
        nextQuest;

    private void OnEnable()
    {
        questName = serializedObject.FindProperty("_questName");
        questType = serializedObject.FindProperty("_questType");
        questDiscription = serializedObject.FindProperty("_questDiscription");
        questStages = serializedObject.FindProperty("_questStages");

        reqItem = serializedObject.FindProperty("_requiredItem");
        reqMat = serializedObject.FindProperty("_requiredMaterial");
        reqQItem = serializedObject.FindProperty("_requiredQuestItem");
        reqCount = serializedObject.FindProperty("_requiredCount");
        questComplete = serializedObject.FindProperty("_storyQuestComplete");
        nextQuest = serializedObject.FindProperty("_nextQuest");
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

        EditorGUILayout.PropertyField(questName);
        EditorGUILayout.PropertyField(questType);

        QuestData.questTypeEnum PHQuestType = (QuestData.questTypeEnum)questType.enumValueIndex;

        EditorGUILayout.PropertyField(questDiscription);
        switch(PHQuestType)
        {
            case QuestData.questTypeEnum.Not_Set:
                break;

            case QuestData.questTypeEnum.OCC_Item:
                EditorGUILayout.PropertyField(reqItem, new GUIContent("Required Item"));
                reqMat.objectReferenceValue = null;
                reqQItem.objectReferenceValue = null;
                reqCount.intValue = 0;
                questComplete.boolValue = false;
                nextQuest.objectReferenceValue = null;
                break;

            case QuestData.questTypeEnum.OCC_QuestItem:
                reqItem.objectReferenceValue = null;
                reqMat.objectReferenceValue = null;
                EditorGUILayout.PropertyField(reqQItem, new GUIContent("Required Item"));
                reqCount.intValue = 0;
                questComplete.boolValue = false;
                nextQuest.objectReferenceValue = null;
                break;

            case QuestData.questTypeEnum.OD_Material:
                reqItem.objectReferenceValue = null;
                EditorGUILayout.PropertyField(reqMat, new GUIContent("Required Material"));
                reqQItem.objectReferenceValue = null;
                EditorGUILayout.PropertyField(reqCount, new GUIContent("Material Count"));
                questComplete.boolValue = false;
                nextQuest.objectReferenceValue = null;
                break;

            case QuestData.questTypeEnum.OCC_TotalCrafted:
                EditorGUILayout.PropertyField(reqItem, new GUIContent("Required Item"));
                reqMat.objectReferenceValue = null;
                reqQItem.objectReferenceValue = null;
                EditorGUILayout.PropertyField(reqCount, new GUIContent("Item Count"));
                questComplete.boolValue = false;
                nextQuest.objectReferenceValue = null;
                break;

            case QuestData.questTypeEnum.Tutorial:
                reqItem.objectReferenceValue = null;
                reqMat.objectReferenceValue = null;
                reqQItem.objectReferenceValue = null;
                reqCount.intValue = 0;
                EditorGUILayout.PropertyField(questStages);
                EditorGUILayout.PropertyField(questComplete, new GUIContent("This Quest Complete"));
                EditorGUILayout.PropertyField(nextQuest);
                break;

            case QuestData.questTypeEnum.Story:
                reqItem.objectReferenceValue = null;
                reqMat.objectReferenceValue = null;
                reqQItem.objectReferenceValue = null;
                reqCount.intValue = 0;
                EditorGUILayout.PropertyField(questStages);
                EditorGUILayout.PropertyField(questComplete, new GUIContent("This Quest Complete"));
                EditorGUILayout.PropertyField(nextQuest);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
