using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using CharacterController;

public class UltimateEvnet : MonoBehaviour
{
    private List<Transform> m_ChildrenSword = new List<Transform>();

    private WaitForSeconds ATTACK_TIME, ATTACK_DELAY;

    private void OnEnable()
    {
        ATTACK_TIME = new WaitForSeconds(1.75f);
        ATTACK_DELAY = new WaitForSeconds(0.15f);

        SetPosition();
        FindChild();
        StartCoroutine(UltimateSkill());
    }

    void SetPosition()
    {
        transform.position = new Vector3(BaseInfo.playerInfo.transform.position.x,
            transform.position.y, BaseInfo.playerInfo.transform.position.z);

        transform.forward = BaseInfo.playerInfo.transform.forward;
    }

    void FindChild()
    {
        foreach(Transform Child in gameObject.transform)
        {
            if(Child.name != name)
            {
                m_ChildrenSword.Add(Child);
            }
        }
    }

    void Update()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 360), 8f, RotateMode.FastBeyond360).
                    SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetRelative(transform);
    }

    /// <summary>
    /// 스킬 사용 시 NormalSword.cs UltimateSkill 함수에서 Ultimate_Effect Ojbect 활성화
    /// 
    /// 1. 캐릭터 기준 지면 아래에서 플레이어 주변 위치로 Y값 증가.
    /// 2. 칼 하나씩 일정 시간 마다 정면으로 투척
    /// 3. 6번째 칼이 투척되고 없어지면 캐릭터 기준 지면 아래로 다시 이동 후 Disable (비활성화)
    /// </summary>
    /// <returns></returns>
    IEnumerator UltimateSkill()
    {
        transform.DOMoveY(0.45f, 0.8f).SetEase(Ease.InOutBack);
        yield return ATTACK_TIME;
        Camera.main.fieldOfView = 50;
        for (int i = 0; i < m_ChildrenSword.Count - 1; i++)
        {
            m_ChildrenSword[i].gameObject.GetOrAddComponet<BoxCollider>().enabled = true;
            m_ChildrenSword[i].DOMove(m_ChildrenSword.Last().position, 0.15f);
            yield return ATTACK_DELAY;
            ResourcesManager.Instance.Destroy(m_ChildrenSword[i].gameObject);
        }

        ResourcesManager.Instance.Destroy(gameObject);
    }

    private void OnDisable()
    {
        Debug.Log("Ultimate Skill Off");
        BaseInfo.playerInfo.m_Anim.SetBool("isUltiSkill", false);
        BaseInfo.playerInfo.stateMachine.ChangeState(StateName.MOVE);
    } 
}
