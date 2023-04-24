using Model.Components;

namespace SelStrom.Asteroids
{
    public class MoveSystem : BaseModelSystem<MoveComponent>
    {
        private readonly GameArea _gameArea;

        public MoveSystem(GameArea gameArea)
        {
            _gameArea = gameArea;
        }

        protected override void UpdateNode(MoveComponent node, float deltaTime)
        {
            var oldPosition = node.Position.Value;
            var position = oldPosition + node.Direction * (node.Speed.Value * deltaTime);
            Model.PlaceWithinGameArea(ref position.x, _gameArea.Size.x);
            Model.PlaceWithinGameArea(ref position.y, _gameArea.Size.y);
            node.Position.Value = position;
        }
    }
}