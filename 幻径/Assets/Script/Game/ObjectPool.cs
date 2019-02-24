using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public int initSpawnCount = 5;
    private List<GameObject> normalPlatformList = new List<GameObject>();
    private List<GameObject> commonlPlatformList = new List<GameObject>();
    private List<GameObject> grassPlatformList = new List<GameObject>();
    private List<GameObject> winterPlatformList = new List<GameObject>();
    private List<GameObject> spikePlatformLeftList = new List<GameObject>();
    private List<GameObject> spikePlatformRightList = new List<GameObject>();
    private List<GameObject> deathEffectList = new List<GameObject>();
    private List<GameObject> dimondList = new List<GameObject>();

    private ManagerVars vars;

    private void Awake()
    {
        Instance = this;
        vars = ManagerVars.GetManageVars();
        Init();

    }
    private void Init()
    {
        for(int i=0;i<initSpawnCount;i++)
        {
            InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
        }

        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.commonPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.commonPlatformGroup[j], ref commonlPlatformList);
            }
        }

        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.grassPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.grassPlatformGroup[j], ref grassPlatformList);

            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.winterPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.winterPlatformGroup[j], ref winterPlatformList);
            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {

            InstantiateObject(vars.spickPlatformLeft, ref spikePlatformLeftList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.spickPlatformRight,ref spikePlatformRightList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.dimondPre, ref dimondList);
        }

    }

    /// <summary>
    /// 实例化对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="addList"></param>
    /// <returns></returns>
    private GameObject InstantiateObject(GameObject prefab,ref List<GameObject> addList)
    {
        GameObject go = Instantiate(prefab, transform);
        go.SetActive(false);
        commonlPlatformList.Add(go);
        return go;
    }
    /// <summary>
    /// 获取通用平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetNormalPlatform()
    {
        for(int i= 0; i < normalPlatformList.Count; i++)
        {
            if(normalPlatformList[i].activeInHierarchy == false)
            {
                return normalPlatformList[i];
            }
        }
        return InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
    }
    /// <summary>
    /// 获取公共组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetCommonPlatform()
    {
        for (int i = 0; i < commonlPlatformList.Count; i++)
        {
            if (commonlPlatformList[i].activeInHierarchy == false)
            {
                return commonlPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.commonPlatformGroup.Count);
        return InstantiateObject(vars.commonPlatformGroup[ran], ref commonlPlatformList);
    }
    /// <summary>
    /// 获取草地组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetGrassPlatform()
    {
        for (int i = 0; i < grassPlatformList.Count; i++)
        {
            if (grassPlatformList[i].activeInHierarchy == false)
            {
                return grassPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.grassPlatformGroup.Count);
        return InstantiateObject(vars.grassPlatformGroup[ran], ref grassPlatformList);
    }
    /// <summary>
    /// 获取冬季组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetWinterPlatform()
    {
        for (int i = 0; i < winterPlatformList.Count; i++)
        {
            if (winterPlatformList[i].activeInHierarchy == false)
            {
                return winterPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.winterPlatformGroup.Count);
        return InstantiateObject(vars.winterPlatformGroup[ran], ref winterPlatformList);

    }
    /// <summary>
    /// 获取左边钉子平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetSpikeLeft()
    {
        for (int i = 0; i < spikePlatformLeftList.Count; i++)
        {
            if (spikePlatformLeftList[i].activeInHierarchy == false)
            {
                return spikePlatformLeftList[i];
            }
        }
        return InstantiateObject(vars.spickPlatformLeft, ref spikePlatformLeftList);
    }
    /// <summary>
    /// 获取右边组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetSpickRight()
    {
        for (int i = 0; i < spikePlatformRightList.Count; i++)
        {
            if (spikePlatformRightList[i].activeInHierarchy == false)
            {
                return spikePlatformRightList[i];
            }
        }
        return InstantiateObject(vars.spickPlatformRight, ref spikePlatformRightList);
    }
    /// <summary>
    /// 获取死亡特效
    /// </summary>
    /// <returns></returns>
    public GameObject GetDeathEffect()
    {
        for (int i = 0; i < deathEffectList.Count; i++)
        {
            if (deathEffectList[i].activeInHierarchy == false)
            {
                return deathEffectList[i];
            }
        }
        return InstantiateObject(vars.deathEffectPre, ref deathEffectList);
    }
    /// <summary>
    /// 获取钻石
    /// </summary>
    /// <returns></returns>
    public GameObject GetDimond()
    {
        for (int i = 0; i < dimondList.Count; i++)
        {
            if (dimondList[i].activeInHierarchy == false)
            {
                return dimondList[i];
            }
        }
        return InstantiateObject(vars.dimondPre, ref dimondList);
    }
}
