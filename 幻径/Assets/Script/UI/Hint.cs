using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Hint : MonoBehaviour
{
    public Image image;
    public Text text;
    private void Awake()
    {
        image.color =  new Color(image.color.r, image.color.g, image.color.b, 0);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        EventCenter.AddListener<string>(EventDefine.Hint, Show);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.Hint, Show);
    }
    private void Show(string str)
    {
        StopCoroutine("Dealy");
        transform.localPosition = new Vector3(0, -100, 0);
        transform.DOLocalMoveY(0, 0.3f).OnComplete(() =>
        {
                StartCoroutine("Dealy");
        });
        image.DOColor(new Color(image.color.r, image.color.g, image.color.b, 0.4f),0.1f);
        text.DOColor(new Color(text.color.r, text.color.g, text.color.b, 1),0.1f);

    }
    private IEnumerator Dealy()
    {
        yield return new WaitForSeconds(1f);
        transform.DOLocalMoveY(70, 0.3f);
        image.DOColor(new Color(image.color.r, image.color.g, image.color.b, 0), 0.1f);
        text.DOColor(new Color(text.color.r, text.color.g, text.color.b, 0 ), 0.1f);
    }
}
