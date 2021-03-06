﻿using System;
using System.Collections;
using System.Collections.Generic;
using DC.Collections.Generic;
using DC.GameLogic;
using UnityEngine;
using DC.SkillSystem;
using DC.ValueSys;

/*
 和SkillSystem有耦合关系
 */

namespace DC.ActorSystem
{
    public interface IActorSystem
    {
        GameActor GetActor(int id);

        void AddActor(int id, GameActor actor);

        GameActor GetMainActor();

        void SetMainActor(GameActor actor);
    }

    public class ActorSys : Singleton<ActorSys>, IActorSystem
    {
        private Dictionary<int, GameActor> mIdToActor = new Dictionary<int, GameActor>();

        private GameActor mMainActor;

        public GameActor GetActor(int id)
        {
            //dic[key] if not has key, it will throw exception
            return mIdToActor.GetValEx(id);
        }

        public void AddActor(int id, GameActor actor)
        {
            mIdToActor[id] = actor;
        }

        public GameActor GetMainActor()
        {
            return mMainActor;
        }

        public void SetMainActor(GameActor actor)
        {
            mMainActor = actor;
        }
    }

    public enum RoleType
    {
        Hero,
        Soldier,
        Building,
    }

    public enum ActorSide
    {
        neutral,
        blue,
        red,
    }

    public enum SideRelation
    {
        self,
        friend,
        neutral,
        enemy,
    }

    /// <summary>
    /// </summary>
    public enum EffectType
    {
        self,
        friend,
        neutral,
        enemy,
        all,
    }
    public enum ActorPos
    {
        root,

        head,

        body,
        body_front,
        body_back,
        body_left,
        body_right,
        body_top,
        body_bottom,

        hand_left,
        hand_right,

        foot_left,
        foot_right,
    }
    
}