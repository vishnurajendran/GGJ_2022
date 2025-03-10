using UnityEditor;
using UnityEngine;

namespace MPUIKIT.Editor {
#pragma warning disable CS0246 // The type or namespace name 'Rectangle' could not be found (are you missing a using directive or an assembly reference?)
    [CustomPropertyDrawer(typeof(Rectangle))]
#pragma warning restore CS0246 // The type or namespace name 'Rectangle' could not be found (are you missing a using directive or an assembly reference?)
    public class RectanglePropertyDrawer : PropertyDrawer{
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            {
                Rect LabelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                Rect RadiusVectorRect = new Rect(position.x, 
                    position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, 
                    position.width, EditorGUIUtility.singleLineHeight);
                
                SerializedProperty uniform = property.FindPropertyRelative("m_UniformCornerRadius");
                SerializedProperty radius = property.FindPropertyRelative("m_CornerRadius");
                
                MPEditorUtility.CornerRadiusModeGUI(LabelRect, ref uniform, new []{"Free", "Uniform"});

                float floatVal = radius.vector4Value.x;
                Vector4 vectorValue = radius.vector4Value;
                float[] zw = new[] {vectorValue.w, vectorValue.z};
                float[] xy = new[] {vectorValue.x, vectorValue.y};
                
                EditorGUI.BeginChangeCheck();
                {
                    EditorGUI.showMixedValue = radius.hasMultipleDifferentValues;
                    if (uniform.boolValue) {
                        floatVal = EditorGUI.FloatField(RadiusVectorRect, "Uniform Radius", floatVal);
                    }
                    else {
                        
                        
                        EditorGUI.MultiFloatField(RadiusVectorRect, new [] {
                            new GUIContent("W"), new GUIContent("Z")}, zw );
                        RadiusVectorRect.y +=
                            EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        EditorGUI.MultiFloatField(RadiusVectorRect, new [] {
                            new GUIContent("X "), new GUIContent("Y")}, xy );

                    }
                    EditorGUI.showMixedValue = false;
                }
                if (EditorGUI.EndChangeCheck()) {
                    radius.vector4Value = uniform.boolValue 
                        ? new Vector4(floatVal, floatVal, floatVal, floatVal) 
                        : new Vector4(xy[0], xy[1], zw[1], zw[0]);
                }
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (property.FindPropertyRelative("m_UniformCornerRadius").boolValue) {
                return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
            }
            return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 2;
        }
    }
}