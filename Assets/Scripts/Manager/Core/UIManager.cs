using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletomManager<UIManager>
{
    int m_Order = 10; // �ֱٿ� ���� Sorting Order

    [Header("Popup UI Open / Close")]
    public bool isInvenOpen, isBossHPOpen;

    // ���� �������� ����� UI�� �������� �ϴ� �����̱� ������ Stack ������ �´� => First In Last Out ( FILO )
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

    // sorting �� �����Ұ����� ���Ұ����� / �˾� UI �� �켱������ �����ϴ� �ڵ�
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

    #region #World Space ���� ���� UI Prefab �ҷ�����
    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourcesManager.Instance.Instantiate($"UI/WorldSpace/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        // Render Mode , Event Camera ����
        Canvas canvas = go.GetOrAddComponet<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Utilles.GetOrAddComponet<T>(go);
    }
    #endregion

    #region #Sub Item ���� ���� UI Prefab �ҷ�����
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

    #region #Scene ���� ���� UI Prefab �ҷ�����
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        // �̸��� ���ٸ� type�� �̸��� �״�� ���
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourcesManager.Instance.Instantiate($"UI/Scene/{name}");
        T sceneUI = Utilles.GetOrAddComponet<T>(go);
        m_SceneUI = sceneUI;

        //�θ� ����
        go.transform.SetParent(Root.transform);

        return sceneUI;
    }
    #endregion

    #region #Popup ���� ���� UI Prefab �ҷ�����
    // UI_Popup ��ũ��Ʈ�� ��ӹ޴� ��ü�� ������
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        // �̸��� ���ٸ� type�� �̸��� �״�� ���
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourcesManager.Instance.Instantiate($"UI/Popup/{name}");
        T popup = Utilles.GetOrAddComponet<T>(go);
        m_PopupStack.Push(popup);

        //�θ� ����
        go.transform.SetParent(Root.transform);

        return popup;
    }
    #endregion

    #region #UI �ݱ� Method��
    // UI_Popup �� ���� Ȯ���Ͽ� �ݴ� �ڵ�
    public void ClosePopupUI(UI_Popup popup)
    {
        if (m_PopupStack.Count == 0)
            return;

        // Peek => �������� �ִ� ���� Ȯ���ϴ� �뵵
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
