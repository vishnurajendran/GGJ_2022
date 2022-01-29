using UnityEditor;
using UnityEngine;

namespace MPUIKIT.Editor {
#pragma warning disable CS0246 // The type or namespace name 'Circle' could not be found (are you missing a using directive or an assembly reference?)
    [CustomPropertyDrawer(typeof(Circle))]
#pragma warning restore CS0246 // The type or namespace name 'Circle' could not be found (are you missing a using directive or an assembly reference?)
    public class CirclePropertyDrawer : PropertyDrawer{

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            {
                Rect radiusRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                Rect toolBarRect = new Rect(position.x + EditorGUIUtility.labelWidth, 
                    position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                    position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
                
                SerializedProperty spFitCircleRadius = property.FindPropertyRelative("m_FitRadius");
                bool FitCirlce = spFitCircleRadius.boolValue;
                EditorGUI.BeginDisabledGroup(FitCirlce);
                {
                    EditorGUI.PropertyField(radiusRect, property.FindPropertyRelative("m_Radius"),
                        new GUIContent("Radius"));
                }
                EditorGUI.EndDisabledGroup();
                
                
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = spFitCircleRadius.hasMultipleDifferentValues;
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(EditorGUIUtility.labelWidth);
                        
                        FitCirlce = GUI.Toolbar(toolBarRect, FitCirlce ? 1 : 0, new[] {"Free", "Fit"}) == 1;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.showMixedValue = false;

                if (EditorGUI.EndChangeCheck()) {
                    spFitCircleRadius.boolValue = FitCirlce;
                }
                
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 2;
        }
    }
}