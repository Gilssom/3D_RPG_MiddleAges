using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    enum Buttons
    {
        PointButton
    }

    enum Texts
    {
        PointText,
        ScoreText
    }

    // �ʱ� ���� :: ���� ������Ʈ�� �־ ã�� ����ε� ���ӿ�����Ʈ�� ������Ʈ ������ �ƴϴ�.
    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    // �������� �� �̿��� �ش� enum Ÿ���� �ѱ�ڴ�.
    public override void Init()
    {
        base.Init();

        // �ش� enum �� Buttons �ε� Button �� ������Ʈ�� ã�� �ش��ϴ� ��ü�� ���� �ش޶�� ��
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        // Extension Method �� Ȱ���� ��� - ������ �ν����Ϳ� �巡�� �� ������� ������ �־��� �͵��� �ڵ� ���ٷ� ��
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Defines.UIEvent.Drag);
    }

    int m_score = 0;

    public void OnButtonClicked(PointerEventData data)
    {
        m_score++;

        GetText((int)Texts.ScoreText).text = $"Score : {m_score}";
    }
}
