using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class List : MonoBehaviour
{
    public ListNode m_FirstNode;
    public ListNode m_LastNode;

    public void Add(int value)
    {
        ListNode newNode = new ListNode();
        //newNode.Create(value);

        if(m_FirstNode == null)
        {
            m_FirstNode = newNode;
            m_LastNode  = newNode;
            return;
        }

        newNode.m_Pre   = m_LastNode;
        m_LastNode      = newNode;
    }

    public void Delete(int index)
    {
        ListNode startNode = m_FirstNode;
        ListNode node = null;

        for (int i = 0; i < index; i++)
        {
            node = startNode.m_Next;
        }

        m_LastNode          = node.m_Pre;
        m_LastNode.m_Next   = null;
    }
}
