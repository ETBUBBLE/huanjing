using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mainplane : MonoBehaviour
{
    private Button Btn_Start;
    private Button Btn_Shop;
    private Button Btn_Music;
    private Button Btn_Rank;
    private ManagerVars vars;
    private void Awake()
    {
        vars = ManagerVars.GetManageVars();
        EventCenter.AddListener(EventDefine.ShowMainPlane, Show);
        EventCenter.AddListener(EventDefine.ChangeShopImage, ChangeShopImage);
        Init();        
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ChangeShopImage, ChangeShopImage);
        EventCenter.RemoveListener(EventDefine.ShowMainPlane, Show);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Start()
    {
        if (Gamedata.IsAgainStart)
        {
            Gamedata.IsAgainStart = false;
            OnStartButtonClick();
        }
        ChangeShopImage();
        if (GameManager.Instance.GetMusicOn())
        {
            Btn_Music.transform.GetChild(0).GetComponent<Image>().sprite = vars.isMusicOn;
        }
        else
        {
            Btn_Music.transform.GetChild(0).GetComponent<Image>().sprite = vars.isMusicOff;
        }
    }
    private void ChangeShopImage()
    {
        
        Btn_Shop.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
            vars.skinSpriteList[GameManager.Instance.GetSelectSkin()];
    } 
    private void Init()
    {
        Btn_Start = transform.Find("start").GetComponent<Button>();
        Btn_Start.onClick.AddListener(OnStartButtonClick);
        Btn_Shop = transform.Find("btn/shop").GetComponent<Button>();
        Btn_Shop.onClick.AddListener(OnShopButtonClick);
        Btn_Rank = transform.Find("btn/rank").GetComponent<Button>();
        Btn_Rank.onClick.AddListener(OnRankButtonClick);
        Btn_Music = transform.Find("btn/music").GetComponent<Button>();
        Btn_Music.onClick.AddListener(OnMusicButtonClick);

    }
    /// <summary>
    /// 开始按钮点击方法
    /// </summary>
    private void OnStartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        GameManager.Instance.IsGameStarted = true;
        EventCenter.Broadcast(EventDefine.ShowGamePlane);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 商店按钮点击
    /// </summary>
    private void OnShopButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        EventCenter.Broadcast(EventDefine.ShowShopPlane);
       // gameObject.SetActive(false);
        
    }
    /// <summary>
    /// 排行按钮点击
    /// </summary>
    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ShowRankPlane);
    }
    /// <summary>
    /// 音效按钮点击
    /// </summary>
    private void OnMusicButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        if (GameManager.Instance.GetMusicOn())
        {
            GameManager.Instance.SetMusicOn(false);
            Btn_Music.transform.GetChild(0).GetComponent<Image>().sprite = vars.isMusicOff;
        }
        else
        {
            GameManager.Instance.SetMusicOn(true);
            Btn_Music.transform.GetChild(0).GetComponent<Image>().sprite = vars.isMusicOn;
        }
    }

}
