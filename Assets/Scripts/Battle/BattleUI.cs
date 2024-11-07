using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class BattleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    bool is_sizeup;

    public void OnPointerClick(PointerEventData eventData)
    {
        var bm = BattleManager.Instance;
        if ((bm.selected_skill > -1) &&
            (transform.parent.gameObject.name == "PlayerPos" ? bm.player_range[int.Parse(gameObject.name) - 1]
            : bm.hero_range[int.Parse(gameObject.name) - 1]))
        {
            bm.SelectTarget(int.Parse(gameObject.name), transform.parent.gameObject.name == "PlayerPos");
            bm.selected_skill = -1;
        }
        else
        {
            var a = GetComponentInParent<Transform>().gameObject.name;
            var b = transform.parent.gameObject.name;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var bm = BattleManager.Instance;
        var parent_name = transform.parent.name;

        if ((bm.selected_skill > -1) &&
             (parent_name == "PlayerPos" ? bm.player_range[int.Parse(gameObject.name) - 1]
            : bm.hero_range[int.Parse(gameObject.name) - 1]))
        {
            transform.DOScale(2.75f, 0.3f);
            is_sizeup = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var bm = BattleManager.Instance;
        if (is_sizeup)
        {
            transform.DOScale(2.5f, 0.3f);
            is_sizeup = false;
        }
    }
}
