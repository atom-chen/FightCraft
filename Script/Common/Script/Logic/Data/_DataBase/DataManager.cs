using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;
using System;

using GameBase;
//using Tables;
using GameUI;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameLogic
{
    public class DataManager
    {
        #region 单例

        private static DataManager m_Instance;
        public static DataManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new DataManager();
                }
                return m_Instance;
            }
        }

        private DataManager() { }

        #endregion

        #region MyRegion

        private Dictionary<string, DataPackBase> _DataDict = new Dictionary<string, DataPackBase>();

        public object GetData(string name)
        {
            if (_DataDict.ContainsKey(name))
            {
                return _DataDict[name];
            }

            return null;
        }

        public void Load()
        {
            foreach (var className in DataConfig._DataPackList)
            {
                DataPackBase obj = LoadClass(className);
                _DataDict.Add(className, obj);
            }
        }

        public void InitEvent()
        {
            foreach (var file in _DataDict)
            {
                file.Value.InitEvents();
            }
        }

        public void Save()
        {
            foreach (var file in _DataDict)
            {
                SaveClass(file.Value);
            }
        }

        #endregion

        #region 

        private DataPackBase LoadClass(string className)
        {
            Type classType = Type.GetType("GameLogic." + className);
            var instanceField = classType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
            var classInstance = (DataPackBase)instanceField.GetValue(null, null);
            DataPackSave.LoadData(classInstance);

            return classInstance;
        }

        private void SaveClass(DataPackBase classObj)
        {
            DataPackSave.SaveData(classObj);
            //classObj.SaveData();
        }
        #endregion

    }
}