using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Rankplane : MonoBehaviour
{
    public Text gold, silver, bronze;
    public Button back;
    public GameObject scoreList;
    public Image image;
    private void Awake()
    {

        EventCenter.AddListener(EventDefine.ShowRankPlane, Show);
        back.onClick.AddListener(BackOnclick);
       // EventCenter.AddListener(EventDefine.CloseRankPlane, BackOnclick);
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRankPlane, Show);
    }
    private void Show()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        GetScore();
        gameObject.SetActive(true);
        scoreList.transform.localScale = Vector3.zero;
        scoreList.transform.DOScale(Vector3.one, 0.1f);

        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        image.DOColor(new Color(image.color.r, image.color.g, image.color.b, 0.4f), 0.1f);

    }
    private void GetScore()
    {
        gold.text = GameManager.Instance.GetBestScore()[0].ToString();
        silver.text = GameManager.Instance.GetBestScore()[1].ToString();
        bronze.text = GameManager.Instance.GetBestScore()[2].ToString();

    }
    private void BackOnclick()
    {
        EventCenter.Broadcast(EventDefine.PlayAudio);
        image.DOColor(new Color(image.color.r, image.color.g, image.color.b, 0f), 0.1f);
        scoreList.transform.DOScale(Vector3.zero, 0.1f).OnComplete(()=> {
            gameObject.SetActive(false);
        });

    }

}
