using Leopotam.EcsLite;
using Model.Components;
using UnityEngine;

namespace SelStrom.Asteroids
{
    public sealed class TimeSystem : IEcsRunSystem
    {
        private readonly TimeService _timeService = default;

        public TimeSystem(TimeService timeService)
        {
            _timeService = timeService;
        }

        public void Run(IEcsSystems systems)
        {
            _timeService.Time = Time.time;
            _timeService.UnscaledTime = Time.unscaledTime;
            _timeService.DeltaTime = Time.deltaTime;
            _timeService.UnscaledDeltaTime = Time.unscaledDeltaTime;
        }
    }

    public class GunSystem : BaseModelSystem<GunComponent>
    {
        protected override void UpdateNode(GunComponent node, float deltaTime)
        {
            if (node.CurrentShoots < node.MaxShoots)
            {
                node.ReloadRemaining -= deltaTime;
                if (node.ReloadRemaining <= 0)
                {
                    node.ReloadRemaining = node.ReloadDurationSec;
                    node.CurrentShoots = node.MaxShoots;
                }
            }

            if (node.Shooting && node.CurrentShoots > 0)
            {
                node.CurrentShoots--;
                node.OnShooting?.Invoke(node);
            }

            node.Shooting = false;
        }
    }
}