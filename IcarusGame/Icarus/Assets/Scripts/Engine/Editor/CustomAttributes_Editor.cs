#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(DisplayNameAttribute))]
public class DisplayNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        DisplayNameAttribute displayNameAttribute = attribute as DisplayNameAttribute;

        EditorGUI.PropertyField(position, property, new GUIContent(displayNameAttribute.m_displayName), true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}


[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        GUI.enabled = false;

        EditorGUI.PropertyField(position, property, label, true);

        GUI.enabled = true;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}

[CustomPropertyDrawer(typeof(ReorderableListAttribute), true)]
public class ReorderableListDrawer : PropertyDrawer
{
    private ReorderableList list;

    private ReorderableList getList(SerializedProperty property)
    {
        if (list == null)
        {
            list = new ReorderableList(property.serializedObject, property, true, true, true, true);
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.width -= 40;
                rect.x += 20;
                EditorGUI.PropertyField(rect, property.GetArrayElementAtIndex(index), true);
            };
        }
        return list;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var listProperty = property.FindPropertyRelative("List");
        ReorderableList list = getList(listProperty);
        float height = 0.0f;

        for (int i = 0; i < listProperty.arraySize; ++i)
        {
            height = Mathf.Max(height, EditorGUI.GetPropertyHeight(listProperty.GetArrayElementAtIndex(i)));
        }

        list.elementHeight = height;
        list.DoList(position);

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return getList(property.FindPropertyRelative("List")).GetHeight();
    }
}
#endif