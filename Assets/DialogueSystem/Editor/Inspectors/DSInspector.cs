using System;
using UnityEditor;

[CustomEditor(typeof(DSDialogue))]
public class DSInspector : Editor
{
    /* Dialogue Scriptable Objects */
    private SerializedProperty _dialogueContainerProperty;
    private SerializedProperty _dialogueGroupProperty;
    private SerializedProperty _dialogueProperty;

    /* Filters */

    private SerializedProperty _groupedDialoguesProperty;
    private SerializedProperty _startingDialoguesOnlyProperty;

    /* Indexes */

    private SerializedProperty _selectedDialogueGroupIndexProperty;
    private SerializedProperty _selectedDialogueIndexProperty;

    private void OnEnable()
    {
        _dialogueContainerProperty = serializedObject.FindProperty("_dialogueContainer");
        _dialogueGroupProperty = serializedObject.FindProperty("_dialogueGroup");
        _dialogueProperty = serializedObject.FindProperty("_dialogue");

        _groupedDialoguesProperty = serializedObject.FindProperty("_groupedDialogues");
        _startingDialoguesOnlyProperty = serializedObject.FindProperty("_startingDialoguesOnly");

        _selectedDialogueGroupIndexProperty = serializedObject.FindProperty("_selectedDialogueGroupIndex");
        _selectedDialogueIndexProperty = serializedObject.FindProperty("_selectedDialogueIndex");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDialogueContainerArea();

        DrawFiltersArea();

        DrawDialogueGroupArea();

        DrawDialogueArea();

        serializedObject.ApplyModifiedProperties();
    }

    #region Draw Methods
    private void DrawDialogueContainerArea()
    {
        EditorGUILayout.LabelField("Dialogue Container", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(_dialogueContainerProperty);

        EditorGUILayout.Space(4);
    }

    private void DrawFiltersArea()
    {
        EditorGUILayout.LabelField("Filters", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(_groupedDialoguesProperty);
        EditorGUILayout.PropertyField(_startingDialoguesOnlyProperty);

        EditorGUILayout.Space(4);
    }

    private void DrawDialogueGroupArea()
    {
        EditorGUILayout.LabelField("Dialogue Group", EditorStyles.boldLabel);

        _selectedDialogueGroupIndexProperty.intValue = EditorGUILayout.Popup("Dialogue Group", _selectedDialogueGroupIndexProperty.intValue, new string[] { });

        EditorGUILayout.PropertyField(_dialogueGroupProperty);

        EditorGUILayout.Space(4);
    }

    private void DrawDialogueArea()
    {
        EditorGUILayout.LabelField("Dialogue", EditorStyles.boldLabel);

        _selectedDialogueIndexProperty.intValue = EditorGUILayout.Popup("Dialogue", _selectedDialogueIndexProperty.intValue, new string[] { });

        EditorGUILayout.PropertyField(_dialogueProperty);
    }
    #endregion
}
