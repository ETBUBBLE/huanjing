using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Gamedata data;
    

   

    /// <summary>
    /// 游戏是否开始
    /// </summary>
    public bool IsGameStarted { get; set; }
    /// <summary>
    /// 游戏是否结束
    /// </summary>
    public bool IsGameovered { get; set; }

    /// <summary>
    /// 游戏是否暂停
    /// </summary>
    public bool isPause;
    /// <summary>
    /// 游戏成绩
    /// </summary>
    private int gameScore;
    private int gameDimond;
    public bool playerMove = false;

    private bool isFirstGame;
    private bool isMusicOn;
    private int selectSkin;
    private int[] bestScoreArr;
    private bool[] skinUnlocked;
    private int dimondCount; 
 
    private ManagerVars vars;
    /// <summary>
    /// 初始化
    /// </summary>
    public void Awake()
    {
        vars = ManagerVars.GetManageVars();
        Instance = this; 
        EventCenter.AddListener(EventDefine.AddDimond, AddDimond);
        EventCenter.AddListener(EventDefine.AddGameScore, AddGameScore);
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);
        InitGameData();
    }
    /// <summary>
    /// 物体毁坏
    /// </summary>
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddDimond, AddDimond);
        EventCenter.RemoveListener(EventDefine.AddGameScore, AddGameScore);
        EventCenter.RemoveListener(EventDefine.PlayerMove, PlayerMove);
    }
    /// <summary>
    /// 更新游戏分数
    /// </summary>
    private void AddGameScore()
    {
        if (IsGameovered == true || IsGameStarted == false || isPause) return;
        gameScore++;
        EventCenter.Broadcast<int>(EventDefine.UpdateScore, gameScore);
    }
    /// <summary>
    /// 获得游戏分数
    /// </summary>
    /// <returns></returns>
    public int GetGameScore()
    {
        return gameScore;
    }
    /// <summary>
    /// 人物是否移动
    /// </summary>
    private void PlayerMove()
    {
        playerMove = true;
    }
    /// <summary>
    /// 更新游戏钻石数量
    /// </summary>
    private void AddDimond()
    {
        gameDimond++;
        EventCenter.Broadcast<int>(EventDefine.UpdateDimond, gameDimond);
    }
    /// <summary>
    /// 获取游戏钻石数量
    /// </summary>
    /// <returns></returns>
    public int GetDimond()
    {
        return gameDimond;
    }
    /// <summary>
    /// 设置音效开启
    /// </summary>
    /// <param name="isMusic"></param>
    public void SetMusicOn(bool isMusic )
    {
        isMusicOn = isMusic;
    }
    /// <summary>
    /// 获得音效状态
    /// </summary>
    /// <returns></returns>
    public bool GetMusicOn()
    {
        return isMusicOn;
    }
    /// <summary>
    /// 获取皮肤状态
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public bool GetSkinUnlocked(int Index)
    {
        return skinUnlocked[Index];
    }
    /// <summary>
    /// 解锁皮肤
    /// </summary>
    /// <param name="Index"></param>
    public void SetSkinUnlocked(int Index)
    {
        skinUnlocked[Index]=true;
        Save();
    }
    /// <summary>
    /// 获得钻石数目
    /// </summary>
    /// <returns></returns>
    public int GetDimondCount()
    {
        return dimondCount;
    }
    /// <summary>
    /// 增加钻石数目
    /// </summary>
    /// <param name="price"></param>
    public void AddDimondCount(int price)
    {
        dimondCount+=price;
        Save(); 
    }
    /// <summary>
    /// 更新选择皮肤
    /// </summary>
    /// <param name="index"></param>
    public void UpdateSelectSkin(int index)
    {
        selectSkin = index;
        Save();
    }
    /// <summary>
    /// 获得当前皮肤
    /// </summary>
    /// <returns></returns>
    public int GetSelectSkin()
    {
        return selectSkin;
    }

    /// <summary>
    /// 更改成绩
    /// </summary>
    /// <param name="score"></param>
    public void  SetBestScore(int score)
    {
        int temp;
        for(int i = 0; i < bestScoreArr.Length; i++)
        {
            if (score > bestScoreArr[i])
            {
                temp = score;
                score = bestScoreArr[i];
                bestScoreArr[i] = temp;
            }
        }
        Save();
    }
    /// <summary>
    /// 获得最高的三个成绩
    /// </summary>
    /// <returns></returns>
    public int[] GetBestScore()
    {
        return bestScoreArr;
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitGameData()
    {
        Read();
        if (data != null)
        {
            isFirstGame = data.GetFirstGame();
        }
        else {
            isFirstGame = true;
        }
        if (isFirstGame)
        {
            isFirstGame = false;
            isMusicOn = true;
            bestScoreArr = new int[3];
            selectSkin=0;
            skinUnlocked = new bool[vars.skinSpriteList.Count];
            skinUnlocked[0] = true;
            dimondCount = 10;
            data = new Gamedata();
            Save();
        }
        else
        {
            isMusicOn = data.GetMusicOn();
            bestScoreArr = data.GetbestScoreArr();
            skinUnlocked = data.GetSkinUnlocked();
            selectSkin = data.GetSelectSkin();
            dimondCount = data.GetdimondCount();

        }
    }

    /// <summary>
    /// 储存数据
    /// </summary>
    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameDate.data"))
            {
                data.SetbestScoreArr(bestScoreArr);
                data.SetdimondCount(dimondCount);
                data.SetFirstGame(isFirstGame);
                data.SetMusicOn(isMusicOn);
                data.SetSkinUnlocked(skinUnlocked);
                data.SetSelectSkin(selectSkin);
               
                bf.Serialize(fs, data);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// 读取数据
    /// </summary>
    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameDate.data",FileMode.Open))
            {
                data = (Gamedata)bf.Deserialize(fs);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

}
