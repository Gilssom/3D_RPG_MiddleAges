using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Category �� �ַ� CodeName �� �� �ϴ� ��찡 ����.
/// Category �� ���ڿ� �� �ٷ� ���� �� �ֵ��� "�� ������" �� �߰����ش�. Equals �Լ��� override �� ������Ѵ�.
/// >> System namespace �߰� �� IEquatable interface ���
/// </summary>
[CreateAssetMenu(menuName = "Category", fileName = "Category_")]
public class Category : ScriptableObject , IEquatable<Category>
{
    [SerializeField]
    private string m_CodeName;
    [SerializeField]
    private string m_DisPlayName;

    public string p_CodeName => m_CodeName;
    public string p_DisPlayName => m_DisPlayName;

    #region #Operator
    public bool Equals(Category other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(other, this)) // other�� �ڽ��� ���ٸ� true
            return true;
        if (GetType() != other.GetType()) // other�� �ڽ��� Type�� �ٸ��� false
            return false;
        
        // other �� CodeName �� ���� CodeName �� ������ Ȯ��
        return m_CodeName == other.p_CodeName;
    }

    public override int GetHashCode() => (p_CodeName, p_DisPlayName).GetHashCode();

    public override bool Equals(object other) => base.Equals(other);

    // lhs => Category , rhs => CodeName
    public static bool operator ==(Category lhs, string rhs)
    {
        if (lhs is null)
            return ReferenceEquals(rhs, null);
        return lhs.p_CodeName == rhs || lhs.p_DisPlayName == rhs;
    }

    public static bool operator !=(Category lhs, string rhs) => !(lhs == rhs);
    // category.CodeName == "Kill" => X
    // category == "Kill" => O
    #endregion
}
