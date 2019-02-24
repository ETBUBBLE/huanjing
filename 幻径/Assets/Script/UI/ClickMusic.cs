using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMusic : MonoBehaviour
{
    public AudioSource audio;
    private ManagerVars vars;
    private void Awake()
    {
        vars = ManagerVars.GetManageVars();
        EventCenter.AddListener(EventDefine.PlayAudio, PlayAudio);
    }
    private void Update()
    {
        if (GameManager.Instance.GetMusicOn())
        {
            audio.mute = false;
        }
        else audio.mute = true;
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.PlayAudio, PlayAudio);
    }
    private void PlayAudio()
    {
        audio.PlayOneShot(vars.btnClip);
    }

}
