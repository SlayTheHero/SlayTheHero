using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.U2D;

public class Projectile : MonoBehaviour
{
    public static ObjectPool<GameObject> Pool = new(OnCreatedFromPool, OnGetFromPool, OnReleasedToPool);
    public enum ProjectileType
    {
        Line, Parabola, Missile
    }
    TrajectoryDataSO data;

    public Animator Animator;

    public Vector3 TargetPos;
    public Vector3 m_OriginPos;
    Vector3 m_PrevPos;
    private void Awake()
    {
        m_PrevPos = transform.position;
    }
    private void Start()
    {
    }
    private void Update()
    {
        ChageRotate();
    }
    public void Shoot()
    {
        gameObject.SetActive(true);
        switch (data.ProjType)
        {
            case ProjectileType.Line:
                LineShoot();
                break;
            case ProjectileType.Parabola:
                ParabolaShoot();
                break;
            case ProjectileType.Missile:
                MissileShoot();
                break;

        }
    }
    public void Init(TrajectoryDataSO _data,Vector3 target_pos)
    {
        data = _data;
        TargetPos = target_pos;
        transform.position += (Vector3)data.StartLocalPos;
    }
    static GameObject OnCreatedFromPool()
    {
        var ret = Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"));
        ret.SetActive(false);
        return ret;
    }
    static void OnGetFromPool(GameObject proj)
    {
    }
    void Release()
    {
        Pool.Release(gameObject);
    }
    static void OnReleasedToPool(GameObject proj)
    {
        proj.GetComponent<Projectile>().Animator.SetTrigger("Fly");
        proj.SetActive(false);
    }

    public void TestRespawn()
    {
        Animator.ResetTrigger("Fly");
        Animator.SetTrigger("Fly");
        transform.position = new Vector3(-6, 0, 0);
    }

    public void LineShoot()
    {
        transform.DOMove(TargetPos, data.Duration).SetEase(data.EaseType).OnComplete(Release);
    }
    void ParabolaShoot()
    {
        transform.DOBlendableMoveBy(new Vector3(0, data.Height, 0), data.Duration / 2).SetLoops(2, LoopType.Yoyo);
        transform.DOBlendableMoveBy(TargetPos - transform.position, data.Duration).SetEase(data.EaseType).OnComplete(Release);
    }
    void MissileShoot()
    {
        m_OriginPos = transform.position;
        DOTween.To(() => 0f, t => { transform.position =  MissileTrajectory(t); }, 1f, data.Duration).SetEase(data.EaseType).OnComplete(Release);
    }

    void ChageRotate()
    {
        if (m_PrevPos == transform.position)
            return;
        var d = transform.position - m_PrevPos;
        float r = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, r);
        m_PrevPos = transform.position;
    }
    Vector3 MissileTrajectory(float t)
    {
        return BezierUtility.BezierPoint(((Vector3)data.StartVelocity) + m_OriginPos, m_OriginPos, TargetPos, TargetPos, t);
    }
}
