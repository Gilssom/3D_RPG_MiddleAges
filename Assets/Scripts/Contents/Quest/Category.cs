using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Category 는 주로 CodeName 을 비교 하는 경우가 많다.
/// Category 와 문자열 을 바로 비교할 수 있도록 "비교 연산자" 를 추가해준다. Equals 함수를 override 를 해줘야한다.
/// >> System namespace 추가 후 IEquatable interface 상속
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
        if (ReferenceEquals(other, this)) // other와 자신이 같다면 true
            return true;
        if (GetType() != other.GetType()) // other와 자신의 Type이 다르면 false
            return false;
        
        // other 의 CodeName 과 나의 CodeName 이 같은지 확인
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
