using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ChestItem))]
public class ChestItemEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var typeProp = property.FindPropertyRelative("type");
        var keyItemProp = property.FindPropertyRelative("_keyItem");
        var dungeonItemProp = property.FindPropertyRelative("_dungeonItem");

        Rect line = position;
        line.height = EditorGUIUtility.singleLineHeight;

        // Draw Type dropdown
        EditorGUI.PropertyField(line, typeProp);
        line.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Draw only relevant field
        if (typeProp.enumValueIndex == (int)ChestItem.ItemType.KeyItems)
        {
            EditorGUI.PropertyField(line, keyItemProp, new GUIContent("Key Item"));
        }
        else if (typeProp.enumValueIndex == (int)ChestItem.ItemType.DungeonItem)
        {
            EditorGUI.PropertyField(line, dungeonItemProp, new GUIContent("Dungeon Item"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
    }
}