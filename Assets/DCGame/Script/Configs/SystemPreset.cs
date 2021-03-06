﻿using UnityEngine;

namespace DC.GameLogic
{
    public static class SystemPreset
    {
        public static float max_skill_cast_range = 200;

        public static readonly string tag_env_ground = "env_ground";

        public static bool IsGround(string tag)
        {
            return tag_env_ground.Equals(tag);
        }

        public static readonly float move_stop_distance = 0.3f;

        public static string GetConfigPath<T>()
        {
            return "Configs/" + typeof(T).Name;
        }

        public static readonly int layer_ground = LayerMask.GetMask("Ground");
        public static readonly int layer_actor = LayerMask.GetMask("Actor");

        public static float GetMiniatureValue(float value)
        {
            return value * 0.99f;
        }

        public static float MiniatureValue(this float value)
        {
            return value * 0.99f;
        }
    }
}