using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBulletRespawn : DrillBullet
{
    DrillSkill skill;
    float removeDistance = 12f;

    public override void Init(SkillBase _owner)
    {
        base.Init(_owner);
        skill = _owner as DrillSkill;

    }
    protected override void CheckReflect()
    {
        if (Time.time - cdReflect < lastTimeReflect)
            return;
        float deltaX = Mathf.Abs(GameDynamicData.mainCharacter.transform.position.x - this.transform.position.x);
        float deltaY = Mathf.Abs(GameDynamicData.mainCharacter.transform.position.y - this.transform.position.y);
        if (removeDistance < deltaX + deltaY && skill)
        {
            skill.CreateBullets();
            return;
        }
        var checkEdge = this.transform.CheckTriggerEdgeScreen();
        if (lastEdge == checkEdge)
        {
            return;
        }
        switch (checkEdge)
        {
            case EdgeScreenType.NONE:
                break;
            case EdgeScreenType.TOP:
                direct = Vector3.Reflect(direct, Vector3.up);
                break;
            case EdgeScreenType.LEFT:
                direct = Vector3.Reflect(direct, Vector3.right);
                break;
            case EdgeScreenType.RIGHT:
                direct = Vector3.Reflect(direct, Vector3.right);
                break;
            case EdgeScreenType.BOTTOM:
                direct = Vector3.Reflect(direct, Vector3.up);
                break;
            default:
                break;
        }
        if (checkEdge != EdgeScreenType.NONE)
        {
            Explosion();
            lastTimeReflect = Time.time;
            if (Vector3.Angle(direct, Vector3.right) < 15f)
            {
                direct = Vector3.Slerp(direct, Vector3.up, 0.5f);
            }
            else if (Vector3.Angle(direct, Vector3.up) < 15f)
            {
                direct = Vector3.Slerp(direct, Vector3.right, 0.5f);
            }
            lastEdge = checkEdge;
            this.transform.right = direct - Vector3.zero;
        }
    }
}
