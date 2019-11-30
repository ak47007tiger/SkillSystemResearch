﻿using System;
using DC.ActorSystem;
using DC.AI;
using UnityEngine;
using UnityEngine.AI;

namespace DC.GameLogic
{
    /*
        nav移动导致 transform 移动不好用，因为限制了位置
        需要在使用 transform 移动时停止nav agent
     */

    public class HeroMoveComponent : GameElement
    {
        public NavArrivePosition mNavArrivePosition;

        protected override void Awake()
        {
            base.Awake();

            mNavArrivePosition = gameObject.GetOrAdd<NavArrivePosition>();

            MsgSys.Add<Vector3>(GameEvent.ClickEnvGround, OnClickEnvGround);
        }

        void Start()
        {
            mNavArrivePosition.mNavMeshAgent.speed = Actor.GetHeroCfg().mSpeed;
        }

        void OnClickEnvGround(Vector3 pos)
        {
            if (null == Actor || !Actor.IsPlayer())
            {
                return;
            }

            if (Actor.IsAutoMoving())
            {
                Actor.StopAutoMove();
            }

            mNavArrivePosition.StartTrace(pos, SystemPreset.move_stop_distance);
        }

        public DCFSMState CreateDCFSMState(int state)
        {
            var enumState = (MoveState) state;
            var type = Type.GetType(string.Format("DC.AI.{0}", enumState.ToString()));
            var instance = (MoveBaseState)Activator.CreateInstance(type);
            //todo d.c set up entity
            return instance;
        }
    }

    public enum MoveState
    {
        MoveIdle = 0,
        MoveForceTranslate,
        MovePosition,
        MoveStop,
        MoveTarget,
        MoveTranslate,
    }
    
}