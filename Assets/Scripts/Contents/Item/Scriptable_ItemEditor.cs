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
        //GUI �ɼ� ����. ũ�⸦ ���� ��������
        options = new GUILayoutOption[] { GUILayout.Width(128), GUILayout.Height(128) };

        //Editor���� serializedObject �� ���� �ش� ��ũ��Ʈ�� ������ �����ϴ�.
        //serializedObject.target�� �� ��ȯ�� ���� �ش� ���� ������ ����������.
        m_Item = serializedObject.targetObject as Item;

        //���� sprite�� �⺻���� ����.
        sprite = m_Item.m_ItemImage;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal(); //�� ������ ���� ����

        GUILayout.Label("Source Image"); //�� ����. �� �� ������ ���������� ���� �ȴ�.
                                         //EditorGUILayout.PrefixLabel("Source Image"); //�� ����. �������� ���� �ȴ�.

        //EditorGUILayout.ObjectField �� ���� �̹����� ���̰� �� �� �ִ�.
        //(Object, type, allowSceneObject (���� ���� ����), options)
        EditorGUILayout.ObjectField(sprite, typeof(Sprite), false, options);

        EditorGUILayout.EndHorizontal();  //�� ������
    }
}
