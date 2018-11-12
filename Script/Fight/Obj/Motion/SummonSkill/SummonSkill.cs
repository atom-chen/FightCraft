using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkill
{

    #region 唯一

    private static SummonSkill _Instance = null;
    public static SummonSkill Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new SummonSkill();
            }
            return _Instance;
        }
    }

    #endregion

    #region summon in fight

    private static float _ModelSizeFixed = 0.7f;
    private static string _ModelShader = "Mobile/Particles/Additive Culled";
    private static Color _ColorRed = CommonDefine.HexToColor("90141480");
    private static Color _ColorOrigin = CommonDefine.HexToColor("D4990080");
    private static Color _ColorPurple = CommonDefine.HexToColor("60149080");
    private static Color _ColorBlue = CommonDefine.HexToColor("1136BD80");

    private Dictionary<string, AI_SummonSkill> _SummonMotions = new Dictionary<string, AI_SummonSkill>();

    public void InitSummonMotions()
    {
        for (int i = 0; i < SummonSkillData.Instance._UsingSummon.Count; ++i)
        {
            if (SummonSkillData.Instance._UsingSummon[i] == null)
                continue;

            var monsterBase = SummonSkillData.Instance._UsingSummon[i].SummonRecord.MonsterBase;
            var summonMotion = ResourcePool.Instance.GetIdleMotion(SummonSkillData.Instance._UsingSummon[i].SummonRecord.MonsterBase);
            summonMotion.InitRoleAttr(SummonSkillData.Instance._UsingSummon[i]);
            summonMotion.InitMotion();
            summonMotion.Animation.transform.localScale = summonMotion.Animation.transform.localScale * _ModelSizeFixed;
            var shader = Shader.Find(_ModelShader);
            var meshRenders = summonMotion.Animation.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var meshRender in meshRenders)
            {
                var mainTex = meshRender.material.GetTexture("_MainTex");
                meshRender.material.shader = shader;
                meshRender.material.SetTexture("_MainTex", mainTex);
                if (SummonSkillData.Instance._UsingSummon[i].SummonRecord.Quality == Tables.ITEM_QUALITY.BLUE)
                {
                    meshRender.material.SetColor("_TintColor", _ColorBlue);
                }
                else if (SummonSkillData.Instance._UsingSummon[i].SummonRecord.Quality == Tables.ITEM_QUALITY.PURPER)
                {
                    meshRender.material.SetColor("_TintColor", _ColorPurple);
                }
                else if (SummonSkillData.Instance._UsingSummon[i].SummonRecord.Quality == Tables.ITEM_QUALITY.ORIGIN)
                {
                    meshRender.material.SetColor("_TintColor", _ColorOrigin);
                }
            }

            FightLayerCommon.SetFriendLayer(summonMotion);
            var summonAI = summonMotion.GetComponent<AI_SummonSkill>();
            _SummonMotions.Add(monsterBase.Id, summonAI);
        }
    }

    public bool SummonAndSkill(int idx, MotionManager masterMotion)
    {
        if (SummonSkillData.Instance._UsingSummon[idx] != null)
        {
            return SummonAndSkill(SummonSkillData.Instance._UsingSummon[idx].SummonRecord.MonsterBase.Id, 0, masterMotion);
        }
        return false;
    }

    public bool SummonAndSkill(string motionID, int skillIdx, MotionManager masterMotion)
    {
        AI_SummonSkill summonAI = null;
        if (_SummonMotions.ContainsKey(motionID))
        {
            summonAI = _SummonMotions[motionID];
        }
        else
        {
            var monsterBase = Tables.TableReader.MonsterBase.GetRecord(motionID);
            var summonMotion = ResourcePool.Instance.GetIdleMotion(monsterBase);
            summonMotion.InitRoleAttr(monsterBase);
            summonMotion.InitMotion();
            FightLayerCommon.SetFriendLayer(summonMotion);
            summonAI = summonMotion.GetComponent<AI_SummonSkill>();
        }

        Vector3 pos = masterMotion.transform.position + masterMotion.transform.forward * summonAI._SummonPosZ;
        summonAI._SelfMotion.NavAgent.enabled = false;
        summonAI._SelfMotion._CanBeSelectByEnemy = false;
        summonAI._SelfMotion.transform.position = pos;
        summonAI._SelfMotion.transform.LookAt(masterMotion.transform);
        summonAI._SelfMotion.gameObject.SetActive(true);
        summonAI.UseSkill(skillIdx);

        return true;
    }

    public void HideSummonMotion(AI_SummonSkill summonAI)
    {
        summonAI._SelfMotion.transform.position = new Vector3(0, -10000, 0);
    }

    #endregion

}
