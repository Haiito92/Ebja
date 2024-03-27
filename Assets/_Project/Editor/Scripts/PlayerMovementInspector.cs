using UnityEditor;

namespace _Project.Editor.Scripts
{
    using Utilities;
    using Runtime.Scripts.Adventure.Characters.PlayerActions;
    
    [CustomEditor(typeof(PlayerMovement))]
    public class PlayerMovementInspector : UnityEditor.Editor
    {
        /* Filters */
        SerializedProperty _hideSetupProperty;

        SerializedProperty _selectedActionIndexProperty;
        string[] _actionOptions = { "Walk", "Jump" };

        /* Properties */

        //Setup
        SerializedProperty _rbProperty;

        //Walk
        SerializedProperty _walkProperty;
        SerializedProperty _speedProperty;
        SerializedProperty _accelerationTimeProperty;
        SerializedProperty _decelerationTimeProperty;

        SerializedProperty OnStartWalkingEventProperty;
        SerializedProperty OnStopWalkingEventProperty;

        //Jump
        SerializedProperty _jumpProperty;
        SerializedProperty _jumpForceProperty;
        SerializedProperty _groundCheckProperty;
        SerializedProperty _groundCheckRadiusProperty;
        SerializedProperty _whatIsGroundProperty;

        private void OnEnable()
        {
            _hideSetupProperty = serializedObject.FindProperty("_hideSetup");
            _selectedActionIndexProperty = serializedObject.FindProperty("_selectedActionIndex");

            _rbProperty = serializedObject.FindProperty("_rb");

            _walkProperty = serializedObject.FindProperty("_walk");
            _speedProperty = serializedObject.FindProperty("_speed");
            _accelerationTimeProperty = serializedObject.FindProperty("_accelerationTime");
            _decelerationTimeProperty = serializedObject.FindProperty("_decelerationTime");

            OnStartWalkingEventProperty = serializedObject.FindProperty("OnStartWalkingEvent");
            OnStopWalkingEventProperty = serializedObject.FindProperty("OnStopWalkingEvent");

            _jumpProperty = serializedObject.FindProperty("_jump");
            _jumpForceProperty = serializedObject.FindProperty("_jumpForce");
            _groundCheckProperty = serializedObject.FindProperty("_groundCheck");
            _groundCheckRadiusProperty = serializedObject.FindProperty("_groundCheckRadius");
            _whatIsGroundProperty = serializedObject.FindProperty("_whatIsGround");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_hideSetupProperty, EditorStyles.boldFont);
            EditorGUILayout.Space(5);

            if(!_hideSetupProperty.boolValue)
            {
                DrawSetup();
            }

            DrawFilterArea();

            if (_actionOptions[_selectedActionIndexProperty.intValue] == "Walk")
            {
                DrawWalkArea();
            }
            else if (_actionOptions[_selectedActionIndexProperty.intValue] == "Jump")
            {
                DrawJumpArea();
            }


            serializedObject.ApplyModifiedProperties();
        }

        #region Draw Methods
        void DrawSetup()
        {
            EditorGUILayout.LabelField("Setup", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            EditorGUILayout.PropertyField(_rbProperty);
            EditorGUILayout.Space(5);

            CustomInspectorUtility.DrawProperties(_walkProperty, _jumpProperty);
            EditorGUILayout.Space(5);

            EditorGUILayout.Space(10);
        }

        void DrawFilterArea()
        {
            EditorGUILayout.LabelField("Action Filter", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            EditorGUILayout.HelpBox("Use this filter to choose the action you want to modify.", MessageType.Info);

            _selectedActionIndexProperty.intValue = EditorGUILayout.Popup("Action", _selectedActionIndexProperty.intValue, _actionOptions);

            EditorGUILayout.Space(10);
        }

        void DrawWalkArea()
        {
            EditorGUILayout.LabelField("Walk", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            CustomInspectorUtility.DrawProperties(_speedProperty, _accelerationTimeProperty, _decelerationTimeProperty);
            EditorGUILayout.Space(5);

            CustomInspectorUtility.DrawProperties(OnStartWalkingEventProperty, OnStopWalkingEventProperty);
        }

        void DrawJumpArea()
        {
            EditorGUILayout.LabelField("Jump", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            CustomInspectorUtility.DrawProperties(_jumpForceProperty, _groundCheckProperty, _groundCheckRadiusProperty, _whatIsGroundProperty);
        }
        #endregion
    }
}
