using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private bool isMoveleft = false;   //左边移动
    private ManagerVars vars;          //储存数据
    public LayerMask playformLayer, obstacleLayer;    //layer
    private Vector3 nextPlatformLeft, nextPlatformRight;   //左边跳后右跳位置
    private bool isJump = true;       //是否正在跳跃
    private float size = 0.15f;

    private Rigidbody2D myBody;        //自身
    public Transform rayDown, rayLeft, rayRight;          //射线发射位置
    private SpriteRenderer spriteRenderer;
    private GameObject lastHit;
    public AudioSource audio;
    private void Awake()
    {
        vars = ManagerVars.GetManageVars();
        myBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
    }
    private void Start()
    {
        ChangeSkin(GameManager.Instance.GetSelectSkin());
    }
    private bool IsPointerOverGameobject(Vector2 mousePosition)
    {
        //创建一个点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);

        eventData.position = mousePosition;
        
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        //向点击位置发射一条射线，检测是否点击UI
        EventSystem.current.RaycastAll(eventData,raycastResults);
        return raycastResults.Count > 0;
    }
    private void Update()
    {
        //if (GameManager.Instance.GetMusicOn())
        //{
        //    audio.mute = false;
        //}
        //else audio.mute = true;
        //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    int fingerId = Input.GetTouch(0).fingerId;
        //    if (EventSystem.current.IsPointerOverGameObject(fingerId)) return;
        //}
        //else if (EventSystem.current.IsPointerOverGameObject()) return;
        if (IsPointerOverGameobject(Input.mousePosition)) return;
        if (GameManager.Instance.isPause || GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameovered) return;
        
        //跳跃判断
        if (Input.GetMouseButtonDown(0) && !isJump)
        {
            audio.PlayOneShot(vars.jumpClip);
            EventCenter.Broadcast(EventDefine.PlayerMove);
            EventCenter.Broadcast(EventDefine.DecidePath);
            isJump = true;
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x <= Screen.width / 2)
            {
                isMoveleft = true;
            }
            else
            {
                isMoveleft = false;
            }
            Jump();
        }
        //死亡判断
        if (IsRayObstacle() == true && GameManager.Instance.IsGameovered == false)
        {
            audio.PlayOneShot(vars.hitClip);
            GameObject go = ObjectPool.Instance.GetDeathEffect();
            //if(go!=null)print("碰撞障碍物");
            go.SetActive(true);
            go.transform.position = gameObject.transform.position;
            spriteRenderer.enabled = false;
            GameManager.Instance.IsGameovered = true;
            StartCoroutine(ShowGameOveredPlane());
        }
        if (myBody.velocity.y < 0 && IsRayPlatform() == false && GameManager.Instance.IsGameovered == false)
        {
            audio.PlayOneShot(vars.fallClip);
            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameovered = true;
            //调用结束面板
            StartCoroutine(ShowGameOveredPlane());
        }
        if (transform.position.y - Camera.main.transform.position.y < -6)
        {
            audio.PlayOneShot(vars.fallClip);
            GameManager.Instance.IsGameovered = true;
            StartCoroutine(ShowGameOveredPlane());
        }
    }

    private IEnumerator ShowGameOveredPlane()
    {
        yield return new WaitForSeconds(1f);
        EventCenter.Broadcast(EventDefine.GameOveredPlane);
    }
    /// <summary>
    /// 是否触碰障碍物
    /// </summary>
    /// <returns></returns>
    private bool IsRayObstacle()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(rayLeft.position, Vector2.left, size, obstacleLayer);
        RaycastHit2D RightHit = Physics2D.Raycast(rayRight.position, Vector2.right, size, obstacleLayer);
        //if(leftHit.collider!=null)Debug.Log(leftHit.collider.tag);
        //if(RightHit.collider!=null)Debug.Log(RightHit.collider.tag);
        if (leftHit.collider != null && leftHit.collider.tag == "Obstacle")
        {
            return true;
        }
        if (RightHit.collider != null && RightHit.collider.tag == "Obstacle")
        {
            return true;
        }
        return false;

    }

    /// <summary>
    /// 是否检测到平台
    /// </summary>
    /// <returns></returns>
    private bool IsRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, playformLayer);
        //if (hit.collider != null)Debug.Log(hit.collider.tag);
        if (hit.collider != null && hit.collider.tag == "Platform")
        {
            if (lastHit == null || hit.collider.gameObject != lastHit)
            {
                if (lastHit != null)
                {
                    EventCenter.Broadcast(EventDefine.AddGameScore);
                }
                lastHit = hit.collider.gameObject;

            }

            return true;
        }
        else
        {
            //if (hit.collider != null) Debug.Log(hit);
            //else print("1111");
            return false;
        }
    }

    /// <summary>
    /// 跳跃动作
    /// </summary>
    private void Jump()
    {

        if (isMoveleft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.DOMoveX(nextPlatformLeft.x, 0.2f);
            transform.DOMoveY(nextPlatformLeft.y + 0.8f, 0.15f);

        }
        else
        {
            transform.localScale = Vector3.one;
            transform.DOMoveX(nextPlatformRight.x, 0.2f);
            transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f);
        }
    }
    /// <summary>
    /// 站在平台上
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            isJump = false;
            Vector3 currentPlatformPos = collision.gameObject.transform.position;
            nextPlatformLeft = new Vector3(currentPlatformPos.x - vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
            nextPlatformRight = new Vector3(currentPlatformPos.x + vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);

        }
    }
    /// <summary>
    /// 触碰钻石
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Pickup")
        {
            //吃到钻石
            collision.gameObject.SetActive(false);
            EventCenter.Broadcast(EventDefine.AddDimond);
        }
    }

    private void ChangeSkin(int Index)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = vars.playerSkinList[Index];

    }
}
