using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GradeSkillElementUI : MonoBehaviour
{
    [SerializeField] Image imgColor;
    [SerializeField] Image imgLock;
    [SerializeField] TextMeshProUGUI tmpDescriptions;

    public void Init(GradeSkillData gradeSkillData, Rarity equipRarity)
    {
        RarityData rarityData = GameData.Instance.staticData.GetRarity(gradeSkillData.rarity);
        imgLock.gameObject.SetActive((int)gradeSkillData.rarity > (int)equipRarity);
        imgLock.color = rarityData.color;
        imgColor.gameObject.SetActive((int)gradeSkillData.rarity <= (int)equipRarity);
        imgColor.overrideSprite = rarityData.border;
        //imgColor.color = rarityData.color;
        tmpDescriptions.color = (int)gradeSkillData.rarity > (int)equipRarity ? 
            new Color(0.15f,0.15f,0.15f) : Color.white;
        //string txtDes = I2.Loc.LocalizationManager.GetTranslation(gradeSkillData.descriptions);
        tmpDescriptions.text = gradeSkillData.descriptions;
    }

}
