using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
     
public class GamePlane : MonoBehaviour
{
    private Button btn_Pause;
    private Button btn_Play;
    private Text txt_Score;
    private Text txt_Diamond;
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowGamePlane, Show);
        EventCenter.AddListener<int>(EventDefine.UpdateScore, UpdateScore);
        EventCenter.AddListener<int>(EventDefine.UpdateDimond, UpdateDimond);
        Init();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGamePlane, Show);
        EventCenter.RemoveListener<int>(EventDefine.UpdateScore, UpdateScore);
        EventCenter.RemoveListener<int>(EventDefine.UpdateDimond, UpdateDimond);
    }
    /// <summary>
    /// 获取控件
    /// </summary>
    private void Init()  
    {
        btn_Pause = transform.Find("pause").GetComponent<Button>();
        btn_Play = transform.Find("play").GetComponent<Button>();
        txt_Diamond = transform.Find("dimond/DiamondCount").GetComponent<Text>();
        txt_Score = transform.Find("score") .GetComponent<Text>();
        btn_Play.gameObject.SetActive(false);
        gameObject.SetActive(false);

        btn_Pause.onClick.AddListener(OnPauseButtonClick);
        btn_Play.onClick.AddListener(OnPlayButtonClick);
    }

    /// <summary>
    /// 游戏暂停
    /// </summary>
    private void OnPauseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        btn_Play.gameObject.SetActive(true);
        btn_Pause.gameObject.SetActive(false);
        Time.timeScale= 0;
        GameManager.Instance.isPause = true;
    }
    /// <summary>
    /// 游戏开始 
    /// </summary>
    private void OnPlayButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        btn_Play.gameObject.SetActive(false);
        btn_Pause.gameObject.SetActive(true);
        Time.timeScale = 1;
        GameManager.Instance.isPause = false;
    }
    
    
    /// <summary>
    /// 更新分数
    /// </summary>
    /// <param name="score"></param>
    private void UpdateScore(int score)
    {
        txt_Score.text = score.ToString();
    }
    /// <summary>
    /// 更新钻石数量
    /// </summary>
    /// <param name="dimondCount"></param>
    private void UpdateDimond(int dimondCount)
    {
        txt_Diamond.text = dimondCount.ToString();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}
