using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameManager.Instance.Spwan(Defines.WorldObject.Player, "Player/Player");
        Camera.main.gameObject.GetOrAddComponet<CameraManager>().SetPlayer(player);
        GameManager.Instance.Spwan(Defines.WorldObject.Monster, "Enemy/Mutant");
    }
}
