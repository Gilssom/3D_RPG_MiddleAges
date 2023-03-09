using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� ������ Count , ������ ������ Reset
[CreateAssetMenu(menuName = "Quest/Task/Action/CoutinuousCount", fileName = "Coutinuous Count")]
public class ContinuousCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        // ���� 0���� ũ�ٸ� / �׵��� ������ ���� ���� ������ ���� ���� ��ȯ / �ƴ϶�� �׵��� ������ ���� 0���� �ʱ�ȭ
        return successCount > 0 ? currentSuccess + successCount : 0;
    }
}
