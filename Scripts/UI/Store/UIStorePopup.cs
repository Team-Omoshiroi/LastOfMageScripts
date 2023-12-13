using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStorePopup : UIBase
{

    [SerializeField] private TMP_Text txtContent;

    [SerializeField] private Button btnConfirm;
    [SerializeField] private Button btnCancel;


    private Action OnConfirm;

    private void Start()
    {
        btnCancel.onClick.AddListener(Close);
        btnConfirm.onClick.AddListener(Confirm);
    }


    public void SetUP( string context , Action Confirm = null)
    {     
        txtContent.text = context;
        OnConfirm = Confirm;
    }

    void Confirm()
    {
        SoundManager.Instance.Play("Effect/MenuSFX/Abstract/abs-confirm-1", eSoundType.PlayerEffect);
        if (OnConfirm != null)
        {
            OnConfirm();
            OnConfirm = null;
        }

        Close();
    }

    void Close()
    {
        SoundManager.Instance.Play("Effect/MenuSFX/Abstract/abs-cancel-1", eSoundType.PlayerEffect);
        gameObject.SetActive(false);
    }




}
