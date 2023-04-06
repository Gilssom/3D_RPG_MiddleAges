using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class Scriptable_ItemEditor : Editor
{
    private Item m_Item;
    private Sprite sprite;
    private GUILayoutOption[] options;

    private void OnEnable()
    {
        options = new GUILayoutOption[] { GUILayout.Width(128), GUILayout.Height(128) };

        m_Item = serializedObject.targetObject as Item;

        sprite = m_Item.m_ItemImage;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Source Image");
        EditorGUILayout.ObjectField(sprite, typeof(Sprite), false, options);

        EditorGUILayout.EndHorizontal();
    }
}
