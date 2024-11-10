using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFieldSkill : SkillBase
{
    public List<GameObject> ListPrefabGravityField;
    GameObject currGravityField;
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_DIE, DeactiveSkill);
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_REVIVE, ActiveSkill);

    }

    private void DeactiveSkill(Component arg1, object arg2)
    {
        DestroyCurrentGravity();
    }
    private void ActiveSkill(Component arg1, object arg2)
    {
        CreateGravityField();
    }
    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_DIE, DeactiveSkill);
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_REVIVE, ActiveSkill);

    }
    public override void LevelUpChange()
    {
        base.LevelUpChange();
        DestroyCurrentGravity();
        CreateGravityField();
    }
    private void DestroyCurrentGravity()
    {
        if (level > 1 && currGravityField != null)
        {
            Destroy(currGravityField.gameObject);
            currGravityField = null;
        }
    }
    private void CreateGravityField()
    {
        var prefab = ListPrefabGravityField[level - 1];
        currGravityField = Instantiate(prefab, this.transform.position, Quaternion.identity, this.transform);
        //currGravityField.Init(this);
        var localScale = prefab.transform.localScale;
        currGravityField.transform.localScale = localScale * CharacterStatusHelper.CalculateBulletScale(this) ;
    }
}
