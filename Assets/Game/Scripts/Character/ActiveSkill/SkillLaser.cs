using PathologicalGames;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLaser : SkillBase
{
    [SerializeField] SpringJoint2D prefabDrone;
    [SerializeField] Rigidbody2D rigidFollow;
    SpringJoint2D tfDrone;
    private float bulletScaleReal = 1f;
    [SerializeField] Transform[] listPrefabLaser;
    [SerializeField] Transform curPrefabLaser => listPrefabLaser[level - 1];

    float timeRefresh = 5f;

    [SerializeField] bool isEvoled = false;
    [ShowIf("isEvoled", true)]
    [SerializeField] GameObject prefabBurn;
    [ShowIf("isEvoled", true)]
    [SerializeField] float distanceBurn = 1f;
    [ShowIf("isEvoled", true)]
    [SerializeField] int maxBurningCount = 8;
    public override void LevelUp()
    {
        base.LevelUp();
        if (tfDrone ==null)
        {
            tfDrone = Instantiate(prefabDrone, this.transform.position, Quaternion.identity) ;
            tfDrone.connectedBody = rigidFollow;
        }
    }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
    }
    public override void FindOpponent()
    {
        mainDirect = findOpponentSystem.GetDirectNearestOpponent();
    }
    private void Update()
    {
        if (level < 1 || !owner.IsAlive)
        {
            return;
        }
        else
        {
            if (Time.time - CurTimeInterval > timeRefresh)
            {
                timeRefresh = Time.time;
                CreateBullet();
            }
        }
    }

    protected override void OnDisable()
    {
        if (tfDrone)
            GameObject.Destroy(tfDrone.gameObject);
        base.OnDisable();
    }
    private void CreateBullet()
    {
        if (tfDrone)
        {
            FindOpponent();
            CreateLaser();
            if (isEvoled)
            {
                CreateBurning();
            }
        }
        void CreateLaser()
        {
            SoundController.Instance.PlaySound(SOUND_TYPE.SKILL_DRONE_LASER);
            var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
            var tf = pool.Spawn(curPrefabLaser, tfDrone.transform.position, curPrefabLaser.rotation);
            tf.localScale = curPrefabLaser.localScale * bulletScaleReal;
            tf.transform.up = mainDirect;
        }
        void CreateBurning()
        {
            var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
            for (int i = 0; i < maxBurningCount; i++)
            {
                var spawnPos = tfDrone.transform.position + mainDirect * i * distanceBurn;
                pool.Spawn(prefabBurn, spawnPos, curPrefabLaser.rotation);
            }
        }
    }

}
