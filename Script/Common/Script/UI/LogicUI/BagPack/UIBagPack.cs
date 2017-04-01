using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameBase;
using GameLogic;
namespace GameUI
{
    public class UIBagPack : UIBase
    {

        #region static funs

        public static void ShowAsyn()
        {
            Hashtable hash = new Hashtable();
            GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UIBagPack", hash);
        }

        #endregion

        #region 

        public UIContainerBase _EquipContainer;
        public UIContainerSelect _ItemsContainer;

        #endregion

        #region 

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            ShowPackItems();
        }

        private void ShowPackItems()
        {
            _EquipContainer.InitContentItem(PlayerDataPack.Instance._SelectedRole._EquipList);
            _ItemsContainer.InitSelectContent(PlayerDataPack.Instance._SelectedRole._BackPackItems, null);

        }

        #endregion

        
    }
}
