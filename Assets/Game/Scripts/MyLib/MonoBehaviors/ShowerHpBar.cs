using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerHpBar : MonoBehaviour
{
    [SerializeField] HpBarSprite prefabHpBarSprite;
    [SerializeField] float marginY = -1f;
    [SerializeField] float marginX = 0f;
    IAlive unit;

    private void Start()
    {
        var unit = GetComponent<IAlive>();
        if (unit != null)
        {
            var hpBar = Instantiate(prefabHpBarSprite, this.transform);
            hpBar.Init(unit, marginY , marginX);
        }
    }
}
