using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ Ÿ���� object name �� �̿��� �´��� �ƴ����� �Ǻ�
[CreateAssetMenu(menuName = "Quest/Task/Target/GameObject", fileName = "Target_")]
public class GameObjectTarget : TaskTarget
{
    [SerializeField]
    private GameObject m_Value;

    public override object Value => m_Value;

    public override bool IsEqual(object target)
    {
        // Prefab �������� GameObject �� ������ �� ���̱� ������
        // ���� ���ϴ� Object �� �̸����� �´��� �ƴ����� �Ǻ��Ѵ�.
        var targetAsGameObject = target as GameObject;

        if (targetAsGameObject == null)
            return false;
        return targetAsGameObject.name.Contains(m_Value.name);
    }
}
