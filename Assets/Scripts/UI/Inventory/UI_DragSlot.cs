using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DragSlot : SingletomManager<UI_DragSlot>
{
    public UI_Inven_Slot m_DragSlot;

    [SerializeField]
    private Image m_ImageItem;

    public void DragSetImage(Image itemImage)
    {
        m_ImageItem.sprite = itemImage.sprite;
    }

    public void SetColor(float alpha)
    {
        Color color = m_ImageItem.color;
        color.a = alpha;
        m_ImageItem.color = color;
    }
}
