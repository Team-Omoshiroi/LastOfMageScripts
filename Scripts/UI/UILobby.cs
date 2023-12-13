using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobby : UIBase
{
    public void OpenOption()
    {
        var ui = UIManager.Instance.ShowUI<UIOption>();
    }
}
