using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/SimpleCount", fileName = "Simple Count")]
public class SimpleCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        //들어온 값을 그동안 성공한 값에 추가준 값을 반환
        return currentSuccess + successCount;
    }
}
