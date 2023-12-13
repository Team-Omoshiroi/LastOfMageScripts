using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : UIBase
{
    [SerializeField] private List<Toggle> _Perf;

    [SerializeField] private List<Toggle> _FPS;

    [SerializeField] private List<Toggle> _Language;

    [SerializeField] private AudioClip Confirm;
    [SerializeField] private AudioClip Cancel;

    [SerializeField] private AudioClip BGMTest;
    [SerializeField] private AudioClip EffectTest;
    [SerializeField] private AudioClip AmbientTest;

    private RectTransform _self;
    private void Awake()
    {
        _self = GetComponentInChildren<RectTransform>();
    }

    public void PlayConfirm()
    {
        SoundManager.Instance.Play(Confirm, eSoundType.PlayerEffect);
    }
    public void PlayCancel()
    {
        SoundManager.Instance.Play(Cancel, eSoundType.PlayerEffect);
    }
    public void LowPerf(bool set)
    {
        if (set)
        {

        }
        else
        {
            if (CheckPerfAllOff())
                _Perf[0].SetIsOnWithoutNotify(true);
        }
        SoundManager.Instance.Play(Confirm, eSoundType.PlayerEffect);
    }
    public void MiddlePerf(bool set)
    {
        if (set)
        {

        }
        else
        {
            if (CheckPerfAllOff())
                _Perf[1].SetIsOnWithoutNotify(true);
        }
        SoundManager.Instance.Play(Confirm, eSoundType.PlayerEffect);
    }
    public void HighPerf(bool set)
    {
        if (set)
        {

        }
        else
        {
            if (CheckPerfAllOff())
                _Perf[2].SetIsOnWithoutNotify(true);
        }
        SoundManager.Instance.Play(Confirm, eSoundType.PlayerEffect);
    }
    private bool CheckPerfAllOff()
    {
        foreach (Toggle toggle in _Perf)
        {
            if (toggle.isOn)
                return false;
        }
        return true;
    }

    public void FPS30(bool set)
    {
        if (set)
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            if (CheckFPSAllOff())
                _FPS[0].SetIsOnWithoutNotify(true);
        }
        SoundManager.Instance.Play(Confirm, eSoundType.PlayerEffect);
    }
    public void FPS60(bool set)
    {
        if (set)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            if (CheckFPSAllOff())
                _FPS[1].SetIsOnWithoutNotify(true);
        }
        SoundManager.Instance.Play(Confirm, eSoundType.PlayerEffect);
    }
    private bool CheckFPSAllOff()
    {
        foreach (Toggle toggle in _FPS)
        {
            if (toggle.isOn)
                return false;
        }
        return true;
    }
    private Coroutine AmbientTestSound;
    //private Coroutine BGMTestSound;
    private IEnumerator StopSound(eSoundType type, float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundManager.Instance.Stop(type);
    }
    public void AmbientSound(Single set)
    {
        if (AmbientTestSound != null)
            StopCoroutine(AmbientTestSound);
        SoundManager.Instance?.VolumeSetting(eSoundType.Ambient, set / 5.0f);
        SoundManager.Instance.Play(AmbientTest, eSoundType.Ambient);
        AmbientTestSound = StartCoroutine(StopSound(eSoundType.Ambient, 5.0f));
    }
    public void EffectSound(Single set)
    {
        SoundManager.Instance?.VolumeSetting(eSoundType.PlayerEffect, set / 5.0f);
        SoundManager.Instance?.VolumeSetting(eSoundType.OtherEffect, set / 5.0f);
        SoundManager.Instance.Play(EffectTest, eSoundType.PlayerEffect);
    }
    public void BGMSound(Single set)
    {
        //if (AmbientTestSound != null)
        //    StopCoroutine(BGMTestSound);
        SoundManager.Instance?.VolumeSetting(eSoundType.Bgm, set / 5.0f);
        //SoundManager.Instance.Play(BGMTest, eSoundType.Bgm);
        //BGMTestSound = StartCoroutine(StopSound(eSoundType.Bgm, 5.0f));
    }
    public void UISize(Single set)
    {
        UIManager.Instance.UISize = set;
        SoundManager.Instance.Play(Confirm, eSoundType.PlayerEffect);
    }
    public void Korean(bool set)
    {
        if (set)
        {

        }
        else
        {
            if (CheckLanguageAllOff())
                _Language[0].SetIsOnWithoutNotify(true);
        }
        SoundManager.Instance.Play(Confirm, eSoundType.PlayerEffect);
    }
    public void English(bool set)
    {
        if (set)
        {

        }
        else
        {
            if (CheckLanguageAllOff())
                _Language[1].SetIsOnWithoutNotify(true);
        }
        SoundManager.Instance.Play(Confirm, eSoundType.PlayerEffect);
    }
    private bool CheckLanguageAllOff()
    {
        foreach (Toggle toggle in _Language)
        {
            if (toggle.isOn)
                return false;
        }
        return true;
    }

    public void Quit()
    {
        var ui = UIManager.Instance.ShowUI<UIStorePopup>("Lobby/PopupConfirm", _self);
        ui.SetUP("정말 종료하시겠습니까?", () => { Application.Quit(); });
        SoundManager.Instance.Play(Confirm, eSoundType.PlayerEffect);
    }
}
