﻿using System;
using System.Collections.Generic;

namespace DC.SkillSystem
{
    interface IBuffCmpnt
    {
        void AddBuff(Buff buff);

        bool RemoveBuff(Buff buff);

        List<Buff> GetBuffList();

        bool Contains(Buff buff);
    }

    public class BuffCmpt : IBuffCmpnt
    {
        private List<Buff> mBuffList = new List<Buff>();

        private BuffCfg mLastAddBuffCfg;
        private BuffCfg mLastRemoveBuffCfg;

        private event Action<Buff> mOnAddListeners;
        private event Action<Buff> mOnRemoveListeners;

        public void AddOnBuffAddListener(Action<Buff> listener)
        {
            mOnAddListeners += listener;
        }

        public void AddOnRemoveAddListener(Action<Buff> listener)
        {
            mOnRemoveListeners -= listener;
        }

        public void AddBuff(Buff buff)
        {
            mBuffList.Add(buff);
            mLastAddBuffCfg = buff.mBuffCfg;
            if (null != mOnAddListeners)
            {
                mOnAddListeners(buff);
            }
        }

        public bool RemoveBuff(Buff buff)
        {
            var removeBuff = mBuffList.Remove(buff);
            if (removeBuff)
            {
                mLastRemoveBuffCfg = buff.mBuffCfg;
            }
            if (null != mOnRemoveListeners)
            {
                mOnRemoveListeners(buff);
            }
            return removeBuff;
        }

        public List<Buff> GetBuffList()
        {
            return mBuffList;
        }

        public bool Contains(Buff buff)
        {
            return mBuffList.Contains(buff);
        }

        public bool Contains(BuffType bufType)
        {
            return mBuffList.Find((item) => item.mBuffCfg.mBuffType == bufType) != null;
        }

        public BuffCfg GetLastAddBuffCfg()
        {
            return mLastAddBuffCfg;
        }

        public BuffCfg GetLastRemoveBuffCfg()
        {
            return mLastRemoveBuffCfg;
        }

    }

}