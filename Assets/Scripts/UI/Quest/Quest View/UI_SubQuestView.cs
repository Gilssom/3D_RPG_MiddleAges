using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SubQuestView : UI_Popup
{
    public override void Init()
    {
        base.Init();

        gameObject.SetActive(false);
    }

    [SerializeField]
    private UI_SubQuestDetailView m_SubQuestDetailView;

    private void ShowDetail(Quest quest)
    {
        m_SubQuestDetailView.Show(quest);
    }

    private void HideDetail()
    {
        m_SubQuestDetailView.Hide();
    }

    public void OpenQuest(Quest quest)
    {
        gameObject.SetActive(true);
        ShowDetail(quest);
    }

    public void CloseQuest()
    {
        HideDetail();
        gameObject.SetActive(false);
    }
}
