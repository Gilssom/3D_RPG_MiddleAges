using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // New Input System �� ����Ѵ�.
using UnityEngine.UI;

public class UI_InputNumber : UI_Base
{
    private bool isActivited = false;

    enum InputFields
    {
        UI_InputField,
    }

    enum GameObjects
    {
        UI_InputBase,
    }

    enum Texts
    {
        UI_PreviewText,
        UI_InputText,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<InputField>(typeof(InputFields));

        GetObject((int)GameObjects.UI_InputBase).SetActive(false);
    }

    public void Activited(bool OnOff)
    {
        if(isActivited)
        {
            if (OnOff)
                Ok();
            else
                Cancel();
        }
    }

    public void Call(UI_Inven_Slot item)
    {
        GetObject((int)GameObjects.UI_InputBase).SetActive(true);
        isActivited = true;
        UI_DragSlot.Instance.SetColor(0);
        Get<InputField>((int)InputFields.UI_InputField).text = "";
        GetText((int)Texts.UI_PreviewText).text = item.m_ItemCount.ToString();
    }

    public void Cancel()
    {
        GetObject((int)GameObjects.UI_InputBase).SetActive(false);
        isActivited = false;
        UI_DragSlot.Instance.m_DragSlot = null;
    }

    public void Ok()
    {
        int num; 
         
        if (GetText((int)Texts.UI_InputText).text != "")
        {
            if (CheckNumber(GetText((int)Texts.UI_InputText).text))
            {
                // ���ڶ�� �Ǻ��� �� ���
                num = int.Parse(GetText((int)Texts.UI_InputText).text);

                if (num > UI_DragSlot.Instance.m_DragSlot.m_ItemCount)
                    num = UI_DragSlot.Instance.m_DragSlot.m_ItemCount;
            }
            else
            {
                // ���� �̿� ���ڶ�� �Ǻ��� �� ���
                num = 0;
            }
        }
        else
        {
            // UI_PreviewText ���� ���� �ִ� ������ �����ִ�.
            num = int.Parse(GetText((int)Texts.UI_PreviewText).text);
        }

        DropItem(num);
    }

    // ���� �� �츮�� �ٷ� �ı����� ������ �̱� ������
    private void DropItem(int num)
    {
        if (InventoryManager.m_ShopActivated)
        {
            Debug.Log($"{UI_DragSlot.Instance.m_DragSlot.m_Item.m_ItemName} {num}�� �Ǹ� �Ϸ�");
            for (int i = 0; i < ShopManager.Instance.m_ShopSaleItems.Length; i++)
            {
                if (ShopManager.Instance.m_ShopSaleItems[i].m_ItemId == UI_DragSlot.Instance.m_DragSlot.m_Item.m_ItemId)
                {
                    BaseInfo.playerInfo.Gold += ShopManager.Instance.m_ShopSaleItems[i].m_SalePrice * num;
                    Debug.Log($"{ShopManager.Instance.m_ShopSaleItems[i].m_SalePrice * num}��� ȹ��");
                    break;
                }
            }
        }

        UI_DragSlot.Instance.m_DragSlot.SetSlotCount(-num);
        SoundManager.Instance.Play("UI/Destroy Item");

        UI_DragSlot.Instance.m_DragSlot = null;
        GetObject((int)GameObjects.UI_InputBase).SetActive(false);
    }

    private bool CheckNumber(string argString)
    {
        char[] tempCharArray = argString.ToCharArray(); // ���� char ����
        bool isNumber = true;

        for (int i = 0; i < tempCharArray.Length; i++)
        {
            // �����ڵ�� ���ؼ� �������� Ȯ���ؾ��� ( ���ڴ� 48 ���� 57 )
            if (tempCharArray[i] >= 48 && tempCharArray[i] <= 57)
                continue;

            // 0 ���� 9 ���ڰ� �ƴϸ� ���ڰ� �ƴϴٷ� �Ǻ�
            isNumber = false;
        }

        return isNumber;
    }
}
