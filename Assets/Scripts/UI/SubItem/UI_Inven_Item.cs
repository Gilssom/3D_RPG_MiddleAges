using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameText,
    }

    string m_Name = "�׽�Ʈ ����";

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = m_Name;

        // ������ ���� Enter �̺�Ʈ
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"{m_Name} ����!"); }, Defines.UIEvent.Enter);

        // ������ ���� Ŭ�� �̺�Ʈ
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"{m_Name} Ŭ��!"); });
    }

    public void SetInfo(string name)
    {
        m_Name = name;
    }
}
