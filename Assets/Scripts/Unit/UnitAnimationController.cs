using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitAnimationController : MonoBehaviour
{

    public Animator Animator;
    public readonly int AttackAnimDuration = 667;
    public readonly int hit_anim_length= 667;
    // Start is called before the first frame update
    void Start()
    {
        
        Animator = GetComponent<Animator>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        Animator.SetTrigger("Attack");
    }
    public void Hit()
    {
        GetComponent<UnitObject>().HpBarRefresh();
        Animator.SetTrigger("Hit");
    }
    public void Idle()
    {
        Animator.SetTrigger("Idle");
    }
    private void OnDestroy()
    {
        Animator.StopPlayback();
    }
}
