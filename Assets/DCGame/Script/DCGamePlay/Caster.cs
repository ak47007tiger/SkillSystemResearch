﻿using System.Collections.Generic;
using DC.SkillSystem;
using DC.ActorSystem;
using DC.Collections.Generic;
using DC.ValueSys;
using UnityEngine;

namespace DC.GameLogic
{
    public class Caster : GameElement, ICaster
    {
        private Dictionary<KeyCode, Skill> mCodeToSkill = new Dictionary<KeyCode, Skill>();

        protected override void Awake()
        {
            base.Awake();
        }

        public CastMsg ConsumeEnough(ISkill skill)
        {
            var values = GetActor().GetValueComponent();
            var mp = values.GetValue(GValueType.mp);
            var skillCfg = skill.GetSkillCfg();
            var consumes = skillCfg.GetConsumes();

            return CastMsg.s_Suc;
        }

        public CastMsg CdEnough(ISkill skill)
        {
            return CastMsg.s_Suc;
        }

        public Transform GetCastTransform(string name)
        {
            throw new System.NotImplementedException();
        }

        public Skill GetSkill(KeyCode key)
        {
            return mCodeToSkill.GetValEx(key);
        }

        public void SetSkill(KeyCode key, Skill skill)
        {
            mCodeToSkill[key] = skill;
        }

        public void RemoveSkill(KeyCode key)
        {
            mCodeToSkill.Remove(key);
        }

        public Skill GetLastSkill()
        {
            return null;
        }

        public bool Cast(SkillCfg skillCfg, CastCfg castCfg)
        {
            DCLog.LogEx("cast", skillCfg.mId);

            var skill = GetSkillSystem().CreateSkill(skillCfg);
            skill.SetCaster(this);
            skill.SetCastCfg(castCfg);

            /*
             判断是否可以释放
             buff
                 沉默
                 晕眩
             能量
             cd
             */

            var buffAllowCast = BuffAllowCast(skill);
            if (buffAllowCast.Error)
            {
                return false;
            }

            var consumeEnough = ConsumeEnough(skill);
            if (consumeEnough.Error)
            {
                return false;
            }

            if (CdEnough(skill).Error)
            {
                return false;
            }

            switch (skillCfg.mTargetType)
            {
                case SkillTargetType.Actor:
                {
                    foreach (var target in castCfg.mTargets)
                    {
                        if (!skill.AllowCastTo(target))
                        {
                            return false;
                        }
                    }
                    //面向目标
                    var newForward = (castCfg.mTargets[0].GetTransform().position - CacheTransform.position).normalized;
                    CacheTransform.forward = newForward;
                    break;
                }
                case SkillTargetType.Position:
                    GetActor().FaceTo((castCfg.mTargetPosition - CacheTransform.position).normalized);
                    break;
                case SkillTargetType.Direction:
                    GetActor().FaceTo(castCfg.mDirection);
                    break;
            }

            var skillTf = skill.GetTransform();
            var skillBirthTf = GetActor().GetActorPos(ActorPos.body_front);
            skillTf.position = skillBirthTf.position;
            skillTf.forward = skillBirthTf.forward;

            
            skill.Create();

            return true;
        }

        public void SetSkillActive(SkillCfg skillCfg, bool active)
        {
            throw new System.NotImplementedException();
        }

        public List<CastCfg> GetActiveCastCfgs()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCastConfig(ICastInput input)
        {
            throw new System.NotImplementedException();
        }

        public List<ISkill> GetActiveSkills()
        {
            throw new System.NotImplementedException();
        }

        public ISkill GetActiveSkill()
        {
            throw new System.NotImplementedException();
        }

        public IActor GetActor()
        {
            return Actor;
        }

        public CastMsg BuffAllowCast(ISkill skill)
        {
            var ownerBuffs = GetActor().GetBuffCmpt().GetBuffList();
            var rejectBuff = ownerBuffs.Find((buff => !buff.AllowCast(skill)));
            if (null == rejectBuff)
            {
                return CastMsg.s_Suc;
            }

            return new CastMsg(CastMsgType.buff_reject, rejectBuff);
        }
    }
}