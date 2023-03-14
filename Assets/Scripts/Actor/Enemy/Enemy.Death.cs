using System;
using AutoGenerate;
using Cysharp.Threading.Tasks;
using IceMilkTea.Core;
using Particle;
using UnityEngine;

namespace Actor.Enemy
{
    public partial class Enemy
    {
        private class DeathState : ImtStateMachine<Enemy, EnemyState>.State
        {
            protected override async void Enter()
            {
                Destroy(Context.gameObject);

                var pos = Context.transform.position;
                await ParticleManager.Instance.PlayVfx(VfxEnum.Death, 1.5f, pos + new Vector3(0, 0.8f));
            }
        }
    }
}