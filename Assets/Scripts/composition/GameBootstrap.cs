using UnityEngine;
using Game.Application;
using Game.Domain;
using Game.Presentation;
using Game.Infrastructure;

namespace Game.Composition
{
    public class GameBootstrap : MonoBehaviour
    {
        [Header("Views & Objects")]
        public BallView PlayerBall;
        public Transformer[] Transformers;
        public FallZone FallZone;

        public BallService BallService { get; private set; }
        public CheckpointService CheckpointService { get; private set; }
        public GameSessionService GameSession { get; private set; }

        void Start()
        {
            BallService = new BallService();
            CheckpointService = new CheckpointService();
            GameSession = new GameSessionService();

            if (PlayerBall != null)
            {
                PlayerBall.Init(BallService);
                
                var startData = new CheckpointData(
                    PlayerBall.transform.position.x,
                    PlayerBall.transform.position.y,
                    PlayerBall.transform.position.z,
                    BallService.CurrentWeight
                );
                CheckpointService.InitializeStart(startData);

                if (FallZone != null)
                {
                    FallZone.Init(CheckpointService, BallService, PlayerBall.GetComponent<Rigidbody>());
                }
            }

            if (Transformers != null)
            {
                foreach (var transformer in Transformers)
                {
                    if (transformer != null) 
                    {
                        transformer.Init(BallService);
                    }
                }
            }

            GameSession.StartSession();
        }

        void Update()
        {
            GameSession.Tick(Time.deltaTime);
        }
    }
}