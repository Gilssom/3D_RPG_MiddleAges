using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public Defines.Scene SceneType { get; protected set; } = Defines.Scene.Unknown;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {

    }

    public abstract void Clear();
}
