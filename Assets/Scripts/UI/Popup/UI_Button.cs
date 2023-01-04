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

    // 초기 문제 :: 원래 컴포넌트를 넣어서 찾는 방식인데 게임오브젝트는 컴포넌트 형식이 아니다.
    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    // 리프렉션 을 이용해 해당 enum 타입을 넘기겠다.
    public override void Init()
    {
        base.Init();

        // 해당 enum 은 Buttons 인데 Button 의 컴포넌트를 찾아 해당하는 객체를 참조 해달라는 뜻
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        // Extension Method 를 활용한 방식 - 기존의 인스펙터에 드래그 앤 드롭으로 설정해 주었던 것들이 코드 한줄로 됨
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
