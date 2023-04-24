using UnityEngine;

namespace SelStrom.Asteroids
{
    public sealed class TimeService
    {
        public float Time;
        public float DeltaTime;
        public float UnscaledDeltaTime;
        public float UnscaledTime;
    }
    
    public sealed class GameArea
    {
        public Vector2 Size;
    }
}