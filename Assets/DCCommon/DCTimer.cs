﻿using System;
using System.Collections.Generic;
using DC.Collections.Generic;
using UnityEngine;

namespace DC
{
    public class ActionRecord
    {
        private float mTickedDuration;
        public Action mAction;
        public float mDelay;

        public ActionRecord(float delay, Action action)
        {
            mDelay = delay;
            mAction = action;
        }

        public bool IsComplete()
        {
            return mTickedDuration >= mDelay;
        }

        public void Update()
        {
            mTickedDuration += Time.deltaTime;
        }

        public void Notify()
        {
            if (mAction != null) mAction();
        }
    }

    public class DCTimer : SingletonMono<DCTimer>
    {
        HashSet<DCBaseTimer> mTimerSet = new HashSet<DCBaseTimer>();
        List<DCBaseTimer> mTimersToInvoke = new List<DCBaseTimer>();
        HashSet<DCBaseTimer> mToDelTimerSet = new HashSet<DCBaseTimer>();

        /// <summary>
        /// 物理帧执行
        /// </summary>
        HashSet<DCBaseTimer> mPhysicTimerSet = new HashSet<DCBaseTimer>();
        HashSet<DCBaseTimer> mToDelPhysicTimerSet = new HashSet<DCBaseTimer>();

        List<Action> mNextFixedUpdate = new List<Action>();

        HashSet<ActionRecord> mActionRecords = new HashSet<ActionRecord>();
        HashSet<ActionRecord> mActionRecordsToDel = new HashSet<ActionRecord>();

        void Update()
        {
            foreach (var timer in mTimerSet)
            {
                timer.Update();
            }
            //避免在timer的update中更改集合内容
            foreach (var timer in mTimersToInvoke)
            {
                timer.mOnEnd();
            }

            if (mToDelTimerSet.Count > 0)
            {
                mTimerSet.RemoveWhere(Match);
                mToDelTimerSet.Clear();
            }

            foreach (var actionRecord in mActionRecords)
            {
                actionRecord.Update();
                if (actionRecord.IsComplete())
                {
                    mActionRecordsToDel.Add(actionRecord);
                }
            }

            //防止action update的时候有往集合里面增加的操作
            if (mActionRecordsToDel.Count > 0)
            {
                mActionRecords.RemoveWhere(MatchDelRecord);
                foreach (var record in mActionRecordsToDel)
                {
                    record.Notify();
                }
                mActionRecordsToDel.Clear();
            }
            
        }

        private bool MatchDelRecord(ActionRecord record)
        {
            return mActionRecordsToDel.Contains(record);
        }

        void FixedUpdate()
        {
            foreach (var timer in mPhysicTimerSet)
            {
                timer.Update();
            }

            if (mToDelPhysicTimerSet.Count > 0)
            {
                mTimerSet.RemoveWhere(Match);
            }

            foreach (var action in mNextFixedUpdate)
            {
                if (action != null) action();
            }
        }

        private bool Match(DCBaseTimer obj)
        {
            return mToDelTimerSet.Contains(obj);
        }

        public static void AddNormal(DCBaseTimer timer)
        {
            Instance.mTimerSet.Add(timer);
        }

        public static void RemoveNormal(DCBaseTimer timer)
        {
            if (null == Instance) return;

            Instance.mToDelTimerSet.Add(timer);
        }

        public static void AddPhysic(DCBaseTimer timer)
        {
            Instance.mPhysicTimerSet.Add(timer);
        }

        public static void RemovePhysic(DCBaseTimer timer)
        {
            Instance.mToDelPhysicTimerSet.Add(timer);
        }

        public static void RunNextFixedUpdate(Action action)
        {
            Instance.mNextFixedUpdate.Add(action);
        }

        public static void RunAction(float delay, Action action)
        {
            Instance.mActionRecords.Add(new ActionRecord(delay, action));
        }

        public static void AddToInvoke(DCBaseTimer timer)
        {
            Instance.mTimersToInvoke.Add(timer);
        }
    }

    public abstract class DCBaseTimer
    {
        public Action mOnEnd;
        protected bool mPause;

        protected int mTargetLoop;
        protected int mTrackedLoop;

        protected bool mDestroyed;

        protected bool mAutoDestroy;

        protected bool mPhysic;

        public DCBaseTimer CreateNormal()
        {
            DCTimer.AddNormal(this);
            return this;
        }

        public void Destroy()
        {
            if (mDestroyed) return;
            mDestroyed = true;

            if (mPhysic)
            {
                DCTimer.RemovePhysic(this);
            }
            else
            {
                DCTimer.RemoveNormal(this);
            }
        }

        public DCBaseTimer SetAutoDestroy(bool auto)
        {
            mAutoDestroy = auto;
            return this;
        }

        public void SetPause(bool pause)
        {
            mPause = pause;
        }

        public bool IsPause()
        {
            return mPause;
        }

        public int Loop
        {
            get { return mTargetLoop; }
        }

        public int TrackedLoop
        {
            get { return mTrackedLoop; }
        }

        public void SetLoop(int loop)
        {
            mTargetLoop = loop;
        }

        public void Update()
        {
            if (mPause) return;

            //all loop completed
            if (mTargetLoop > 0 && mTrackedLoop == mTargetLoop && mAutoDestroy)
            {
                Destroy();
                return;
            }

            OnUpdate();
        }

        protected abstract void OnUpdate();

        protected virtual void Invoke()
        {
            if (null != mOnEnd)
            {
                DCTimer.AddToInvoke(this);
            }
        }
    }

    public class DCFrameTimer : DCBaseTimer
    {

        public int mTargetCnt;
        private int mTrackedCnt;

        public DCFrameTimer(int cnt, Action onEnd, int loop = 1)
        {
            mTargetCnt = cnt;
            mOnEnd = onEnd;
            mTargetLoop = loop;
        }

        public DCBaseTimer CreatePhysic()
        {
            mPhysic = true;
            DCTimer.AddPhysic(this);
            return this;
        }

        protected override void OnUpdate()
        {
            mTrackedCnt++;
            if (mTrackedCnt >= mTargetCnt)
            {
                mTrackedLoop++;

                Invoke();

                //for next loop
                mTrackedCnt = 0;
            }
        }
    }

    public class DCDurationTimer : DCBaseTimer
    {
        private float mTargetDuration;
        private float mTrackedDuration;

        public DCDurationTimer(float duration, Action onEnd, int loop = 1)
        {
            mTargetDuration = duration;
            mOnEnd = onEnd;
            mTargetLoop = loop;
        }

        protected override void OnUpdate()
        {
            mTrackedDuration += Time.deltaTime;
            if (mTrackedDuration >= mTargetDuration)
            {
                mTrackedLoop++;

                Invoke();

                //for next loop
                mTrackedDuration = 0;
            }
        }
    }
}