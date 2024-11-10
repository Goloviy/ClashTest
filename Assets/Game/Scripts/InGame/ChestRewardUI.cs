using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class GachaReward
{
    public int countItem;
    public int percent;
}
public class ChestRewardUI : PopupUI
{
    public GachaReward[] rewards;
    protected GachaReward playerReward;
    [SerializeField] protected ItemSkillReward prefabItem;
    [SerializeField] protected ItemSkillReward prefabItemMini;
    protected ItemSkillReward curItem;
    protected ElementSkillUIData[] resultData;
    protected int[] resultLevels;
    [SerializeField] protected GameObject goChest;
    [SerializeField] protected Button btnClose;
    [SerializeField] protected Button btnSkip;
    [SerializeField] protected Transform tfParentGroup;
    [SerializeField] protected Transform tfChest;

    public SkeletonGraphic spineChest;
    [SpineAnimation(dataField: "spineChest")] public string animClose;
    [SpineAnimation(dataField: "spineChest")] public string animOpenning;
    [SpineAnimation(dataField: "spineChest")] public string animOpenStatic;
    protected int indexShow = 0;
    protected bool canSkip = false;
    protected bool isEndReward = false;
    protected List<Transform> items;
    protected Vector3 originPosChest = Vector3.zero;

    protected override void Awake()
    {
        items = new List<Transform>();
        base.Awake();
        btnClose.onClick.AddListener(OnClickClose);
        btnSkip.onClick.AddListener(OnClickSkip);

    }

    protected virtual void OnClickSkip()
    {
        if (canSkip && !isEndReward)
        {
            if (curItem && curItem.gameObject != null)
            {
                curItem.OnSkip();
                GameObject.Destroy(curItem.gameObject);
                curItem = null;
            }
            tfChest.DOLocalMove(originPosChest + Vector3.down * 100, 0.2f).SetUpdate(true).SetId(GetInstanceID());
            CreateFullItems();
        }
    }
    protected void CreateFullItems()
    {
        spineChest.AnimationState.SetAnimation(0, animOpenStatic, false);
        foreach (var itemData in resultData)
        {
            var newItem = Instantiate(prefabItemMini, tfParentGroup);
            newItem.Init(itemData, null);
            items.Add(newItem.transform);
        }
        FinishReward();
    }
    protected void OnClickClose()
    {
        DOTween.Kill(GetInstanceID());
        if (items.Count > 0)
        {
            foreach (var item in items)
            {
                GameObject.Destroy(item.gameObject);
            }
            items.Clear();
        }
        this.Close();
        Time.timeScale = 1f;
    }

    public override void Open()
    {
        if (GameDynamicData.mainCharacter == null)
        {
            return;
        }
        if (originPosChest == Vector3.zero)
        {
            originPosChest = tfChest.localPosition;
        }
        tfChest.localPosition = originPosChest;
        canSkip = false;
        isEndReward = false;
        spineChest.AnimationState.SetAnimation(0, animClose, false);
        btnClose.gameObject.SetActive(false);
        indexShow = 0;
        base.Open();
        RollResultMachine();
        resultData = GetResultSkill(playerReward.countItem);
        if (resultData !=null && resultData.Length > 0)
        {
            CreateSingleItem(resultData[indexShow++]);
        }
    }
    protected async void CreateSingleItem(ElementSkillUIData elementSkill)
    {
        canSkip = false;
        spineChest.AnimationState.SetAnimation(0, animOpenning, false);
        await Task.Delay(300);
        if (curItem != null)
        {
            Destroy(curItem.gameObject);
        }
        curItem = Instantiate(prefabItem, goChest.transform.position + Vector3.up * 100, 
            Quaternion.identity, 
            this.transform);
        curItem.Init(elementSkill, OnNextSingleItem);
        canSkip = true;
    }

    protected virtual void OnNextSingleItem()
    {
        if (curItem)
        {
            GameObject.Destroy(curItem.gameObject);
            curItem = null;
        }
        if (indexShow < resultData.Length)
        {

            CreateSingleItem(resultData[indexShow++]);
        }
        else
        {
            CreateFullItems();
            //FinishReward();
        }
    }

    protected void FinishReward()
    {
        isEndReward = true;
        btnClose.gameObject.SetActive(true);
    }

    protected void RollResultMachine()
    {
        //random reward
        int totalPercent = 0;
        foreach (var reward in rewards)
        {
            totalPercent += reward.percent;
        }
        int rd = Random.Range(0, totalPercent);
        foreach (var reward in rewards)
        {
            rd -= reward.percent;
            if (rd <= 0)
            {
                playerReward = reward;
                break;
            }
        }
    }

    protected ElementSkillUIData[] GetResultSkill(int count)
    {
        ElementSkillUIData[] eUIDatas = new ElementSkillUIData[count];
        for (int i = 0; i < count; i++)
        {
            var skillsCanlearn = GameData.Instance.playerData.tempData.GetSkillLearnedCanUpLevel();
            var defaulSkills = GameData.Instance.staticData.skillsData.GetDefaultSkill();
            SkillData rdSkill;
            if (skillsCanlearn.Count > 0)
            {
                int rd = Random.Range(0, skillsCanlearn.Count);
                var skillData = GameData.Instance.staticData.skillsData.GetSkill(skillsCanlearn[rd]);
                rdSkill = skillData;

            }
            else
            {
                int rd = Random.Range(0, defaulSkills.Length);
                rdSkill = defaulSkills[rd];

            }
            GameDynamicData.mainCharacter.LearnSkill(rdSkill.type);
            var skillLevel = GameData.Instance.playerData.GetLevelSkill(rdSkill.type);
            eUIDatas[i] = new ElementSkillUIData(rdSkill.type, skillLevel);
        }
        return eUIDatas;
    }
    protected ElementSkillUIData[] AddMoreSkill(int count, ElementSkillUIData[] skills)
    {
        ElementSkillUIData[] resultArrays = new ElementSkillUIData[skills.Length + count];
        var newSkills = GetResultSkill(count);
        skills.CopyTo(resultArrays, 0);
        newSkills.CopyTo(resultArrays, skills.Length);
        return resultArrays;

    }
}
public class ElementSkillUIData
{
    public SkillName skillName;
    public int level;

    public ElementSkillUIData(SkillName skillName, int level)
    {
        this.skillName = skillName;
        this.level = level;
    }
}