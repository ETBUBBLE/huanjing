using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ShopPlane : MonoBehaviour
{
    private ManagerVars vars;
    private Transform parent;
    private Text txt_Name,txt_Dimond,txt_DimondCount;
    private Button btn_back,btn_Buy,btn_Select;
    private int selectIndex;
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowShopPlane, Show);
        vars = ManagerVars.GetManageVars();
        
    }
    private void Start()
    {
        Init();

        gameObject.SetActive(false);
        
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowShopPlane, Show);
    }
    private void Init()
    {
        btn_back = transform.Find("btn_back").GetComponent<Button>();
        btn_Select = transform.Find("btn_select").GetComponent<Button>();
        btn_Buy = transform.Find("btn_Buy").GetComponent<Button>();
        
        btn_Select.onClick.AddListener(SelectOnClick);
        btn_Buy.onClick.AddListener(BuyOnClick);

        btn_back.onClick.AddListener(BackOnClick);
        txt_Name = transform.Find("txt_Name").GetComponent<Text>();
        txt_Dimond = transform.Find("btn_Buy/Text").GetComponent<Text>();
        txt_DimondCount = transform.Find("Dimond/count").GetComponent<Text>();
        parent = transform.Find("ScrolRrect/Parent");
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(vars.skinSpriteList.Count * 160 + 320, 300);
       for(int i = 0; i < vars.skinSpriteList.Count; i++)
        {
            GameObject go = Instantiate(vars.skinChooseTtem, parent);
            //为解锁皮肤
            if (GameManager.Instance.GetSkinUnlocked(i)==false)
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            else
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }
            go.transform.localPosition = new Vector3(i * 160+160, 0, 0);
            go.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];         
        }
    }
    private void Show()
    {
        parent.transform.DOLocalMoveX(GameManager.Instance.GetSelectSkin() * -160, 0.2f);
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 返回按钮被点击
    /// </summary>
    private void BackOnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        EventCenter.Broadcast(EventDefine.ShowMainPlane);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 购买按钮点击
    /// </summary>
    private void BuyOnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        if (GameManager.Instance.GetDimondCount() >= vars.priceSkinList[selectIndex])
        {
            GameManager.Instance.AddDimondCount(-vars.priceSkinList[selectIndex]);
            GameManager.Instance.SetSkinUnlocked(selectIndex);
            parent.GetChild(selectIndex).GetChild(0).GetComponent<Image>().color = Color.white;
        }
        else
        {
            EventCenter.Broadcast<string>(EventDefine.Hint, "钻石不足");

            Debug.Log("钻石不足，无法购买");
        }
    }

    /// <summary>
    /// 选择按钮点击
    /// </summary>
    private void SelectOnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        GameManager.Instance.UpdateSelectSkin(selectIndex);
        EventCenter.Broadcast(EventDefine.ChangeShopImage);
        EventCenter.Broadcast<int>(EventDefine.ChangeSkin, GameManager.Instance.GetSelectSkin());
        BackOnClick();
    }
    private void Update()
    { 
        selectIndex = (int)Mathf.Round((parent.transform.localPosition.x) / -160.0f);
        if (Input.GetMouseButtonUp(0))
        {
            parent.transform.DOLocalMoveX(selectIndex * -160, 0.2f);
        } 
        SetItemSize(selectIndex);
        RefreshUI(selectIndex);
        
    }
    /// <summary>
    /// 控制商店角色形象大小
    /// </summary>
    /// <param name="index"></param>
    private void SetItemSize(int index)
    {
        for(int i = 0 ; i < vars.skinSpriteList.Count; i++){
            if (index == i)
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
            }
            else parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
        }
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    /// <param name="index"></param>
    private void RefreshUI(int index)
    {
        txt_Name.text = vars.skinNameList[index];
        txt_Dimond.text = vars.priceSkinList[index].ToString();
        txt_DimondCount.text = GameManager.Instance.GetDimondCount().ToString();
        if (GameManager.Instance.GetSkinUnlocked(index))
        {
            btn_Select.gameObject.SetActive(true);
            btn_Buy.gameObject.SetActive(false);
        }
        else
        {
            btn_Select.gameObject.SetActive(false);
            btn_Buy.gameObject.SetActive(true);
        }
    }
}
