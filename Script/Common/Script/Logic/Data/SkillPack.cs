using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameLogic
{
    public class SkillPack : DataPackBase
    {
        #region 单例

        private static SkillPack _Instance;
        public static SkillPack Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SkillPack();
                }
                return _Instance;
            }
        }

        private SkillPack() { }

        #endregion

        [SaveField(1)]
        private List<SkillInfoItem> _SkillItems = new List<SkillInfoItem>();

        private Dictionary<Tables.SKILL_CLASS, List<SkillInfoItem>> _SkillClassItems;
        public Dictionary<Tables.SKILL_CLASS, List<SkillInfoItem>> SkillClassItems
        {
            get
            {
                if (_SkillClassItems == null)
                {
                    _SkillClassItems = new Dictionary<Tables.SKILL_CLASS, List<SkillInfoItem>>();
                    InitSkill();
                }
                return _SkillClassItems;
            }
        }

        private void InitSkill()
        {
            foreach (var skillPair in Tables.TableReader.SkillInfo.Records)
            {
                if (skillPair.Value.Profession == PlayerDataPack.Instance._SelectedRole.Profession)
                {
                    if (!_SkillClassItems.ContainsKey(skillPair.Value.SkillClass))
                    {
                        _SkillClassItems.Add(skillPair.Value.SkillClass, new List<SkillInfoItem>());
                    }

                    if (_SkillClassItems.ContainsKey(skillPair.Value.SkillClass))
                    {
                        var skillInfo = GetSkillInfo(skillPair.Value.Id);
                        if (skillInfo == null)
                        {
                            skillInfo = new SkillInfoItem(skillPair.Value.Id);
                            skillInfo._SkillLevel = 0;
                        }
                        _SkillClassItems[skillPair.Value.SkillClass].Add(skillInfo);
                    }
                }
            }
        }

        public SkillInfoItem GetSkillInfo(string skillID)
        {
            var skillItem = _SkillItems.Find((skillInfo) =>
            {
                if (skillInfo._SkillID == skillID)
                {
                    return true;
                }
                return false;
            });

            if (skillItem == null)
            {
                skillItem = new SkillInfoItem(skillID);
                skillItem._SkillLevel = 0;
                _SkillItems.Add(skillItem);
            }

            return skillItem;
        }

        public void SkillLevelUp(string skillID)
        {
            //cost

            var findSkill = _SkillItems.Find((skillInfo) =>
            {
                if (skillInfo._SkillID == skillID)
                {
                    return true;
                }
                return false;
            });

            if (findSkill == null)
            {
                findSkill = new SkillInfoItem(skillID);
                _SkillItems.Add(findSkill);
            }
            else
            {
                var skillTab = Tables.TableReader.SkillInfo.GetRecord(skillID);
                if (skillTab.MaxLevel > findSkill._SkillLevel)
                {
                    ++findSkill._SkillLevel;
                }
                
            }
        }

        #region 

        #endregion
    }
}
