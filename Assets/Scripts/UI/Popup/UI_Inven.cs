using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Popup
{
    enum GameObjects
    {
        ItemPanel,
        UI_Inven_GridArea
    }

    GameObject m_Gridpanel;

    public override void Init()
    {
        base.Init();

        UIManager.Instance.isInvenOpen = true;

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.UI_Inven_GridArea);

        m_Gridpanel = gridPanel;

        foreach (Transform child in gridPanel.transform)
            ResourcesManager.Instance.Destroy(child.gameObject);

        // 실제 인벤토리 정보를 참고해야함
        for (int i = 0; i < 8; i++)
        {
            GameObject item = UIManager.Instance.MakeSubItem<UI_Inven_2000>(parent: gridPanel.transform).gameObject;

            // item.GetOrAddComponent<> => Extension Method 활용
            UI_Inven_2000 invenItem = item.GetOrAddComponet<UI_Inven_2000>();
        }
    }

    //public void ItemAdd()
    //{
    //    GameObject item = UIManager.Instance.MakeSubItem<UI_Inven_2000>(parent: m_Gridpanel.transform).gameObject;
    //}
}