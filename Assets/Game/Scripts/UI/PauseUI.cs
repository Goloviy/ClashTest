using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : PopupUI
{
    bool isClose = true;
    [SerializeField] Button btnResume;
    [SerializeField] Button btnHome;
    [SerializeField] ElementSkillLearned[] WeaponSkillElements;
    [SerializeField] ElementSkillLearned[] SupportSkillElements;
    [SerializeField] Transform tfParentReward;
    protected override void Awake()
    {
        base.Awake();
        btnResume.onClick.AddListener(OnClickResume);
        btnHome.onClick.AddListener(OnClickHome);
    }

    private void OnClickHome()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        Time.timeScale = 1;
        SceneManager.LoadScene("Home");
    }

    public override void Close()
    {
        base.Close();
        isClose = true;
    }
    public override void Open()
    {
        base.Open();
        isClose = false;
        LoadLearned();
    }
    private void LoadReward()
    {

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
        var elemens = isAtk ? WeaponSkillElements : SupportSkillElements;
        foreach (var item in elemens)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < list.Count; i++)
        {
            var nameSkill = list[i];
            int levelSkill = GameData.Instance.playerData.GetLevelSkill(nameSkill);
            if (i < elemens.Length )
            {
                elemens[i].Init(nameSkill, levelSkill);
            }
        }
    }
    private void OnClickResume()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        Time.timeScale = 1;
        base.Close();
    }
}
