using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� ���� ��ȯ�ϴ� Module
[CreateAssetMenu(menuName = "Quest/Task/Action/PositiveCount", fileName = "Positive Count")]
public class PositiveCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        // ���� 0���� ũ�� / �׵��� ������ ���� ���� ������ ���� ���ؼ� ��ȯ / �ƴ϶�� �׵��� ������ ���� ��ȯ
        return successCount > 0 ? currentSuccess + successCount : currentSuccess;
    }
}
