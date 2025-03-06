using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewTrajectoryDataSO", menuName = "ScriptableObject/TrajectoryData")]
public class TrajectoryDataSO : ScriptableObject
{
    public Projectile.ProjectileType ProjType;
    public Ease EaseType;
    public float Duration;
    public float Height;
    public Vector2 StartVelocity;
    public Vector2 StartLocalPos;
}
