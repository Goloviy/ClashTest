using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GachaItemResultUI : PopupUI
{
    [SerializeField] SkeletonGraphic skeletonGraphic;
    [SerializeField] GachaRewardItem prefabItem;
    [SerializeField] GachaRewardItemMini prefabItemMini;
    GachaRewardItem curItem;
    [SerializeField] Button btnClose;
    [SerializeField] Button btnSkip;
    [SerializeField] GameObject goFinishTitle;
    [SerializeField] Transform tfParentGroup;
    UserEquipment[] resultData;
    public SkeletonGraphic spineChest;
    [SpineAnimation(dataField: "spineChest")] public string animClose;
    [SpineAnimation(dataField: "spineChest")] public string animOpenning;
    [SpineAnimation(dataField: "spineChest")] public string animOpenStatic;
    //temp
    int indexShow = 0;
    bool canSkip = false;
    bool isEndReward = false;
    List<Transform> items;
    Vector3 originPosChest = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
        btnClose.onClick.AddListener(OnClickClose);
        btnSkip.onClick.AddListener(OnClickSkip);

    }
    public override void Open()
    {
        var chestData = GameData.Instance.staticData.gachaData.GetDataItemShop(GameDynamicData.curGachaType);
        skeletonGraphic.skeletonDataAsset = chestData.spineChest;
        skeletonGraphic.SetAllDirty();
        goFinishTitle.SetActive(false);
        spineChest.gameObject.SetActive(true);
        if (originPosChest == Vector3.zero)
        {
            originPosChest = spineChest.transform.localPosition;
        }
        items = new List<Transform>();
        spineChest.transform.localPosition = originPosChest;
        canSkip = false;
        isEndReward = false;
        spineChest.Clear();
        spineChest.Initialize(true);
        spineChest.AnimationState.SetAnimation(0, animClose, false);
        btnClose.gameObject.SetActive(false);
        indexShow = 0;
        base.Open();
        resultData = GameDynamicData.cachedUserEquipmentCollected;
        if (resultData != null && resultData.Length > 0)
        {
            CreateSingleItem(resultData[indexShow++]);
        }
    }
    private async void CreateSingleItem(UserEquipment elementSkill)
    {
        canSkip = false;
        spineChest.Clear();
        spineChest.Initialize(true);
        spineChest.AnimationState.SetAnimation(0, animOpenning, false);
        await Task.Delay(200);
        curItem = Instantiate(prefabItem, spineChest.transform.position + Vector3.up * 100,
            Quaternion.identity,
            this.transform);
        curItem.Init(elementSkill, OnNextSingleItem);
        canSkip = true;
    }
    private void OnNextSingleItem()
    {
        if (!canSkip)
        {
            return;
        }
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
            OnClickSkip();
        }
    }
    private void OnClickSkip()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (canSkip && !isEndReward)
        {
            if (curItem && curItem.gameObject != null)
            {
                curItem.OnSkip();
                GameObject.Destroy(curItem.gameObject);
                curItem = null;
            }
            canSkip = false;
            CreateFullItems();
        }
    }
    private void OnClickClose()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

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
        UIManagerHome.Instance.Open(PopupType.SHOP);
    }
    private async void CreateFullItems()
    {
        goFinishTitle.SetActive(true);
        spineChest.gameObject.SetActive(false);
        //spineChest.AnimationState.SetAnimation(0, animOpenStatic, false);
        foreach (var itemData in resultData)
        {
            await Task.Delay(150);
            var newItem = Instantiate(prefabItemMini, tfParentGroup);
            newItem.Init(itemData, null);
            items.Add(newItem.transform);
        }
        FinishReward();
    }
    private void FinishReward()
    {
        isEndReward = true;
        btnClose.gameObject.SetActive(true);
    }

}
