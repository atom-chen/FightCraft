using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

using GameBase;
using GameUI;

namespace GameLogic
{
    public class DataPackBase
    {


        #region save/load

        public virtual void SaveData()
        {
            
        }

        public virtual void LoadData()
        {

        }

        public virtual void ClearAllData()
        {
           
        }

        #endregion
    }
}