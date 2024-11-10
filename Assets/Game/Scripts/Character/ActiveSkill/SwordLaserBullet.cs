using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLaserBullet : MonoBehaviour
{
    SkinnedMeshRenderer SkinnedMeshRenderer;
    int curValueBlend = 50;
    int targetBlend = 50;
    int valuePerFrame = 10;
    /// <summary>
    /// require value % 10 == 0
    /// </summary>
    [SerializeField] int maxBlendValue = 150;
    [SerializeField] int centerBlendValue = 50;
    /// <summary>
    /// require value % 10 == 0
    /// </summary>
    [SerializeField] int minBlendValue = -50;
    Material materialLaser;
    [SerializeField] bool disableBlendShapde = false;
    private void Awake()
    {
        SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        materialLaser = SkinnedMeshRenderer.materials[0];
    }

    /// <summary>
    /// state == 1 : Left
    /// state == -1 : Right
    /// state == 0 : Center
    /// </summary>
    /// <param name="state"></param>
    public void Turn(float state)
    {
        if (state < 0)
        {
            targetBlend = minBlendValue;
        }
        else if (state > 0)
        {
            targetBlend = maxBlendValue;
        }
        else
        {
            targetBlend = centerBlendValue;
        }
    }
    private void Update()
    {
        if (!disableBlendShapde)
        {
            int direct = curValueBlend < targetBlend ? 1 : -1;
            if (curValueBlend != targetBlend)
            {
                curValueBlend += direct * valuePerFrame;
                SkinnedMeshRenderer.SetBlendShapeWeight(0, curValueBlend);
            }
        }

        if (materialLaser)
        {
            var offset = materialLaser.mainTextureOffset;
            offset.x += 0.05f;
            materialLaser.mainTextureOffset = offset;
        }
    }
}
