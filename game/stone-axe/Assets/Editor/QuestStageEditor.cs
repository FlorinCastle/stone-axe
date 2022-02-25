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
        currencyValue;

    private void OnEnable()
    {
        questStageType = serializedObject.FindProperty("_questStageType");
        dialogueLine = serializedObject.FindProperty("_dialogueLine");
        dialogueSpeaker = serializedObject.FindProperty("_speaker");
        questEvent = serializedObject.FindProperty("_questEvent");
        itemToGet = serializedObject.FindProperty("_itemToGet");
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
