using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    // Ű�� , ���� , ��ġ
    public string m_ItemName;
    [Tooltip("Hp, Dp, At, MaxHp ������ ���� ����")]
    public string[] m_Part;
    public int[] m_Shame;
}

public class ItemEffectDataBase : SingletomManager<ItemEffectDataBase>
{
    [SerializeField]
    private ItemEffect[] m_ItemEffects;

    public Player m_Player;

    public DataManager m_DataManager { get; private set; }

    // ������ ���ȭ ü�� / ���� / ���ݷ� / �ִ� �����
    private const string HP = "Hp", DP = "Dp", AT = "At", MaxHp = "MaxHp";

    void Awake()
    {
        m_DataManager = new DataManager();

        m_DataManager.Init("ItemBaseData");
        SetItemEffects();
    }

    void SetItemEffects()
    {
        for (int i = 0; i < 4; i++)
        {
            m_ItemEffects[i].m_Part = new string[3];
            m_ItemEffects[i].m_Shame = new int[3];

            Dictionary<int, Data.ItemStat> dict = m_DataManager.ItemDict;
            Data.ItemStat stat = dict[i + 1000];

            m_ItemEffects[i].m_ItemName = stat.itemName;
            m_ItemEffects[i].m_Part[0] = stat.itemPart_0;
            m_ItemEffects[i].m_Part[1] = stat.itemPart_1;
            m_ItemEffects[i].m_Part[2] = stat.itemPart_2;
            m_ItemEffects[i].m_Shame[0] = stat.itemShame;
            m_ItemEffects[i].m_Shame[1] = stat.itemShame;
            m_ItemEffects[i].m_Shame[2] = stat.itemShame;
        }
    }

    public void UseItem(Item item)
    {
        if (item.m_ItemType == Item.ItemType.Equipment)
        {
            // ������ ����
        }
        else if (item.m_ItemType == Item.ItemType.Used)
        {
            for (int i = 0; i < m_ItemEffects.Length; i++)
            {
                if (m_ItemEffects[i].m_ItemName == item.m_ItemName)
                {
                    for (int j = 0; j < m_ItemEffects[i].m_Part.Length; j++)
                    {
                        switch (m_ItemEffects[i].m_Part[j])
                        {
                            case HP:
                                m_Player.IncreaseHp(m_ItemEffects[i].m_Shame[j]);
                                break;
                            case DP:
                                m_Player.IncreaseDF(m_ItemEffects[i].m_Shame[j]);
                                break;
                            case AT:
                                m_Player.IncreaseAt(m_ItemEffects[i].m_Shame[j]);
                                break;
                            case MaxHp:
                                m_Player.IncreaseMaxHp(m_ItemEffects[i].m_Shame[j]);
                                break;
                            case null:
                                break;
                            default:
                                Debug.LogWarning("�߸��� Status ������ �����ҷ��� ��");
                                break;
                        }
                        Debug.Log($"{item.m_ItemName}�� ����߽��ϴ�.");
                    }
                    return;
                }
            }
            Debug.LogError("ItemEffectDataBase.cs �� ��ġ�ϴ� ItemName �� �����ϴ�.");
        }
    }
}
