using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public AnimatorOverrideController controller { get; set; }
    public bool AutoRelease { get; set; }

    public AnimationClip clip { get; set; }

    private Animator m_Animator;

    private Coroutine m_Coroutine;
    public void Init()
    {
        m_Animator = GetComponent<Animator>();
        controller = new AnimatorOverrideController(m_Animator.runtimeAnimatorController);
        m_Animator.runtimeAnimatorController = controller;
    }
    public void SetData(AnimationClip animationClip, bool is_auto_released)
    {
        clip = animationClip;
        AutoRelease = is_auto_released;
        controller["Effect"] = clip;
    }

    public void Release()
    {
        EffectManager.Instance.ReleaseToPool(this);
    }

    public void ResetEffect()
    {
        StopCoroutine(m_Coroutine);
        m_Animator.ResetTrigger("Invoke");
        m_Animator.ResetTrigger("Reset");
    }
    public void Invoke()
    {
        m_Animator.SetTrigger("Invoke");
        if (AutoRelease)
            m_Coroutine = StartCoroutine(AutoReleaseCoroutine(clip.length));
    }
    IEnumerator AutoReleaseCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        Release();
    }
}
