using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���� ��ȯ�ϴ� Module
[CreateAssetMenu(menuName = "Quest/Task/Action/NegativeCount", fileName = "Negative Count")]
public class NegativeCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        // ���� 0���� �۴ٸ� / �׵��� ������ ���� ���� ������ ���� ���� ��ȯ / �ƴ϶�� �׵��� ������ ���� ��ȯ
        return successCount < 0 ? currentSuccess - successCount : currentSuccess;
    }
}
