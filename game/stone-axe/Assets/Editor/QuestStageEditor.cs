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
        dialogueSpeaker;

    private void OnEnable()
    {
        questStageType = serializedObject.FindProperty("_questStageType");
        dialogueLine = serializedObject.FindProperty("_dialogueLine");
        dialogueSpeaker = serializedObject.FindProperty("_speaker");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(questStageType);

        QuestStage.questStageEnum PHQuestStage = (QuestStage.questStageEnum)questStageType.enumValueIndex;

        switch(PHQuestStage)
        {
            case QuestStage.questStageEnum.Not_Set:
                dialogueLine.stringValue = "";
                break;

            case QuestStage.questStageEnum.Dialogue:
                EditorGUILayout.PropertyField(dialogueSpeaker);
                EditorGUILayout.PropertyField(dialogueLine);
                break;

            case QuestStage.questStageEnum.Craft_Item:
                dialogueLine.stringValue = "";
                break;

            case QuestStage.questStageEnum.Sell_Item:
                dialogueLine.stringValue = "";
                break;

            case QuestStage.questStageEnum.Buy_Item:
                dialogueLine.stringValue = "";
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
