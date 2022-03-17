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
        currencyValue,
        npcRef;

    private void OnEnable()
    {
        questStageType = serializedObject.FindProperty("_questStageType");
        dialogueLine = serializedObject.FindProperty("_dialogueLine");
        dialogueSpeaker = serializedObject.FindProperty("_speaker");
        questEvent = serializedObject.FindProperty("_questEvent");
        itemToGet = serializedObject.FindProperty("_item");
        part1Mat = serializedObject.FindProperty("_part1Mat");
        part2Mat = serializedObject.FindProperty("_part2Mat");
        part3Mat = serializedObject.FindProperty("_part3Mat");
        currencyValue = serializedObject.FindProperty("_currencyValue");
        npcRef = serializedObject.FindProperty("_npcRef");
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
                itemToGet.objectReferenceValue = null;
                npcRef.objectReferenceValue = null;
                break;

            case QuestStage.questStageEnum.Dialogue:
                questEvent.enumValueIndex = 0;
                EditorGUILayout.PropertyField(dialogueSpeaker);
                EditorGUILayout.PropertyField(dialogueLine);
                currencyValue.intValue = 0;
                itemToGet.objectReferenceValue = null;
                npcRef.objectReferenceValue = null;
                break;

            case QuestStage.questStageEnum.Craft_Item:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                currencyValue.intValue = 0;
                //itemToGet = null;
                EditorGUILayout.PropertyField(itemToGet, new GUIContent("Item to Craft"));
                npcRef.objectReferenceValue = null;
                break;

            case QuestStage.questStageEnum.Sell_Item:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                currencyValue.intValue = 0;
                itemToGet.objectReferenceValue = null;
                npcRef.objectReferenceValue = null;
                break;

            case QuestStage.questStageEnum.Buy_Item:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                currencyValue.intValue = 0;
                itemToGet.objectReferenceValue = null;
                npcRef.objectReferenceValue = null;
                break;

            case QuestStage.questStageEnum.Disassemble_Item:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                currencyValue.intValue = 0;
                itemToGet.objectReferenceValue = null;
                npcRef.objectReferenceValue = null;
                break;

            case QuestStage.questStageEnum.Have_Currency:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                EditorGUILayout.PropertyField(currencyValue, new GUIContent("Currency to Earn"));
                //currencyValue.intValue = 0;
                itemToGet.objectReferenceValue = null;
                npcRef.objectReferenceValue = null;
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
                        EditorGUILayout.PropertyField(itemToGet, new GUIContent("Item to Get"));
                        if (itemToGet.objectReferenceValue != null)
                        {
                            ItemData item = (ItemData)itemToGet.objectReferenceValue;

                            EditorGUILayout.PropertyField(part1Mat, new GUIContent("Part 1 - " + item.Part1.PartName));
                            EditorGUILayout.PropertyField(part2Mat, new GUIContent("Part 2 - " + item.Part2.PartName));
                            EditorGUILayout.PropertyField(part3Mat, new GUIContent("Part 3 - " + item.Part3.PartName));

                        }
                        npcRef.objectReferenceValue = null;
                        break;

                    case QuestStage.questEvent.Force_For_Sale:
                        currencyValue.intValue = 0;
                        EditorGUILayout.PropertyField(itemToGet, new GUIContent("Item to Force for Sale"));
                        if (itemToGet.objectReferenceValue != null)
                        {
                            ItemData item = (ItemData)itemToGet.objectReferenceValue;

                            EditorGUILayout.PropertyField(part1Mat, new GUIContent("Part 1 - " + item.Part1.PartName));
                            EditorGUILayout.PropertyField(part2Mat, new GUIContent("Part 2 - " + item.Part2.PartName));
                            EditorGUILayout.PropertyField(part3Mat, new GUIContent("Part 3 - " + item.Part3.PartName));

                        }
                        npcRef.objectReferenceValue = null;
                        break;


                    case QuestStage.questEvent.Get_Currency:
                        EditorGUILayout.PropertyField(currencyValue, new GUIContent("Currency to Get"));
                        itemToGet.objectReferenceValue = null;
                        npcRef.objectReferenceValue = null;
                        break;

                    case QuestStage.questEvent.Remove_Currency:
                        EditorGUILayout.PropertyField(currencyValue, new GUIContent("Currency to Remove"));
                        itemToGet.objectReferenceValue = null;
                        npcRef.objectReferenceValue = null;
                        break;

                    case QuestStage.questEvent.Summon_NPC:
                        currencyValue.intValue = 0;
                        EditorGUILayout.PropertyField(npcRef, new GUIContent("NPC Prefab Reference"));
                        itemToGet.objectReferenceValue = null;
                        break;

                    case QuestStage.questEvent.Dismiss_Quest_NPC:
                        currencyValue.intValue = 0;
                        npcRef.objectReferenceValue = null;
                        itemToGet.objectReferenceValue = null;
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
