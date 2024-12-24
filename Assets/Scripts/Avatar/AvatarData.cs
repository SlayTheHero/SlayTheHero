using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarData
{
    public readonly int ID;
    public readonly string Name;
    public readonly string Head;
    public readonly string Body;
    public readonly string Neck;
    public readonly string RArm;
    public readonly string RHand;
    public readonly string RLeg;
    public readonly string RFoot;
    public readonly string LArm;
    public readonly string LHand;
    public readonly string LLeg;
    public readonly string LFoot;

    public AvatarData(int iD, string name, string head, string body, string neck, string rArm, string rHand, string rLeg, string rFoot, string lArm, string lHand, string lLeg, string lFoot)
    {
        ID = iD;
        Name = name;
        Head = head;
        Body = body;
        Neck = neck;
        RArm = rArm;
        RHand = rHand;
        RLeg = rLeg;
        RFoot = rFoot;
        LArm = lArm;
        LHand = lHand;
        LLeg = lLeg;
        LFoot = lFoot;
    }
}
