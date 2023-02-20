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
        //GUI 옵션 설정. 크기를 위해 설정해줌
        options = new GUILayoutOption[] { GUILayout.Width(128), GUILayout.Height(128) };

        //Editor에선 serializedObject 를 통해 해당 스크립트에 접근이 가능하다.
        //serializedObject.target의 형 변환을 통해 해당 값에 접근이 가능해진다.
        m_Item = serializedObject.targetObject as Item;

        //기존 sprite를 기본으로 해줌.
        sprite = m_Item.m_ItemImage;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal(); //줄 맞춤을 위해 설정

        GUILayout.Label("Source Image"); //라벨 설정. 이 후 값들이 오른쪽으로 정렬 된다.
                                         //EditorGUILayout.PrefixLabel("Source Image"); //라벨 설정. 왼쪽으로 정렬 된다.

        //EditorGUILayout.ObjectField 를 통해 이미지를 보이게 할 수 있다.
        //(Object, type, allowSceneObject (수정 가능 여부), options)
        EditorGUILayout.ObjectField(sprite, typeof(Sprite), false, options);

        EditorGUILayout.EndHorizontal();  //줄 마무리
    }
}
