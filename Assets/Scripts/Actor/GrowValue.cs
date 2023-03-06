using System;
using UnityEngine;

namespace Actor
{
    /// <summary>
    /// レベルに応じた値を計算する値
    /// </summary>
    [Serializable]
    public struct GrowValue
    {
        [SerializeField] private float baseValue;
        [SerializeField] private float growRate;

        public float GetValue(in float level)
        {
            return baseValue + growRate * Mathf.Max(1, level);
        }
    }
}