using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Pool;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Prefab;
    private ObjectPool<Effect> m_Pool;
    private static EffectManager m_Instance;
    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = GameObject.Find("EffectManager").GetComponent<EffectManager>();
            return m_Instance;
        }
    }
    private void Awake()
    {
        m_Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_Pool = new ObjectPool<Effect>(OnCreateEffectFromPool, OnGetEffectFromPool, OnReleaseEffectFromPool);
    }
    Effect OnCreateEffectFromPool()
    {
        var obj = Instantiate(m_Prefab);
        obj.GetComponent<Effect>().Init();
        return obj.GetComponent<Effect>();
    }
    void OnGetEffectFromPool(Effect effect)
    {
        effect.gameObject.SetActive(true);
    }

    void OnReleaseEffectFromPool(Effect effect)
    {
        effect.ResetEffect();
        effect.gameObject.SetActive(false);
    }

    public Effect CreateEffect(AnimationClip clip, bool is_auto_released = false)
    {
        var effect = m_Pool.Get();
        effect.SetData(clip, is_auto_released);
        return effect;
    }
    public Effect CreateEffect(AnimationClip clip, Vector3 position,int layer = 0, bool is_auto_released = false)
    {
        var effect = CreateEffect(clip, is_auto_released);
        effect.transform.position = position;
        effect.gameObject.GetComponent<SpriteRenderer>().sortingOrder = layer;
        return effect;
    }
    public Effect CreateEffect(string name, bool is_auto_released = false)
    {
        var effect = m_Pool.Get();
        var clip = AnimationDB.GetAnimationClip(AnimationDB.AnimType.Effect, name);
        effect.SetData(clip, is_auto_released);
        return effect;
    }
    public Effect CreateEffect(string name, Vector3 position, bool is_auto_released)
    {
        var effect = CreateEffect(name, is_auto_released);
        effect.transform.position = position;
        return effect;
    }
    public void ReleaseToPool(Effect effect)
    {
        m_Pool.Release(effect);
    }
}
