using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Handle Data", menuName = "Scriptable Object/Weapon Handle Data", order = int.MaxValue)]
public class WeaponHandleData : ScriptableObject
{
    public Vector3 m_LocalPosition;
    public Vector3 m_LocalRotation;
    public Vector3 m_LocalScale;
}
