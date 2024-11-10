using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSkillReward : MonoBehaviour
{
    [SerializeField] bool isMoveUp = true;
    [SerializeField] float moveX = -120f;
    [SerializeField] Image imgIcon;
    [SerializeField] TextMeshProUGUI tmpTitle;
    [SerializeField] TextMeshProUGUI tmpContent;
    [SerializeField] Image[] highlights;
    [SerializeField] GameObject goContent;
    [SerializeField] GameObject goItem;
    ElementSkillUIData elementData;
    Action action;

    bool isSkip = false;
    public void OnSkip()
    {
        DOTween.Kill(GetInstanceID());
        isSkip = true;
        //OnNextStep();
    }

    public void Init(ElementSkillUIData elementData, Action OnFinish)
    {
        this.action = OnFinish;
        this.elementData = elementData;
        LoadUI();
        goContent.SetActive(false);
        goItem.SetActive(true);
        goItem.transform.DORotate(new Vector3(0, 720, 0), 0.8f, RotateMode.FastBeyond360).SetUpdate(true).onComplete += OnRollFinish;
        if (isMoveUp)
        {
            this.transform.DOLocalMoveY(150f, 1.2f).SetUpdate(true).SetId(GetInstanceID()).onComplete += OnMoveUpFinish;
        }
        else
        {
            OnMoveUpFinish();
        }
    }

    private void OnRollFinish()
    {
        var data = GameData.Instance.staticData.skillsData.GetSkill(elementData.skillName);
        imgIcon.overrideSprite = data.spriteIcon;
    }

    private void OnMoveUpFinish()
    {
        goItem.transform.DOLocalMoveX(moveX, 0.3f).SetUpdate(true).SetId(GetInstanceID()).onComplete += OnMoveLeftFinish;
    }

    private async void OnMoveLeftFinish()
    {
        goContent.SetActive(true);
        await Task.Delay(3000);
        OnNextStep();
    }

    private void OnNextStep()
    {
        if (!isSkip)
        {
            this.action?.Invoke();
        }
    }

    private void LoadUI()
    {
        var data = GameData.Instance.staticData.skillsData.GetSkill(elementData.skillName);
        //string txtTit2 = I2.Loc.LocalizationManager.GetTranslation(data.nameSkill);
        tmpTitle.text = data.nameSkill;
        //string txtDes = I2.Loc.LocalizationManager.GetTranslation(data.descriptionsLevelup[elementData.level - 1 < 0 ? 0 : elementData.level - 1]);
        tmpContent.text = data.descriptionsLevelup[elementData.level - 1 < 0 ? 0 : elementData.level - 1];
        for (int i = 0; i < highlights.Length; i++)
        {
            if (i == elementData.level - 1)
            {
                highlights[i].transform.DOPunchScale(Vector3.one * 0.05f, 3f, 3)
                    .SetId(GetInstanceID())
                    .SetEase(Ease.Linear);
            }
            highlights[i].gameObject.SetActive(i < elementData.level);
        }
    }
}