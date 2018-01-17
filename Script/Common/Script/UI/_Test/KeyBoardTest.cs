using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyBoardTest : MonoBehaviour
{

    void Update()
    {
        if (Application.loadedLevelName != GameDefine.GAMELOGIC_SCENE_NAME)
            return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            var instance = GameCore.Instance.UIManager.GetUIInstance<UIEquipRefresh>("LogicUI/EquipReset/UIEquipRefresh");
            if (instance == null)
                return;

            if (!instance.isActiveAndEnabled)
                return;

            for(int i = 0; i< 100; ++i)
                instance.OnBtnMaterial();
        }
    }

}
