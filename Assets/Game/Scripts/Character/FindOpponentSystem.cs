using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindOpponentSystem : MonoBehaviour
{
    [SerializeField] Transform tfMainDirect;
    Vector3 mainDirect;
    List<Transform> opponents;
    const string MOVEMENT = "Movement";
    private void Awake()
    {
        tfMainDirect.localPosition = Vector3.left;
        mainDirect = Vector3.left;
    }
    private void Update()
    {
        var moveDirection = new Vector3(UltimateJoystick.GetHorizontalAxis(MOVEMENT), 
            UltimateJoystick.GetVerticalAxis(MOVEMENT), 0);
        if (moveDirection != Vector3.zero)
        {
            tfMainDirect.localPosition = moveDirection.normalized;
            mainDirect = moveDirection.normalized;
        }
    }
    /// <summary>
    /// Direct of character (relate to controller)
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMainDirect() => mainDirect;

    public Transform GetRandomOpponentInSideCamera()
    {
        //need update opponents 
        UpdateOpponents();

        //then get random element
        if (opponents.Count > 0)
        {
            for (int i = 0; i < 10; i++)
            {
                var e = opponents.GetRandomElement();
                if (CameraFollower.Instance.IsInsideCam(e.position))
                {
                    return e;
                }
            }
            return null;
        }
        else
        {
            return null;
        }
    }
    public Transform GetOpponentNearest()
    {
        //need update opponents 
        UpdateOpponents();
        float distance = Mathf.Infinity;
        int indexOpponent = -1;
        //then get random element
        if (opponents.Count > 0)
        {
            bool hasTarget = false;
            //first opponent
            var tfFirst = GetTransformFirstOpponent();
            if (tfFirst && CameraFollower.Instance.IsInsideCam(tfFirst.position))
            {
                hasTarget = true;
            }
            else
            {
                hasTarget = false;
            }

            if (!hasTarget)
            {
                for (int i = 0; i < 20; i++)
                {
                    int rd = Random.Range(0, opponents.Count);
                    var e = opponents[rd];
                    if (CameraFollower.Instance.IsInsideCam(e.position, 0f))
                    {
                        var pDistance = e.position.x + e.position.y;
                        if (pDistance < distance)
                        {
                            indexOpponent = rd;
                        }
                    }
                }

                if (indexOpponent != -1)
                {
                    //searching near enemy from 20
                    return opponents[indexOpponent];
                }
                else
                {
                    // null opponent
                    return null;
                }
            }
            else
            {
                //first opponent
                return tfFirst;
            }
        }
        else
        {
            // null opponent
            return null;
        }

    }
    public Vector3 GetRandomPositionInScreen()
    {
        return CameraFollower.Instance.RandomInsideCam();
    }
    public Vector3 GetDirectNearestOpponent()
    {
        var tf = GetOpponentNearest();
        if (tf)
        {
            var pos = tf.position;
            Vector3 direct;
            if (pos != Vector3.zero)
                direct = Vector3.Normalize(pos - this.transform.position);
            else
                direct = Vector3.zero;
            return direct;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 GetDirectFirstOpponent()
    {
        var pos = GetPosFirstOpponent();
        Vector3 direct;
        if (CameraFollower.Instance.IsInsideCam(pos))
        {
            if (pos != Vector3.zero)
                direct = Vector3.Normalize(pos - this.transform.position);
            else
                direct = Vector3.zero;
        }
        else
        {
            direct = Vector3.zero;
            //var tf = GetRandomOpponentInSideCamera();
            //direct = Vector3.Normalize(pos - tf.position);
        }
        return direct;

    }
    public Vector3 GetRandomDirectionToOpponent(Vector3 root)
    {
        //need update opponents 
        UpdateOpponents();

        //then get random element
        if (opponents.Count > 0)
        {
            return  (opponents.GetRandomElement().position - root).normalized;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public Vector3 GetRandomPosOpponent()
    {
        //need update opponents 
        UpdateOpponents();

        //then get random element
        if (opponents.Count > 0)
        {
            return opponents.GetRandomElement().position;
        }
        else
        {
            return Vector3.zero;
        }
    }
    /// <summary>
    /// Opponents had addded sequence
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosFirstOpponent()
    {
        //need update opponents 
        UpdateOpponents();
        if (opponents.Count > 0)
        {
            return opponents[0].position;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public Transform GetTransformFirstOpponent()
    {
        //need update opponents 
        UpdateOpponents();
        if (opponents.Count > 0)
        {
            return opponents[0];
        }
        else
        {
            return null;
        }
    }
    private void UpdateOpponents()
    {
        SpawnPool pool = PoolManager.Pools[StringConst.POOL_OPPONENT_NAME];
        opponents = new List<Transform>(pool);
    }
}
