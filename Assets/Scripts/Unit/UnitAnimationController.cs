using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitAnimationController : MonoBehaviour
{
    
    public UnityEvent Attack;
    public UnityEvent Hit;
    public UnityEvent Idle;

    public Animator Animator;
    // Start is called before the first frame update
    void Start()
    {
        Attack = new UnityEvent();
        Hit = new UnityEvent();
        Idle = new UnityEvent();
        Animator = GetComponent<Animator>();
        Attack.AddListener(OnAttack);
        Hit.AddListener(OnHit);
        Idle.AddListener(OnIdle);
        BattleManager.Instance.OnUnitInit.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAttack()
    {
        Animator.SetTrigger("Attack");
    }
    void OnHit()
    {
        Animator.SetTrigger("Idle");
    }
    void OnIdle()
    {
        Animator.SetTrigger("Idle");
    }
    void OnAttackAnimDone()
    {
        var bm = BattleManager.Instance;
        if (bm.skill_target.GetComponent<UnitAnimationController>() == null)
        {
            bm.skill_target.GetComponentInChildren<UnitAnimationController>().Hit.Invoke();
            return;
        }

        bm.skill_target.GetComponent<UnitAnimationController>().Hit.Invoke();
    }
}
