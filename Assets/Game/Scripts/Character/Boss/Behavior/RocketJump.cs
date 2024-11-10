using DG.Tweening;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketJump : EnemyBehavior
{
    [SerializeField] float timeFly = 0.9f;
    [SerializeField] Transform prefabAim;
    [SerializeField] ParticleSystem particleExplosions;
    [SerializeField] Transform[] tfFirePoints;
    [SerializeField] Transform prefabBullet;
    Vector3 aimPos;
    Transform tfAim;
    [SerializeField] private float lifeTimeBullet = 1f;
    [SerializeField] private float bulletSpeed = 5f;

    protected override void Awake()
    {
        base.Awake();
       
    }
    protected override void Start()
    {
        base.Start();
    }
    public override void Select(Action<EnemyBehaviorState> action)
    {
        base.Select(action);
        turnFaceable.TurnFaceToEnemy();
        PlayAnim();
        Aim();
    }
    private void Aim()
    {
        var tfTarget = GameDynamicData.mainCharacter.transform;
        if (tfAim == null)
        {
            tfAim = Instantiate(prefabAim, this.transform.position, Quaternion.identity);
        }
        tfAim.gameObject.SetActive(true);
        tfAim.DOMove(tfTarget.position, 0.25f).onComplete += () =>
        {
            aimPos = tfTarget.position;
            tfAim.DOMove(aimPos, 0.05f);
        };
    }
    
    public override void StartLongAction()
    {
        base.StartLongAction();
        this.transform.DOMove(aimPos, timeFly).SetEase(Ease.Linear);
    }
    public override void EndLongAction()
    {
        base.EndLongAction();
        tfAim.gameObject.SetActive(false);
        this.transform.position = aimPos;
        particleExplosions.transform.position = aimPos;
        particleExplosions.gameObject.SetActive(true);
        particleExplosions.Play();
        Fire();
    }
    private void Fire()
    {
        foreach (var tfFire in tfFirePoints)
        {
            var direct = (tfFire.position - this.transform.position).normalized;
            var tfBullet = pool.Spawn(prefabBullet, this.transform.position, Quaternion.identity);
            var projecttile = tfBullet.GetComponent<EnemyProjectTile>();
            if (projecttile)
            {
                SetupBullets(projecttile);
            }
            tfBullet.right = direct;
            Vector3 lastPos = this.transform.position + (lifeTimeBullet * bulletSpeed * direct);
            tfBullet.DOMove(lastPos, lifeTimeBullet).SetEase(Ease.Linear).onComplete += () =>
            {
                if (pool.IsSpawned(tfBullet))
                {
                    pool.Despawn(tfBullet);
                }
            };
        }
    }
    public override void AnimationState_End(TrackEntry trackEntry)
    {
        base.AnimationState_End(trackEntry);
        FinishBehavior();
    }

}
