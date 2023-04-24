using System;

namespace SelStrom.Asteroids
{
    public interface IApplicationComponent
    {
        public event Action OnUpdate;
        public event Action OnPause;
        public event Action OnResume;
    }
}