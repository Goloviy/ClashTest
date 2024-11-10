using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ElementSkillLearned : MonoBehaviour
{
    [SerializeField] Image imgIcon;
    [SerializeField] Transform[] stars;
    private void Awake()
    {
        imgIcon = GetComponent<Image>();
        var _stars = GetComponentsInChildren<Transform>().ToList();
        Transform removeTf = null;
        foreach (var star in _stars)
        {
            if (ReferenceEquals(star, this.transform))
            {
                removeTf = star;
            }
        }
        if (removeTf != null)
        {
            _stars.Remove(removeTf);
        }
        stars = _stars.ToArray();
        this.gameObject.SetActive(false);
    }
    public void Init(SkillName skillName, int level)
    {
        if (level <= 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
            var skillData = GameData.Instance.staticData.skillsData.GetSkill(skillName);
            imgIcon.overrideSprite = skillData.spriteIcon;
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(i < level);
            }
        }
    }
}
