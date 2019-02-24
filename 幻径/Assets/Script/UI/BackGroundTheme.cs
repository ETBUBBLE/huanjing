using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundTheme : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private ManagerVars vars;
    private void Awake()
    {
        vars = ManagerVars.GetManageVars();
        spriteRenderer = GetComponent<SpriteRenderer>();
        int ranValue = Random.Range(0, vars.bgThemeSpriteList.Count);
        spriteRenderer.sprite = vars.bgThemeSpriteList[ranValue];
 
    }
}
