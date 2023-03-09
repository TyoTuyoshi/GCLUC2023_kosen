using System;
using UnityEngine;
using Utils;

namespace Actor.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Pair<string, Type> animateStates;
        
        
    }
}