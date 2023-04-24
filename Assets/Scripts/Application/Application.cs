using Leopotam.EcsLite;
using SelStrom.Asteroids.Configs;
using UnityEngine;

namespace SelStrom.Asteroids
{
    public class Application
    {
        private GameObjectPool _gameObjectPool;
        private IApplicationComponent _appComponent;
        private Game _game;
        private Model _model;
        private GameScreen _gameScreen;
        private GameData _configs;
        private Transform _gameContainer;
        private PlayerInput _playerInput;
        private EntitiesCatalog _catalog;


        private EcsWorld _world;
        private IEcsSystems _systems;

        public void Connect(IApplicationComponent appComponent, GameData configs,
            Transform poolContainer, Transform gameContainer, GameScreen gameScreen)
        {
            _gameScreen = gameScreen;
            _configs = configs;
            _playerInput = new PlayerInput();
            _gameContainer = gameContainer;
            _appComponent = appComponent;

            _gameObjectPool = new GameObjectPool();
            _gameObjectPool.Connect(poolContainer);

            _catalog = new EntitiesCatalog();
            
            _world = new EcsWorld();
            _systems = new EcsSystems(_world, configs);
        }

        public void Start()
        {
            var mainCamera = Camera.main;
            var orthographicSize = mainCamera!.orthographicSize;
            var sceneWidth = mainCamera.aspect * orthographicSize * 2;
            var sceneHeight = orthographicSize * 2;
            Debug.Log("Scene size: " + sceneWidth + " x " + sceneHeight);

            var size = new Vector2(sceneWidth, sceneHeight);
            var timeService = new TimeService();

            _systems
                .Add(new TimeSystem(timeService))
                .Add(new RotateSystem())
                .Add(new ThrustSystem())
                .Add(new MoveSystem(new GameArea{Size = size}))
                .Add(new LifeTimeSystem())
                .Add(new GunSystem())
                .Add(new LaserSystem())
                .Add(new ShootToSystem())
                .Add(new MoveToSystem())
                .Init();
            
            _catalog.Connect(_configs, new ModelFactory(_model), new ViewFactory(_gameObjectPool, _gameContainer));

            _game = new Game(_catalog, _model, _configs, _playerInput, _gameScreen);
            _game.Start();

            _appComponent.OnUpdate += OnUpdate;
            _appComponent.OnPause += OnPause;
            _appComponent.OnResume += OnResume;
            _playerInput.OnBackAction += OnBack;
        }

        private void OnResume()
        {
            _appComponent.OnUpdate += OnUpdate;
        }

        private void OnPause()
        {
            _appComponent.OnUpdate -= OnUpdate;
        }

        private void OnUpdate()
        {
            _systems.Run();
        }

        private void OnBack()
        {
            UnityEngine.Application.Quit(0);
        }

        private void Dispose()
        {
            _catalog.Dispose();
            _gameObjectPool.Dispose();
            
            _systems?.Destroy();
            _world?.Destroy();

            _catalog = null;
            _gameObjectPool = null;
            _appComponent = null;
            _game = null;
            _model = null;
            _configs = null;
            _gameContainer = null;
            _playerInput = null;
            _gameScreen = null;

            _systems = null;
            _world = null;
        }

        public void Quit()
        {
            _appComponent.OnUpdate -= OnUpdate;
            _appComponent.OnPause -= OnPause;
            _appComponent.OnResume -= OnResume;
            _playerInput.OnBackAction -= OnBack;

            Dispose();
        }
    }
}