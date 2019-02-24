using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatScript : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderers;
    public GameObject obstacle;
    public bool startTime=false;
    private float falltime;
    private Rigidbody2D mybody;
    public void Awake()
    {
        mybody = GetComponent<Rigidbody2D>();
    }
    public void Init(Sprite sprite,float fallTime,int obstacleDir)
    {
        mybody.bodyType = RigidbodyType2D.Static;
        this.falltime = fallTime;
        startTime = true;
        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = sprite;
        }
        if (obstacleDir == 0)
        {
            if (obstacle != null)
            {
                
               obstacle.transform.localPosition = new Vector3(-obstacle.transform.localPosition.x, obstacle.transform.localPosition.y, obstacle.transform.localPosition.z);
            }
        }
    }
    private void Update()
    {
        if (GameManager.Instance.IsGameStarted == false) return;
        if (startTime)
        {
            falltime -= Time.deltaTime;
            if (falltime < 0)   //倒计时结束
            {
                //掉落
                startTime = false;
                if (mybody.bodyType != RigidbodyType2D.Dynamic)
                {
                    mybody.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
        if (transform.position.y - Camera.main.transform.position.y < -6)
        {
            StartCoroutine(DealyHide());
        }
    }
    private IEnumerator DealyHide()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
