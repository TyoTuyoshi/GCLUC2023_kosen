using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TipsUI : MonoBehaviour
    {
        private readonly SortedSet<Tip> _tips = new();
        private TextMeshProUGUI _text;

        private void Start()
        {
            TryGetComponent(out _text);
        }

        private void Update()
        {
            if (_tips.Any()) _text.text = _tips.First().Text;
        }

        /// <summary>
        ///     Tipsを表示用リストに追加
        /// </summary>
        /// <param name="text">表示内容</param>
        /// <param name="priority">優先順位</param>
        public Tip AddTips(string text, int priority = 0)
        {
            var tip = new Tip
            {
                Text = text,
                Priority = priority
            };
            _tips.Add(tip);
            return tip;
        }

        public bool RemoveTips(Tip tip)
        {
            return _tips.Remove(tip);
        }

        public class Tip : IComparable<Tip>
        {
            public string Text { get; init; }
            public int Priority { get; init; }

            public int CompareTo(Tip other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                return Priority.CompareTo(other.Priority);
            }
        }
    }
}