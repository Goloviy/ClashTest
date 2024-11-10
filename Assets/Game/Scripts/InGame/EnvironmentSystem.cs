using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSystem : MonoBehaviour
{
    SpriteRenderer rendererBG;

    private void Awake()
    {
        rendererBG = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (GameDynamicData.CurGameMode == GameMode.CAMPAIGN)
        {
            var chapterData = GameData.Instance.staticData.GetChapterLevel(GameDynamicData.SelectChapterLevel);
            rendererBG.sprite = chapterData.bg;
            rendererBG.size = new Vector2(550, 550);
        }
    }
}
