using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class Pair<TF, TS>
    {
        [SerializeField] private TF first;
        [SerializeField] private TS second;

        public Pair(TF first, TS second)
        {
            this.first = first;
            this.second = second;
        }

        public TF First => first;
        public TS Second => second;
    }
}