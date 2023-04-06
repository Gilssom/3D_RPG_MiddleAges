using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletomManager<UIManager>
{
    int m_Order = 10; // 최근에 사용된 Sorting Order

    [Header("Popup UI Open / Close")]
    public bool isInvenOpen, isBossHPOpen;

    // 가장 마지막에 띄워진 UI가 지워져야 하는 구조이기 때문에 Stack 구조가 맞다 => First In Last Out ( FILO )
    Stack<UI_Popup> m_PopupStack = new Stack<UI_Popup>();
    UI_Scene m_SceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@@UI_Root");
            if (root == null)
                root = new GameObject { name = "@@UI_Root" };
            return root;
        }    
    }

    // sorting 을 적용할것인지 안할것인지 / 팝업 UI 의 우선순위를 변경하는 코드
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utilles.GetOrAddComponet<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = m_Order;
            m_Order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    #region #World Space 폴더 안의 UI Prefab 불러오기
    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourcesManager.Instance.Instantiate($"UI/WorldSpace/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        // Render Mode , Event Camera 설정
        Canvas canvas = go.GetOrAddComponet<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Utilles.GetOrAddComponet<T>(go);
    }
    #endregion

    #region #Sub Item 폴더 안의 UI Prefab 불러오기
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourcesManager.Instance.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return Utilles.GetOrAddComponet<T>(go);
    }
    #endregion

    #region #Scene 폴더 안의 UI Prefab 불러오기
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        // 이름이 없다면 type의 이름을 그대로 사용
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourcesManager.Instance.Instantiate($"UI/Scene/{name}");
        T sceneUI = Utilles.GetOrAddComponet<T>(go);
        m_SceneUI = sceneUI;

        //부모 설정
        go.transform.SetParent(Root.transform);

        return sceneUI;
    }
    #endregion

    #region #Popup 폴더 안의 UI Prefab 불러오기
    // UI_Popup 스크립트를 상속받는 객체를 가져옴
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        // 이름이 없다면 type의 이름을 그대로 사용
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourcesManager.Instance.Instantiate($"UI/Popup/{name}");
        T popup = Utilles.GetOrAddComponet<T>(go);
        m_PopupStack.Push(popup);

        //부모 설정
        go.transform.SetParent(Root.transform);

        return popup;
    }
    #endregion

    #region #UI 닫기 Method들
    // UI_Popup 을 직접 확인하여 닫는 코드
    public void ClosePopupUI(UI_Popup popup)
    {
        if (m_PopupStack.Count == 0)
            return;

        // Peek => 마지막에 있는 것을 확인하는 용도
        if(m_PopupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (m_PopupStack.Count == 0)
            return;

        UI_Popup popup = m_PopupStack.Pop();
        ResourcesManager.Instance.Destroy(popup.gameObject);
        popup = null;
        m_Order--;
    }

    public void CloseAllPopupUI()
    {
        while (m_PopupStack.Count > 0)
            ClosePopupUI();
    }
    #endregion

    public void Clear()
    {
        CloseAllPopupUI();
        m_SceneUI = null;
    }
}
