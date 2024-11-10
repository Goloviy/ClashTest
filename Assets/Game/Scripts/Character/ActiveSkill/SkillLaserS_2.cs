using DG.Tweening;
using PathologicalGames;
using UnityEngine;

public class SkillLaserS_2 : SkillBase
{
    public GameObject prefabBullet;
    public Transform prefabExplosion;
    Vector3 rdPos;
    float timeRefresh = 5f;
    private float bulletSpeed = 5f;
    private float bulletSpeedReal;
    private float bulletScaleReal;

    public override void FindOpponent()
    {
        base.FindOpponent();
        float distance = 0f;
        do
        {
            rdPos = findOpponentSystem.GetRandomPositionInScreen();
            distance = Vector2.Distance(this.transform.position, rdPos);
        } while (distance < 2.25f);
    }

    public void Update()
    {
        if (!owner.IsAlive)
            return;
        if (level >= 1)
        {
            float timeInvertal = CurTimeInterval;
            if (Time.time - timeRefresh > timeInvertal)
            {
                timeRefresh = Mathf.Infinity;
                CreateBullets();
            }
        }
    }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(bulletSpeed, this);
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
    }
    public override void CreateBullets()
    {

        int countBullet = CurCountBullet;
        for (int i = 0; i < countBullet; i++)
        {
            FindOpponent();
            var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
            var tf = pool.Spawn(prefabBullet, this.transform.position, Quaternion.identity);
            tf.localScale = prefabBullet.transform.localScale * bulletScaleReal;
            float timeMove = Vector3.Distance(rdPos, this.transform.position) / bulletSpeedReal;
            //tf.right = rdPos - this.transform.position;
            tf.DOMove(rdPos, timeMove).SetEase(Ease.Linear).onComplete += () =>
            {
                if (pool.IsSpawned(tf))
                {
                    var tfExplosion = pool.Spawn(prefabExplosion, tf.position, Quaternion.identity);
                    pool.Despawn(tf);
                }
            };
            this.StartDelayAction(1f, () =>
            {
                timeRefresh = Time.time;
            });
        }
    }
}
