using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveItemBase
{
    public string _SaveFileName;
    public bool _DirtyFlag;

    public void SaveClass(bool isSaveChild)
    {
        DataPackSave.SaveData(this, isSaveChild);
    }

    public void LoadClass(bool loadChild)
    {
        DataPackSave.LoadData(this, loadChild);
    }
}
