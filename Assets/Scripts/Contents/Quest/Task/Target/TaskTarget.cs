using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskTarget : ScriptableObject
{
    public abstract object Value { get; }

    // ����Ʈ�ý��ۿ� ����� Target�� Task�� ������ Target�� ������ Ȯ���ϴ� �Լ�
    public abstract bool IsEqual(object target);
}
