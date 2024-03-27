using UnityEditor;

namespace _Project.Editor.Scripts.Utilities
{
    public static class CustomInspectorUtility
    {
        public static void DrawProperties(params SerializedProperty[] properties)
        {
            foreach (SerializedProperty property in properties)
            {
                EditorGUILayout.PropertyField(property);
            }
        }
    }
}
