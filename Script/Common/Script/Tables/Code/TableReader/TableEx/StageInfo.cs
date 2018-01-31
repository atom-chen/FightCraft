﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class StageInfoRecord : TableRecordBase
    {
        private List<string> _ValidScenePath;
        public List<string> ValidScenePath
        {
            get
            {
                if (_ValidScenePath == null)
                {
                    _ValidScenePath = new List<string>();
                    foreach (var scenePath in ScenePath)
                    {
                        if (!string.IsNullOrEmpty(scenePath))
                        {
                            _ValidScenePath.Add(scenePath);
                        }
                    }
                }
                return _ValidScenePath;
            }
        }
    }

    public partial class StageInfo : TableFileBase
    {
    }
}