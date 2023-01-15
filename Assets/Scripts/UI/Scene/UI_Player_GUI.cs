using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_GUI : UI_Scene
{
    enum GameObjects
    {
        HPBar,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
    }

    void Update()
    {
        // 각각의 오브젝트에서 가지고 있는 Stat 에 의해 HP Bar 변경
        float ratio = BaseInfo.playerInfo.Hp / (float)BaseInfo.playerInfo.MaxHp;
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        if (ratio <= 0)
            ResourcesManager.Instance.Destroy(this.gameObject);

        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
