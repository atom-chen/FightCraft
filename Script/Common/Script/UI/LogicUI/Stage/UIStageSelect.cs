
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using GameLogic;
using UnityEngine.EventSystems;
using System;
using Tables;

using GameBase;

namespace GameUI
{
    public class UIStageSelect : UIBase
    {

        #region static funs

        public static void ShowAsyn()
        {
            Hashtable hash = new Hashtable();
            GameCore.Instance.UIManager.ShowUI("LogicUI/Stage/UIStageSelect", UILayer.PopUI, hash);
        }

        #endregion

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            InitContainer();
        }

        #region difficult panel

        public List<Toggle> _DiffToggles;
        public Text _SelectedDiffText;

        private int _SelectedDiff;

        public void OnDiffSelect(bool isOn)
        {
            if (!isOn)
                return;

            for (int i = 0; i < _DiffToggles.Count; ++i)
            {
                if (_DiffToggles[i].isOn)
                {
                    _SelectedDiff = i;
                    _SelectedDiffText.text = "Diff" + i;
                    break;
                }
            }
        }

        #endregion

        #region stagePanel

        public UIContainerSelect _StageContainer;

        private StageInfoRecord _SelectedStage;

        public void InitContainer()
        {
            _StageContainer.InitSelectContent(TableReader.StageInfo.Records.Values, null, OnSelectStage);
        }

        private void OnSelectStage(object stageObj)
        {
            StageInfoRecord stageInfo = stageObj as StageInfoRecord;
            if (stageInfo == null)
                return;

            _SelectedStage = stageInfo;

            SetStageInfo(_SelectedStage);
        }

        #endregion

        #region stageInfo

        public Text _StageName;
        public Text _StageLevel;
        public Text _StageDesc;

        private void SetStageInfo(StageInfoRecord stage)
        {
            _StageName.text = stage.Name;
            _StageLevel.text = "1";
            _StageDesc.text = stage.Desc;
        }

        public void OnEnterStage()
        {
            LogicManager.Instance.EnterFight(_SelectedStage.ScenePath);
        }

        #endregion
    }
}
