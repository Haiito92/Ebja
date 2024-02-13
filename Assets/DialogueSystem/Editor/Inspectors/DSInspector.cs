using System;
using System.Collections.Generic;
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

        DSDialogueContainerSO dialogueContainer = (DSDialogueContainerSO) _dialogueContainerProperty.objectReferenceValue;

        if(dialogueContainer == null )
        {
            StopDrawing("Select a Dialogue Container to see the rest of the Inspector.");

            return;
        }

        DrawFiltersArea();

        if(_groupedDialoguesProperty.boolValue)
        {
            List<string> dialogueGroupNames = dialogueContainer.GetDialogueGroupNames();

            if(dialogueGroupNames.Count == 0 )
            {
                StopDrawing("There are no Dialogue Groups in this Dialogue Container.");

                return;
            }

            DrawDialogueGroupArea(dialogueContainer, dialogueGroupNames);
        }

        DrawDialogueArea();

        serializedObject.ApplyModifiedProperties();
    }

    #region Draw Methods
    private void DrawDialogueContainerArea()
    {
        DSInspectorUtility.DrawHeader("Dialogue Container");

        _dialogueContainerProperty.DrawPropertyField();

        DSInspectorUtility.DrawSpace();
    }

    private void DrawFiltersArea()
    {
        DSInspectorUtility.DrawHeader("Filters");

        _groupedDialoguesProperty.DrawPropertyField();
        _startingDialoguesOnlyProperty.DrawPropertyField();

        DSInspectorUtility.DrawSpace();
    }

    private void DrawDialogueGroupArea(DSDialogueContainerSO dialogueContainer, List<string> dialogueGroupNames)
    {
        DSInspectorUtility.DrawHeader("Dialogue Group");

        int oldSelectedDialogueGroupIndex = _selectedDialogueGroupIndexProperty.intValue;

        DSDialogueGroupSO oldDialogueGroup = (DSDialogueGroupSO)_dialogueGroupProperty.objectReferenceValue;

        bool isOldDialogueGroupNull = oldDialogueGroup == null;

        string oldDialogueGroupName = isOldDialogueGroupNull ? "" : oldDialogueGroup.GroupName;

        UpdateIndexOnNamesListUpdate(dialogueGroupNames, _selectedDialogueGroupIndexProperty, oldSelectedDialogueGroupIndex, oldDialogueGroupName, isOldDialogueGroupNull);

        _selectedDialogueGroupIndexProperty.intValue = DSInspectorUtility.DrawPopup("Dialogue Group", _selectedDialogueGroupIndexProperty, dialogueGroupNames.ToArray());

        string selectedDialogueGroupName = dialogueGroupNames[_selectedDialogueGroupIndexProperty.intValue];

        DSDialogueGroupSO selectedDialogueGroup = DSIOUtility.LoadAsset<DSDialogueGroupSO>($"Assets/DialogueSystem/Runtime/Dialogues/{dialogueContainer.FileName}/Groups/{selectedDialogueGroupName}", selectedDialogueGroupName);

        _dialogueGroupProperty.objectReferenceValue = selectedDialogueGroup;

        _dialogueGroupProperty.DrawPropertyField();

        DSInspectorUtility.DrawSpace();
    }

    private void DrawDialogueArea()
    {
        DSInspectorUtility.DrawHeader("Dialogue");

        _selectedDialogueIndexProperty.intValue = DSInspectorUtility.DrawPopup("Dialogue", _selectedDialogueIndexProperty, new string[] { });

        _dialogueProperty.DrawPropertyField();
    }

    private void StopDrawing(string reason)
    {
        DSInspectorUtility.DrawHelpBox(reason);

        serializedObject.ApplyModifiedProperties();
    }
    #endregion

    #region Index Methods
    private void UpdateIndexOnNamesListUpdate(List<string> optionNames, SerializedProperty indexProperty, int oldSelectedPropertyIndex, string oldPropertyName, bool isOldPropertyNull)
    {
        if (isOldPropertyNull)
        {
            indexProperty.intValue = 0;

            return;
        }

        bool oldIndexIsOutOfBoundsOfNamesLiastCount = oldSelectedPropertyIndex > optionNames.Count - 1;
        bool oldNameIsDifferentThanSelectedName = oldIndexIsOutOfBoundsOfNamesLiastCount || oldPropertyName != optionNames[oldSelectedPropertyIndex];

        if (oldNameIsDifferentThanSelectedName)
        {
            if (optionNames.Contains(oldPropertyName))
            {
                indexProperty.intValue = optionNames.IndexOf(oldPropertyName);
            }
            else
            {
                indexProperty.intValue = 0;
            }
        }

    }
    #endregion
}
