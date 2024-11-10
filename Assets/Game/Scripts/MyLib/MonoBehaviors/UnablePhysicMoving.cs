using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class UnablePhysicMoving : MonoBehaviour
{
    [SerializeField] bool isDisableWhenTakeMagnet = true;
    //Rigidbody2D rigidbody;
    Collider2D collider;
    //Vector3 originPos;
    bool magnetState = false;
    bool isInit = false;

    int frameDelta = 5;
    int lastFrameCheck = 0;
    int frame = 0;
    Coroutine WaitActive;
    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }
    private void OnSpawned(SpawnPool pool)
    {
        magnetState = false;
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_TAKE_MAGNET, OnTakeMagnet);
        EventDispatcher.Instance.RegisterListener(EventID.MAGNET_END_EFFECT, OnEndMagnetEffect);
        isInit = true;
        lastFrameCheck = 0;
        frame = 0;
        ActivePhysic(true);

    }

    private void OnEndMagnetEffect(Component arg1, object arg2)
    {
        if (isDisableWhenTakeMagnet)
        {
            magnetState = false;
            ActivePhysic(true);
        }
    }
    private void OnDespawned()
    {
        magnetState = false;
        isInit = false;
        ActivePhysic(false);
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_TAKE_MAGNET, OnTakeMagnet);
        EventDispatcher.Instance.RemoveListener(EventID.MAGNET_END_EFFECT, OnEndMagnetEffect);

    }
    private void OnTakeMagnet(Component arg1, object arg2)
    {
        if (isDisableWhenTakeMagnet)
        {
            magnetState = true;
            ActivePhysic(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        magnetState = true;
        ActivePhysic(false);
        /// temporary fix bug when move to main character fail
        //this.StartDelayAction(2f, () => { ActivePhysic(true); });
    }
    private void FixedUpdate()
    {
        if (!isInit || magnetState)
        {
            return;
        }
        if (++frame > lastFrameCheck + frameDelta )
        {
            lastFrameCheck = frame;
            bool isInsideCam = CameraFollower.Instance.IsInsideCam(this.transform.position, 0f);
            if (isInsideCam)
            {
                ActivePhysic(true);
            }
            else
            {
                ActivePhysic(false);
            }
        }

    }
    private void ActivePhysic(bool isActive)
    {
        if (collider.enabled != isActive)
            collider.enabled = isActive;
    }
}
