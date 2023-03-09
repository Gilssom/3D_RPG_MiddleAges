using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 양수가 들어오면 Count , 음수가 들어오면 Reset
[CreateAssetMenu(menuName = "Quest/Task/Action/CoutinuousCount", fileName = "Coutinuous Count")]
public class ContinuousCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        // 값이 0보다 크다면 / 그동안 성공한 값에 현재 성공한 값을 빼서 반환 / 아니라면 그동한 성공한 값을 0으로 초기화
        return successCount > 0 ? currentSuccess + successCount : 0;
    }
}
