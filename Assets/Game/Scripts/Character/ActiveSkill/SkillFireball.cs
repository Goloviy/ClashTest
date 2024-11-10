using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SkillFireball : SkillBase
{
    [SerializeField] SpringJoint2D prefabDrone;
    [SerializeField] Rigidbody2D rigidFollow;
    SpringJoint2D tfDrone;
    public GameObject prefabBullet;
    public GameObject prefabExplorer;
    float timeRefresh = Mathf.NegativeInfinity;
    [SerializeField] private float bulletSpeed = 7f;
    [SerializeField] private float bulletScaleReal;
    [SerializeField] private float bulletSpeedReal;
    Transform tfTarget;
    public override void LevelUp()
    {
        base.LevelUp();
        if (tfDrone == null)
        {
            tfDrone = Instantiate(prefabDrone, this.transform.position, Quaternion.identity);
            tfDrone.connectedBody = rigidFollow;
        }
    }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(bulletSpeed, this);
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
    }
    public override void FindOpponent()
    {
        tfTarget = findOpponentSystem.GetOpponentNearest();
    }
    private void Update()
    {
        if (!owner.IsAlive)
            return;
        if (level <= 0)
            return;
        if (Time.time - CurTimeInterval > timeRefresh)
        {
            timeRefresh = Mathf.Infinity;
            FindOpponent();
            CreateBullets();
        }
    }
    public override async void CreateBullets()
    {
        base.CreateBullets();
        if (tfTarget && tfDrone)
        {
            for (int i = 1; i <= CurCountBullet; i++)
            {
                CreateBullet(this.tfDrone.transform.position);
                await Task.Delay(100);
            }
        }
        timeRefresh = Time.time;

    }
    protected override void OnDisable()
    {
        if (tfDrone)
            GameObject.Destroy(tfDrone.gameObject);
        base.OnDisable();
    }
    private void CreateBullet(Vector3 startPos)
    {
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        var tf = pool.Spawn(prefabBullet, startPos, Quaternion.identity);
        var originScale = prefabBullet.transform.localScale;
        tf.localScale = originScale * bulletScaleReal;
        var distance = Vector3.Distance(tfTarget.position, tfDrone.transform.position);
        var lifeTimeBullet = distance / bulletSpeedReal;
        tf.DOMove(tfTarget.position, lifeTimeBullet).SetEase(Ease.Linear).onComplete += () =>
        {
            if (pool.IsSpawned(tf))
            {
                var tfExplosion = pool.Spawn(prefabExplorer, tf.position, Quaternion.identity);
                tfExplosion.localScale = originScale * bulletScaleReal;
                tf.localScale = originScale;
                pool.Despawn(tf);
            }

        };
    }
}
