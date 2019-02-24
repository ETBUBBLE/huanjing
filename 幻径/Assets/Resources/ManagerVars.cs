using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName ="CreatManagerVarsContainer")]
public class ManagerVars : ScriptableObject
{
    public static ManagerVars GetManageVars()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }
    public List<Sprite> bgThemeSpriteList = new List<Sprite>(); // 背景图片。

    public List<Sprite> platformThemeSpriteList = new List<Sprite>(); //平台图片

    public float nextXPos = 0.554f, nextYPos = 0.645f;

    public GameObject characterPre;         //人物
    public GameObject normalPlatformPre;    //普通平台


    public GameObject spickPlatformLeft;
    public GameObject spickPlatformRight;
    public GameObject deathEffectPre;

    public List<GameObject> commonPlatformGroup = new List<GameObject>();
    public List<GameObject> grassPlatformGroup = new List<GameObject>();
    public List<GameObject> winterPlatformGroup = new List<GameObject>();
    public List<string> skinNameList = new List<string>();

    public List<Sprite> skinSpriteList=new List<Sprite>();
    public List<int> priceSkinList=new List<int>();
    public List<Sprite> playerSkinList = new List<Sprite>();
    public GameObject skinChooseTtem;
    public GameObject dimondPre;
    public AudioClip fallClip, jumpClip, dimondClip, btnClip,hitClip;
    public Sprite isMusicOn, isMusicOff;
}
