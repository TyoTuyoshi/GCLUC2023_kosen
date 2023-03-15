using System;
using Actor.Bullet;
using AutoGenerate;
using Event;
using IceMilkTea.Core;
using Particle;
using UniRx;
using UnityEngine;

namespace Actor.Player
{
    public partial class Player
    {
        private static readonly int AnimIdGunAttackTrigger = Animator.StringToHash("GunAttackTrigger");

        [SerializeField] [Header("GunAttack")] [Space]
        private GameObject gunWeapon;

        [SerializeField] private float shotPower = 2f;
        [SerializeField] private BulletBase bulletPrefab;
        [SerializeField] private Transform gunShotOrigin;

        private class GunAttackState : ImtStateMachine<Player, PlayerState>.State
        {
            private IDisposable _disposable;

            protected override void Enter()
            {
                // TODO: 銃の登場演出
                Context.gunWeapon.SetActive(true);

                Context._animator.SetTrigger(AnimIdGunAttackTrigger);

                _disposable = Context.OnAnimEvent
                    .Where(e => e == "OnShot")
                    .Subscribe(OnShot);
            }

            private async void OnShot(string _)
            {
                var rot = Context.transform.rotation.eulerAngles;
                var pos = Context.gunShotOrigin.position;

                EventPublisher.Instance.PublishEvent(new ShotEvent
                {
                    Dir = new Vector3(rot.y == 0 ? -1 : 1, 0) * Context.shotPower,
                    Pos = pos,
                    Prefab = Context.bulletPrefab,
                    Source = Context
                });
                await ParticleManager.Instance.PlayVfx(VfxEnum.MuzzleFlash, 1, Context.gunShotOrigin.position);
            }

            protected override void Update()
            {
                var animator = Context._animator;
                if (animator.IsInTransition(0) || !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;

                StateMachine.SendEvent(PlayerState.Idle);
            }

            protected override void Exit()
            {
                // TODO: 銃の登場演出
                Context.gunWeapon.SetActive(false);

                Context._animator.ResetTrigger(AnimIdGunAttackTrigger);
                _disposable.Dispose();
                _disposable = null;
            }
        }
    }
}