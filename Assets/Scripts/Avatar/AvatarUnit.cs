using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarUnit : MonoBehaviour
{
    public int AvatarID;
    SpriteRenderer Head;
    SpriteRenderer Body;
    SpriteRenderer Neck;
    SpriteRenderer RArm;
    SpriteRenderer RHand;
    SpriteRenderer RLeg;
    SpriteRenderer RFoot;
    SpriteRenderer LArm;
    SpriteRenderer LHand;
    SpriteRenderer LLeg;
    SpriteRenderer LFoot;

    private void Start()
    {
        FindAndSetSpriteRenderer();
        SetAvatar(0);
    }
    public void SetAvatar(int id)
    {
        if(Head == null)
        {
            FindAndSetSpriteRenderer();
        }
        AvatarData ad = AvatarDB.GetAvatarData(id);
        AvatarID = ad.ID;
        Head.sprite = Resources.Load<Sprite>("Avatars/"+ad.Head);
        Body.sprite = Resources.Load<Sprite>("Avatars/" + ad.Body);
        Neck.sprite = Resources.Load<Sprite>("Avatars/" + ad.Neck);
        RArm.sprite = Resources.Load<Sprite>("Avatars/" + ad.RArm);
        RHand.sprite = Resources.Load<Sprite>("Avatars/" + ad.RHand);
        RLeg.sprite = Resources.Load<Sprite>("Avatars/" + ad.RLeg);
        RFoot.sprite = Resources.Load<Sprite>("Avatars/" + ad.RFoot);
        LArm.sprite = Resources.Load<Sprite>("Avatars/" + ad.LArm);
        LHand.sprite = Resources.Load<Sprite>("Avatars/" + ad.LHand);
        LLeg.sprite = Resources.Load<Sprite>("Avatars/" + ad.LLeg);
        LFoot.sprite = Resources.Load<Sprite>("Avatars/" + ad.LFoot);

    }

    void FindAndSetSpriteRenderer()
    {
        var list = GetComponentsInChildren<SpriteRenderer>();
        Head = list[0];
        LHand = list[1];
        RHand = list[2];
        LArm = list[3];
        RArm = list[4];
        Body = list[5];
        Neck = list[6];
        RFoot = list[7];
        LFoot = list[8];
        LLeg = list[9];
        RLeg = list[10];
        foreach (var item in list)
        {
            Debug.Log(item.gameObject.name);

        }
    }
}
