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
        questEvent;

    private void OnEnable()
    {
        questStageType = serializedObject.FindProperty("_questStageType");
        dialogueLine = serializedObject.FindProperty("_dialogueLine");
        dialogueSpeaker = serializedObject.FindProperty("_speaker");
        questEvent = serializedObject.FindProperty("_questEvent");
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
                break;

            case QuestStage.questStageEnum.Dialogue:
                questEvent.enumValueIndex = 0;
                EditorGUILayout.PropertyField(dialogueSpeaker);
                EditorGUILayout.PropertyField(dialogueLine);
                break;

            case QuestStage.questStageEnum.Craft_Item:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                break;

            case QuestStage.questStageEnum.Sell_Item:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                break;

            case QuestStage.questStageEnum.Buy_Item:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                break;

            case QuestStage.questStageEnum.Disassemble_Item:
                questEvent.enumValueIndex = 0;
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                break;

            case QuestStage.questStageEnum.Force_Event:
                EditorGUILayout.PropertyField(questEvent);
                dialogueSpeaker.stringValue = "";
                dialogueLine.stringValue = "";
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
