using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAnimEvent : MonoBehaviour
{
    [Header("���� ��Ʈ �ڽ�")]
    [SerializeField, Tooltip("��Ʈ �ڽ� �ݶ��̴�")]
    protected Collider m_AttackArea;
    [SerializeField, Tooltip("��Ʈ �ڽ��� �ٽ� ���� �ð�")]
    protected float m_HitCheckTime;

    protected WaitForSeconds m_CheckTime;

    private void Start()
    {
        Init();
    }

    protected abstract void Init();
    protected abstract void OnFinishedAttack();
    protected abstract void OnForwardAttack(float Power);
    protected abstract void OffForwardAttack();
    protected abstract IEnumerator AttackArea();
    protected abstract void TestAttackEffect(int AttackNumber);

}
