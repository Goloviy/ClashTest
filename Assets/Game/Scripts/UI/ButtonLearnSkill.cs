using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLearnSkill : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpSkillName;
    [SerializeField] TextMeshProUGUI tmpContent;
    [SerializeField] TextMeshProUGUI tmpEvol;
    [SerializeField] Image imgSkillIcon;
    //[SerializeField] Image imgEvol;
    //[SerializeField] Image imgEvol2;
    [SerializeField] Image[] imgEvols;
    [SerializeField] Sprite bgAttack;
    [SerializeField] Sprite bgSupport;
    [SerializeField] Sprite bgSupper;
    [SerializeField] GameObject goGroupStarNormal;
    [SerializeField] GameObject goGroupStarSupper;
    Image imgBG;

    [SerializeField] List<GameObject> stars;
    Button btn;
    SkillName skillType;
    Action ActionSelectSkill;
    private void Awake()
    {
        imgBG = GetComponent<Image>();
    }
    public void Init(SkillName type, Action ActionSelectSkill)
    {
        this.skillType = type;
        LoadUI();
        this.ActionSelectSkill = ActionSelectSkill;
    }
    private void LoadUI()
    {
        var data = GameData.Instance.staticData.skillsData.GetSkill(skillType);
        var skillLevel = GameData.Instance.playerData.GetLevelSkill(skillType);
        goGroupStarNormal.SetActive(!data.evolved);
        goGroupStarSupper.SetActive(data.evolved);
        var spriteBg = data.group == GroupSkill.ACTIVE ? bgAttack : bgSupport;
        spriteBg = data.evolved ? bgSupper : spriteBg;
        imgBG.overrideSprite = spriteBg;
        //main skill info
        //string txtTit2 = I2.Loc.LocalizationManager.GetTranslation(data.nameSkill);
        tmpSkillName.text = data.nameSkill;
        //string txtDes = I2.Loc.LocalizationManager.GetTranslation(data.descriptionsLevelup[skillLevel]);
        tmpContent.text = data.descriptionsLevelup[skillLevel];
        imgSkillIcon.overrideSprite = data.spriteIcon;
        //evol info
        for (int i = 0; i < imgEvols.Length; i++)
        {
            if (data.supportEvol.Count > i)
            {
                var dataSkill = GameData.Instance.staticData.skillsData.GetSkill( data.supportEvol[i]);
                imgEvols[i].overrideSprite = dataSkill.spriteIcon;
                imgEvols[i].gameObject.SetActive(true);
                bool isEquipSkill = GameStaticData.SkillWeapons.Contains(data.supportEvol[i]);
                if (!isEquipSkill)
                {
                    //show skill can evol
                    imgEvols[i].gameObject.SetActive(true);
                }
                else
                {
                    if (GameDynamicData.mainCharacter.skillHandle.GetSkill(data.supportEvol[i]).Level > 0)
                    {
                        //show equipment'skill;
                        imgEvols[i].gameObject.SetActive(true);
                    }
                    else
                        //hide equipment'skill not include;
                        imgEvols[i].gameObject.SetActive(false);
                }
            }
            else
            {
                //empty
                imgEvols[i].gameObject.SetActive(false);
            }

        }
        //stars
        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].SetActive(skillLevel >= i);
        }
        //btn
        btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        EventDispatcher.Instance.PostEvent(EventID.CHARACTER_SELECT_SKILL_LEVELUP, skillType);
        ActionSelectSkill?.Invoke();
    }
}
