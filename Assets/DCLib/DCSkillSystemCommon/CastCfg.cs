﻿using System;
using System.Collections.Generic;
using UnityEngine;
using DC.ActorSystem;
using DC.DCPhysics;
using DC.GameLogic;

namespace DC.SkillSystem
{

    /// <summary>
    /// 释放技能的配置
    /// 位置，方向，目标
    /// 默认配置 + 运行时玩家修改的配置
    /// </summary>
    public interface ICastCfg
    {
    }

    public class CastCfg : ICastCfg
    {
        public static readonly CastCfg Empty = new CastCfg();

        public List<GameActor> mTargets;
        public Vector3 mDirection;
        public Vector3 mTargetPosition;
        /// <summary>
        /// 释放力度
        /// </summary>
        public int mPower;
        public List<int> mExtParams;
        public KeyCode mFromKey;
        /// <summary>
        /// 多段技能的后续技能
        /// </summary>
        public bool mIsSubSkill;

        public bool mPrepared;

        public CastCfg()
        {

        }

        public List<GameActor> GetTargetActors()
        {
            return mTargets;
        }

        public bool IsTarget(GameActor actor)
        {
            if (Toolkit.IsNullOrEmpty(mTargets)) return false;
            return mTargets.Contains(actor);
        }

        public IActor GetTarget()
        {
            if (Toolkit.IsNullOrEmpty(mTargets)) return null;
            return mTargets[0];
        }

        public void SetTargetActors(List<GameActor> targets)
        {
            mTargets = targets;
            mPrepared = true;
        }

        public void SetTargetActor(GameActor actor)
        {
            var list = new List<GameActor>();
            list.Add(actor);
            SetTargetActors(list);
        }

        public void SetDirection(Vector3 direction)
        {
            mDirection = direction;
            mPrepared = true;

        }

        public Vector3 GetTargetPosition()
        {
            return mTargetPosition;
        }

        public void SetTargetPosition(Vector3 position)
        {
            mTargetPosition = position;
            mPrepared = true;

        }

        public int GetPower()
        {
            return mPower;
        }

        public void SetPower(int power)
        {
            mPower = power;
        }

        public List<int> GetExtParams()
        {
            return mExtParams;
        }

        public void SetExtParams(List<int> extParams)
        {
            mExtParams = extParams;
        }
    }
}