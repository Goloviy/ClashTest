using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupLearnSkill : PopupUI
{
    [SerializeField] ButtonLearnSkill[] btnSkills;
    [SerializeField] Button btnRefresh;
    [SerializeField] Image[] imgAtks;
    [SerializeField] Image[] imgBuffs;
    TextMeshProUGUI[] tmpAtks;
    TextMeshProUGUI[] tmpBuffs;
    bool isReferencesFinish = false;
    const string STR_LEVEL = "Lv.";
    protected override void Awake()
    {
        base.Awake();

        btnRefresh.onClick.AddListener(RefreshSkills);
    }
    private void OnEnable()
    {
        
    }
    private void UpdateReferencesUI()
    {
        
        if (isReferencesFinish)
        {
            return;
        }
        else
        {
            tmpAtks = new TextMeshProUGUI[imgAtks.Length];
            tmpBuffs = new TextMeshProUGUI[imgBuffs.Length];
            int i = 0;
            foreach (var item in imgAtks)
            {
                tmpAtks[i++] = item.GetComponentInChildren<TextMeshProUGUI>();

            }
            int j = 0;
            foreach (var item in imgBuffs)
            {
                tmpBuffs[j++] = item.GetComponentInChildren<TextMeshProUGUI>();

            }
            isReferencesFinish = true;
        }

    }
    public override void Open()
    {
        UpdateReferencesUI();
        base.Open();
        LoadUI();
    }
    private void LoadUI()
    {
        //btnRefresh.gameObject.SetActive(GameData.Instance.playerData.RefreshSkillAvailable);
        btnRefresh.gameObject.SetActive(true);
        LoadButtonSkill();
        LoadLearned();
    }
    private void LoadLearned()
    {
        var listAttack = GameData.Instance.playerData.tempData.ListAttackSkill;
        var listSupport = GameData.Instance.playerData.tempData.ListSupportSkill;
        LoadIconSkillLearned(listAttack, true);
        LoadIconSkillLearned(listSupport, false);
    }
    private void LoadIconSkillLearned(List<SkillName> list, bool isAtk)
    {
        var arrayIcon = isAtk ? imgAtks : imgBuffs;
        var arrayText = isAtk ? tmpAtks : tmpBuffs;
        foreach (var item in arrayIcon)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < list.Count; i++)
        {
            var nameSkill = list[i];
            var SkillData = GameData.Instance.staticData.skillsData.GetSkill(nameSkill);
            if (i < arrayIcon.Length && SkillData)
            {
                arrayIcon[i].overrideSprite = SkillData.spriteIcon;
                arrayIcon[i].gameObject.SetActive(true);
            }
            var level = GameData.Instance.playerData.GetLevelSkill(nameSkill);
            StringBuilder builder = new StringBuilder();
            if (level >= 0)
            {
                builder.Append(STR_LEVEL);
                builder.Append(level);
                arrayText[i].text = builder.ToString();
                builder.Clear();
            }
        }
    }
    private void LoadButtonSkill()
    {
        var data = GameData.Instance.playerData.tempData.GetRandom3SkillToLearn();
        int count = 3;
        for (int i = 0; i < count; i++)
        {
            var btn = btnSkills[i];
            if (data.Length > i)
            {
                btn.gameObject.SetActive(true);
                btn.Init(data[i].type, OnSelectSkill);
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }
    }

    private void OnSelectSkill()
    {
        Time.timeScale = 1f;
        Close();
    }

    public override void Close()
    {
        base.Close();
    }
    public void RefreshSkills()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        OnSuccess(1);
    }

    private void OnFail()
    {
        
    }

    private void OnSuccess(int point)
    {
        LoadButtonSkill();
    }
}
