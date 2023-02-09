using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    // 키값 , 부위 , 수치
    public string m_ItemName;
    [Tooltip("Hp, Dp, At, MaxHp 부위만 설정 가능")]
    public string[] m_Part;
    public int[] m_Shame;
}

public class ItemEffectDataBase : SingletomManager<ItemEffectDataBase>
{
    [SerializeField]
    private ItemEffect[] m_ItemEffects;

    public Player m_Player;

    public DataManager m_DataManager { get; private set; }

    // 변수의 상수화 체력 / 방어력 / 공격력 / 최대 생명력
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
            // 아이템 장착
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
                                Debug.LogWarning("잘못된 Status 부위를 적용할려고 함");
                                break;
                        }
                        Debug.Log($"{item.m_ItemName}을 사용했습니다.");
                    }
                    return;
                }
            }
            Debug.LogError("ItemEffectDataBase.cs 에 일치하는 ItemName 이 없습니다.");
        }
    }
}
