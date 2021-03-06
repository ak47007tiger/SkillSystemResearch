﻿using System;
using System.Collections.Generic;
using DC.Collections.Generic;
using UnityEngine;

namespace DC.GameLogic
{
    [Serializable]
    public class KeyToSkill : KVPair<KeyCode, List<int>>
    {
        public KeyToSkill(KeyCode key, List<int> value) : base(key, value)
        {
        }
    }

    [CreateAssetMenu(fileName = "HeroCfg", menuName = "DC/ScriptableObjects/HeroCfg", order = 1)]
    public class HeroCfg : ScriptableObject
    {
        public int mId;

        public int mPassiveSkillId;

        public List<int> mSkillList = new List<int>();

        [Header("按键和对应技能")]
        public List<KeyToSkill> mKeyToSkillPairList = new List<KeyToSkill>();

        /// <summary>
        /// 换肤用
        /// </summary>
        /// <returns></returns>
        public string mModelPath;
        public string mPrefabPath;
        public string mName;
        public string mDesc;
        public string mUiIcon;

        public float mSpeed;

        private List<KeyCode> mSkillKeyList = new List<KeyCode>();
        private Dictionary<KeyCode, List<int>> mKeyToSkillId = new Dictionary<KeyCode, List<int>>();

        public void BuildDerivedData()
        {
            mKeyToSkillId.Clear();
            Toolkit.PairListToDictionary(mKeyToSkillPairList, mKeyToSkillId);
            mSkillKeyList.Clear();
            mSkillKeyList.AddRange(mKeyToSkillId.Keys);
        }

        public List<KeyCode> GetSkillKeyList()
        {
            return mSkillKeyList;
        }

        public int GetSkillId(KeyCode position, int index = 0)
        {
            if (mKeyToSkillId.TryGetValue(position, out var ids))
            {
                return ids[index];
            }

            return 0;
        }

        public int GetNextSkill(KeyCode position, int skillId)
        {
            if (mKeyToSkillId.TryGetValue(position, out var ids))
            {
                return ids[ids.IndexOf(skillId) + 1];
            }

            return 0;
        }

        public string GetIconPath()
        {
            return "Texture/icon_hero/" + mUiIcon;
        }
    }

}
