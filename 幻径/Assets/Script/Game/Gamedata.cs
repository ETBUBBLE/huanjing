using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gamedata
{
    public static bool IsAgainStart =false;

    private bool isFirstGame;
    private bool isMusicOn;
    private int selectSkin;
    private int[]  bestScoreArr;
    private bool[] skinUnlocked;
    private int dimondCount;
    
    public void SetFirstGame(bool isFirstGame)
    {
        this.isFirstGame = isFirstGame;
    }
    public void SetSelectSkin(int selectSkin)
    {
        this.selectSkin = selectSkin;
    }
    public void SetMusicOn(bool isMusicOn)
    {
        this.isMusicOn = isMusicOn;
    }
    public void SetbestScoreArr(int[] bestScoreArr)
    {
        this.bestScoreArr = bestScoreArr;
    }
    public void SetSkinUnlocked(bool[] skinUnlocked)
    {
        this.skinUnlocked = skinUnlocked;
    }
    public void SetdimondCount(int dimondCount)
    {
        this.dimondCount = dimondCount;
    }




    public bool GetFirstGame()
    {
       return isFirstGame;
    }
    public int GetSelectSkin()
    {
        return selectSkin;
    }
    public bool GetMusicOn()
    {
        return isMusicOn;
    }
    public int[] GetbestScoreArr()
    {
        return bestScoreArr;
    }
    public bool[] GetSkinUnlocked()
    {
        return skinUnlocked;
    }
    public int GetdimondCount()
    {
        return dimondCount;
    }

}
