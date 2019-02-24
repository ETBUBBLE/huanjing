using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverdPlane : MonoBehaviour
{
    public Text txt_score, txt_bestscore, txt_dimond;
    public Button btn_home, btn_rank, btn_restart;
    private void Awake()
    {
        btn_home.onClick.AddListener(HomeOnClick);
        btn_rank.onClick.AddListener(RankOnClick);
        btn_restart.onClick.AddListener(RestartOnClick);

        EventCenter.AddListener(EventDefine.GameOveredPlane, Show);
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.GameOveredPlane, Show);
    }
    /// <summary>
    /// 展示
    /// </summary>
    private void Show()
    {
        gameObject.SetActive(true);
        txt_score.text = GameManager.Instance.GetGameScore().ToString();
        txt_dimond.text = "+" + GameManager.Instance.GetDimond().ToString();
        GameManager.Instance.SetBestScore(GameManager.Instance.GetGameScore());
        txt_bestscore.text = "最高分 " + GameManager.Instance.GetBestScore()[0].ToString();
        GameManager.Instance.AddDimondCount(GameManager.Instance.GetDimond());

    }
    /// <summary>
    /// 主页按钮点击
    /// </summary>
    private void HomeOnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    /// <summary>
    /// 排行按钮点击
    /// </summary>
    private void RankOnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        EventCenter.Broadcast(EventDefine.ShowRankPlane);
    }
    /// <summary>
    /// 再来一局按钮点击
    /// </summary>
    private void RestartOnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        Gamedata.IsAgainStart = true;
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
