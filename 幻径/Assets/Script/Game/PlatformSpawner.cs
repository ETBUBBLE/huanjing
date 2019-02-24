using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformGroupType
{
    Grass,Winter
}

public class PlatformSpawner : MonoBehaviour
{
    public int milestoneCount = 10;         //里程碑数
    public float fallTime;
    public float minFallTime;
    public float multiple;                  
    public Vector3 startSpawnPos;           //初始位置
    private int spawnerPlatformCount;       //生成数量
    private ManagerVars vars;               //容器，资源
    private Vector3 PlatformPosition;       // 生成位置
    private bool isLeftSpawn = false;       //左边生成
    private Sprite selectPlatformSprite;    // 选择平台背景
    private PlatformGroupType groupType;    //主题类型
    private bool spikeSpawnLeft = false;    //钉子左边生成
    private Vector3 spikePlatformPos;       //钉子生成位置
    private int afterSpawnSpikeSpawnCount;  //生成钉子后需要生成的钉子数量。
    private bool isSpawnSpike=false;        //是否生成钉子

    private void Awake()
    {
        vars = ManagerVars.GetManageVars();
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
    }
    private void Update()
    {
        if (GameManager.Instance.IsGameStarted && GameManager.Instance.IsGameovered == false && GameManager.Instance.isPause == false)
        {
            UpdateFallTime();
        }  
    }
    private void UpdateFallTime()
    {
        if (GameManager.Instance.GetGameScore() > milestoneCount)
        {
            milestoneCount *= 2;
            fallTime *= multiple;
            if (fallTime < minFallTime)
            {
                fallTime = minFallTime;
            }
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);
    }
    private void Start()
    {
        RandomPlatTheme();

        
        PlatformPosition = startSpawnPos;
        for(int i=0; i < 5; i++)
        {
            spawnerPlatformCount = 5;
            DecidePath();
        }
        //生成人物
        GameObject go = Instantiate(vars.characterPre);
        go.transform.position = new Vector3(0, -1.9f, 0);
         
    }
    /// <summary>
    /// 随机选择主题
    /// </summary>
    private void RandomPlatTheme()
    {
        int ran = Random.Range(0, vars.platformThemeSpriteList.Count);
        selectPlatformSprite = vars.platformThemeSpriteList[ran];
        if (ran == 3)
        {
            groupType = PlatformGroupType.Winter;
        }
        else groupType = PlatformGroupType.Grass;
    }

    /// <summary>
    /// 确定路径
    /// </summary>
    private void DecidePath()
    {
        if (spawnerPlatformCount > 0)
        {
            spawnerPlatformCount--;
            SpawnPlatform();
        }
        else
        {

            isLeftSpawn = !isLeftSpawn;
            spawnerPlatformCount = Random.Range(1, 4);
            SpawnPlatform();
        }
    }
    /// <summary>
    /// 生成平台
    /// </summary>
    private void SpawnPlatform()
    {
        if (isSpawnSpike)
        {
            AfterSpawnSpike();
            return;
        }
        int ranObstacleDir = Random.Range(0, 2);
        if (spawnerPlatformCount >= 1)
        {
            SpawnNormalPlatform(ranObstacleDir);
        }
        else 
        {
            int rand = Random.Range(0, 3);
            if (rand == 0)              //生成通用平台
            {
                SpawnCommonPlatformGroup(ranObstacleDir);
            }
            else if (rand == 1)         //生成主题平台
            {
                switch (groupType)
                {
                    case PlatformGroupType.Grass:
                        SpawnGrassPlatformGroup(ranObstacleDir);
                        break;
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatformGroup(ranObstacleDir);
                        break;
                    default:
                        break;
                }
            }
            else                        //生成钉子平台
            {
                int value = -1;
                if (isLeftSpawn)
                {
                    value = 0;
                }
                else
                {
                    value = 1;
                }
                SpawnSpickPlatform(value);
                afterSpawnSpikeSpawnCount = 4;
                isSpawnSpike = true;
                
                if (spikeSpawnLeft)   //钉子在左边
                {
                    spikePlatformPos = new Vector3(PlatformPosition.x - 1.65f,
                        PlatformPosition.y + vars.nextYPos, 0);
                }
                else
                {
                    spikePlatformPos = new Vector3(PlatformPosition.x + 1.65f,
                        PlatformPosition.y + vars.nextYPos, 0);
                }

            }
        }
        int randDimond = Random.Range(0, 3);
        if (randDimond == 2 && GameManager.Instance.playerMove)
        {
            GameObject go = ObjectPool.Instance.GetDimond();
            go.transform.position = new Vector3(PlatformPosition.x, PlatformPosition.y + 0.5f, 0);
            go.SetActive(true);
        }
        if (isLeftSpawn)
        {
            PlatformPosition = new Vector3(PlatformPosition.x - vars.nextXPos, PlatformPosition.y + vars.nextYPos, 0);
        }
        else
        {
            PlatformPosition = new Vector3(PlatformPosition.x + vars.nextXPos, PlatformPosition.y + vars.nextYPos, 0);
        }
       
    }
    /// <summary>
    /// 生成普通平台
    /// </summary>
    private void SpawnNormalPlatform(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetNormalPlatform();
        go.transform.position = PlatformPosition;
        go.GetComponent<PlatScript>().Init(selectPlatformSprite,fallTime,ranObstacleDir);
        go.SetActive(true);
    }
    /// <summary>
    /// 生成通用平台
    /// </summary>
    private void SpawnCommonPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetCommonPlatform();
        go.transform.position = PlatformPosition;
        go.GetComponent<PlatScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }
    /// <summary>
    /// 生成草地平台
    /// </summary> 
    private void SpawnGrassPlatformGroup(int ranObstacleDir)
    {

        GameObject go = ObjectPool.Instance.GetGrassPlatform();
        go.transform.position = PlatformPosition;
        go.GetComponent<PlatScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }
    /// <summary>
    /// 生成冬季平台
    /// </summary>
    private void SpawnWinterPlatformGroup(int ranObstacleDir)
    {
        GameObject go = ObjectPool.Instance.GetWinterPlatform();
        go.transform.position = PlatformPosition;
        go.GetComponent<PlatScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成钉子
    /// </summary>
    /// <param name="dir"></param> 是否左边啊生成
    private void SpawnSpickPlatform(int dir)
    {
        GameObject temp = null;
        if (dir == 0)
        {
            spikeSpawnLeft = false;
            temp =ObjectPool.Instance.GetSpickRight();
        }
        else
        {
            spikeSpawnLeft = true;
            temp = ObjectPool.Instance.GetSpikeLeft();
            
        }

        temp.transform.position = PlatformPosition;
        temp.GetComponent<PlatScript>().Init(selectPlatformSprite, fallTime, 1);
        temp.SetActive(true);
    }
    
    /// <summary>
    /// 生成钉子后的平台
    /// </summary>
    private void AfterSpawnSpike()
    {
        if (afterSpawnSpikeSpawnCount > 0)
        {
            afterSpawnSpikeSpawnCount--;
            GameObject temp = ObjectPool.Instance.GetNormalPlatform();
            GameObject temp2 = ObjectPool.Instance.GetNormalPlatform();
            temp.SetActive(true);
            temp2.SetActive(true);
            temp2.transform.position = spikePlatformPos;
            temp.transform.position = PlatformPosition;
            temp2.GetComponent<PlatScript>().Init(selectPlatformSprite, fallTime, 1);
            temp.GetComponent<PlatScript>().Init(selectPlatformSprite, fallTime, 1);
            if (spikeSpawnLeft) //钉子左边，原来的路径是右边。
            { 
                PlatformPosition = new Vector3(PlatformPosition.x + vars.nextXPos, PlatformPosition.y + vars.nextYPos, 0);
                spikePlatformPos = new Vector3(spikePlatformPos.x - vars.nextXPos, spikePlatformPos.y + vars.nextYPos, 0);
            }
            else
            {
                PlatformPosition = new Vector3(PlatformPosition.x - vars.nextXPos, PlatformPosition.y + vars.nextYPos, 0);
                spikePlatformPos = new Vector3(spikePlatformPos.x + vars.nextXPos, spikePlatformPos.y + vars.nextYPos, 0);
            }
            
        }
        else
        {
            isSpawnSpike = false;
            DecidePath();
        } 
    }

}
