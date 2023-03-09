using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskTarget : ScriptableObject
{
    public abstract object Value { get; }

    // 퀘스트시스템에 보고된 Target이 Task에 설정한 Target과 같은지 확인하는 함수
    public abstract bool IsEqual(object target);
}
