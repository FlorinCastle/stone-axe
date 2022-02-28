using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestStage)), CanEditMultipleObjects]
public class QuestStageEditor : Editor
{
    public SerializedProperty
        questStageType,
        dialogueLine,
        dialogueSpeaker,
        questEvent,
        itemToGet,
        part1Mat,
        part2Mat,
        part3Mat,
        currencyValue;

    private void OnEnable()
    {
        questStageType = serializedObject.FindProperty("_questStageType");
        dialogueLine = serializedObject.FindProperty("_dialogueLine");
        dialogueSpeaker = serializedObject.FindProperty("_speaker");
        questEvent = serializedObject.FindProperty("_questEvent");
        itemToGet = serializedObject.FindProperty("_itemToGet");
        part1Mat = serializedObject.FindProperty("_part1Mat");
        part2Mat = serializedObject.FindProperty("_part2Mat");
        part3Mat = serializedObject.FindProperty("_part3Mat");
        currencyValue = serializedObject.FindProperty("_currencyValue");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(questStageType);

        QuestStage.questStageEnum PHQuestStage = (QuestStage.questStageEnum)questStageType.enumValueIndex;

        switch(PHQuestStage)
        {
            case QuestStage.questStageEnum.Not_Set:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                currencyValue.intValue = 0;
                itemToGet = null;
                break;

            case QuestStage.questStageEnum.Dialogue:
                questEvent.enumValueIndex = 0;
                EditorGUILayout.PropertyField(dialogueSpeaker);
                EditorGUILayout.PropertyField(dialogueLine);
                currencyValue.intValue = 0;
                itemToGet = null;
                break;

            case QuestStage.questStageEnum.Craft_Item:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                currencyValue.intValue = 0;
                itemToGet = null;
                break;

            case QuestStage.questStageEnum.Sell_Item:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                currencyValue.intValue = 0;
                itemToGet = null;
                break;

            case QuestStage.questStageEnum.Buy_Item:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                currencyValue.intValue = 0;
                itemToGet = null;
                break;

            case QuestStage.questStageEnum.Disassemble_Item:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                currencyValue.intValue = 0;
                itemToGet = null;
                break;

            case QuestStage.questStageEnum.Force_Event:
                EditorGUILayout.PropertyField(questEvent);
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                QuestStage.questEvent PHEvent = (QuestStage.questEvent)questEvent.enumValueIndex;
                switch(PHEvent)
                {
                    case QuestStage.questEvent.Get_Item:
                        currencyValue.intValue = 0;
                        EditorGUILayout.PropertyField(itemToGet);
                        if (itemToGet.objectReferenceValue != null)
                        {
                            ItemData item = (ItemData)itemToGet.objectReferenceValue;

                            EditorGUILayout.PropertyField(part1Mat, new GUIContent("Part 1 - " + item.Part1.PartName));
                            EditorGUILayout.PropertyField(part2Mat, new GUIContent("Part 2 - " + item.Part2.PartName));
                            EditorGUILayout.PropertyField(part3Mat, new GUIContent("Part 3 - " + item.Part3.PartName));

                        }

                        break;
                    case QuestStage.questEvent.Get_Currency:
                        EditorGUILayout.PropertyField(currencyValue);
                        itemToGet = null;
                        break;
                }
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
/*
 * 
                        if (itemToGet != null)
                        {
                            ItemData item = (ItemData)itemToGet.objectReferenceValue;
                            if (item.Part1.ValidMaterialData.Contains((MaterialData)part1Mat.objectReferenceValue))
                                EditorGUILayout.PropertyField(part1Mat);
                            else
                                part1Mat = null;
                        }
*/
