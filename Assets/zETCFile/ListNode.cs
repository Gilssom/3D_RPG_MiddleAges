using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListNode
{
    public ListNode m_Pre;
    public ListNode m_Next;
    public int m_Value;

    public void Create(ListNode pre, ListNode next, int value)
    {
        m_Pre = pre;
        m_Next = next;
        m_Value = value;
    }
}
