using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    [Header("���� ���� ���� ��ġ ( �׽�Ʈ )")]
    Transform m_MutantSpawnPoint, m_WarrockSpawnPoint;

    void Start()
    {
        GameObject player = GameManager.Instance.Spwan(Defines.WorldObject.Player, "Player/Player");
        Camera.main.gameObject.GetOrAddComponet<CameraManager>().SetPlayer(player);

        GameObject go = new GameObject { name = "SpawningPool" };
        //MutantSpawn Mpool = go.GetOrAddComponet<MutantSpawn>();
        //WarrockSpawn Wpool = go.GetOrAddComponet<WarrockSpawn>();
        //Mpool.SetKeepMonsterCount(5);
        //Mpool.SetPosition(m_MutantSpawnPoint.position);
        //Wpool.SetKeepMonsterCount(3);
        //Wpool.SetPosition(m_WarrockSpawnPoint.position);

        UIManager.Instance.ShowSceneUI<UI_Player_GUI>();
    }
}
