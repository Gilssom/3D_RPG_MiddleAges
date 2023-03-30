using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // New Input System 을 사용한다.
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
                // 숫자라고 판별이 날 경우
                num = int.Parse(GetText((int)Texts.UI_InputText).text);

                if (num > UI_DragSlot.Instance.m_DragSlot.m_ItemCount)
                    num = UI_DragSlot.Instance.m_DragSlot.m_ItemCount;
            }
            else
            {
                // 숫자 이외 문자라고 판별이 날 경우
                num = 0;
            }
        }
        else
        {
            // UI_PreviewText 에는 현재 최대 갯수가 적혀있다.
            num = int.Parse(GetText((int)Texts.UI_PreviewText).text);
        }

        DropItem(num);
    }

    // 버릴 때 우리는 바로 파괴시켜 버릴것 이기 때문에
    private void DropItem(int num)
    {
        if (InventoryManager.m_ShopActivated)
        {
            Debug.Log($"{UI_DragSlot.Instance.m_DragSlot.m_Item.m_ItemName} {num}개 판매 완료");
            for (int i = 0; i < ShopManager.Instance.m_ShopSaleItems.Length; i++)
            {
                if (ShopManager.Instance.m_ShopSaleItems[i].m_ItemId == UI_DragSlot.Instance.m_DragSlot.m_Item.m_ItemId)
                {
                    BaseInfo.playerInfo.Gold += ShopManager.Instance.m_ShopSaleItems[i].m_SalePrice * num;
                    Debug.Log($"{ShopManager.Instance.m_ShopSaleItems[i].m_SalePrice * num}골드 획득");
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
        char[] tempCharArray = argString.ToCharArray(); // 문자 char 변형
        bool isNumber = true;

        for (int i = 0; i < tempCharArray.Length; i++)
        {
            // 유니코드로 비교해서 숫자인지 확인해야함 ( 숫자는 48 부터 57 )
            if (tempCharArray[i] >= 48 && tempCharArray[i] <= 57)
                continue;

            // 0 부터 9 숫자가 아니면 숫자가 아니다로 판별
            isNumber = false;
        }

        return isNumber;
    }
}
