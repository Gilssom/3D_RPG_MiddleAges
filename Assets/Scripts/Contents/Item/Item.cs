using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Item : ScriptableObject
{
    public int m_ItemId;
    public string m_ItemName;
    [TextArea]
    public string m_ItemInfo , m_ItemTypeInfo, m_ItemClassInfo;
    public ItemType m_ItemType;
    public ItemClass m_ItemClass;
    public Sprite m_ItemImage;
    public GameObject m_ItemPrefabs;
    public UsedType m_UsedType;
    public ItemParts m_ItemPart;

    public enum ItemClass
    {
        // 일반 , 고급 , 희귀 , 영웅 , 전설
        Normal,
        Advanced,
        Rair,
        Epic,
        Legendary,
        Relics,
    }

    public enum ItemType
    {
        // 장비 , 소비 , 재료 , 기타
        Equipment,
        Used,
        Ingredient,
        Etc,
    }

    public enum ItemParts
    {
        // 모자 어깨 상의 하의 장갑 무기
        Helmat,
        Shoulder,
        Top,
        Bottom,
        Glove,
        Weapon,
        None,
    }

    public enum UsedType
    {
        None,
        Potion,
        Food,
    }
}
