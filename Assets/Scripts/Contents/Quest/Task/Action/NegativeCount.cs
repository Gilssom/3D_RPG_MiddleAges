using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 음수일 때만 반환하는 Module
[CreateAssetMenu(menuName = "Quest/Task/Action/NegativeCount", fileName = "Negative Count")]
public class NegativeCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        // 값이 0보다 작다면 / 그동안 성공한 값에 현재 성공한 값을 빼서 반환 / 아니라면 그동한 성공한 값만 반환
        return successCount < 0 ? currentSuccess - successCount : currentSuccess;
    }
}
