﻿namespace DC.GameLogic
{
    public enum GameEvent
    {
        ClickEnvGround,
        /// <summary>
        /// ui按键到技能
        /// </summary>
        KeyCodeEvt,
        /// <summary>
        /// 技能状态同步给ui
        /// 技能目标设置
        /// 技能释放
        /// 技能cd结束
        /// </summary>
        SkillEvt,
    }
}