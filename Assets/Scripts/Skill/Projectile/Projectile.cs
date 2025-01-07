using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    public static ObjectPool<GameObject> Pool = new(OnCreatedFromPool, OnGetFromPool,OnReleasedToPool);
    public Animator Animator;
    public int TargetPos;
    public float Duration;

    public void Shoot()
    {
        var pos = TargetPos > 4 ? TargetPos % 4 * 1.5f : TargetPos * -1.5f;
        transform.DOMove(new Vector2(pos, 0.4f), Duration).onComplete = () => { Attack(); };
    }
    public void Init(int target_pos, float duration)
    {
        Animator = GetComponent<Animator>();
        TargetPos = target_pos;
        Duration = duration;
    }
    static GameObject OnCreatedFromPool()
    {
        return Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"));
    }
    static void OnGetFromPool(GameObject proj)
    {
        proj.SetActive(true);
    }
    void Attack()
    {
        Animator.SetTrigger("Hit");
    }
    static void OnReleasedToPool(GameObject proj)
    {
        proj.GetComponent<Projectile>().Animator.SetTrigger("Fly");
        proj.SetActive(false);
    }

    public void TestRespawn()
    {
        Animator.SetTrigger("Fly");
        transform.position = new Vector3(-6,0,0);
    }

}
