﻿namespace DC.ss
{
    [NodeInfo(10001001, "发送事件")]
    public class Action_10001001 : Action
    {
        public int evtId;

        public override bool Exec(object userData)
        {
            return base.Exec(userData);
        }
    }
}