using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 지정한 타겟을 object name 을 이용해 맞는지 아닌지를 판별
[CreateAssetMenu(menuName = "Quest/Task/Target/GameObject", fileName = "Target_")]
public class GameObjectTarget : TaskTarget
{
    [SerializeField]
    private GameObject m_Value;

    public override object Value => m_Value;

    public override bool IsEqual(object target)
    {
        // Prefab 형식으로 GameObject 가 전달이 될 것이기 때문에
        // 서로 비교하는 Object 의 이름으로 맞는지 아닌지를 판별한다.
        var targetAsGameObject = target as GameObject;

        if (targetAsGameObject == null)
            return false;
        return targetAsGameObject.name.Contains(m_Value.name);
    }
}
