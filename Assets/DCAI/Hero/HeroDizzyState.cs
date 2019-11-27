﻿using DC.GameLogic;
using DC.SkillSystem;

namespace DC.AI
{
    public class HeroDizzyState : BaseHeroState
    {
        public override void Reason(object data)
        {
            base.Reason(data);

            //to idle state
            var buffEvt = GetBuffEvt(data);
            if (buffEvt != null && buffEvt.mOperate == BuffOperate.Remove)
            {
                switch (buffEvt.mBuff.mBuffCfg.mBuffType)
                {
                    case BuffType.dizzy:
                        Hero.SetTransition(Transition.ToIdle);
                        break;
                }
            }
        }
    }
}