using System.Collections.Generic;
using Leopotam.EcsLite;

namespace SelStrom.Asteroids
{
    public interface IModelSystem
    {
        public void Update(float deltaTime);
        public void Remove(IGameEntityModel model);
        public void CleanUp();
    }

    public abstract class BaseModelSystem<TNode> : IModelSystem, IEcsRunSystem
    {
        private readonly Dictionary<IGameEntityModel, TNode> _entityToNode = new();

        public void Add(IGameEntityModel model, TNode node)
        {
            _entityToNode.Add(model, node);
        }

        public void Remove(IGameEntityModel model)
        {
            _entityToNode.Remove(model);
        }

        public void Update(float deltaTime)
        {
            foreach (var node in _entityToNode.Values)
            {
                UpdateNode(node, deltaTime);
            }
        }

        public void CleanUp()
        {
            _entityToNode.Clear();
        }

        protected abstract void UpdateNode(TNode node, float deltaTime);
        
        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld ();
            var filter = world.Filter<Weapon> ().End ();
        
            // Фильтр хранит только сущности, сами даные лежат в пуле компонентов "Weapon".
            // Пул так же может быть закеширован где-то.
            var weapons = world.GetPool<Weapon>();
        
            foreach (int entity in filter) {
                ref Weapon weapon = ref weapons.Get (entity);
                weapon.Ammo = System.Math.Max (0, weapon.Ammo - 1);
            }
        }
    }
}