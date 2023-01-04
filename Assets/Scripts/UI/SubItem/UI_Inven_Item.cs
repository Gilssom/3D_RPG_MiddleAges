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

    string m_Name;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = m_Name;

        // 아이템 슬롯 클릭 이벤트
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"{m_Name} 클릭!"); });
    }

    public void SetInfo(string name)
    {
        m_Name = name;
    }
}
