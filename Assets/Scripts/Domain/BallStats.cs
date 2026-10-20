namespace Game.Domain
{
    public struct BallStats
    {
        public float Mass;
        public float Force;
        public float Drag;

        public static BallStats Get(BallWeight weight)
        {
            return weight switch
            {
                BallWeight.Stone => new BallStats { Mass = 3f, Force = 25f, Drag = 0.5f },
                BallWeight.Paper => new BallStats { Mass = 0.3f, Force = 10f, Drag = 2.0f },
                BallWeight.Wood => new BallStats { Mass = 1f, Force = 15f, Drag = 0.5f }
            };
        }
    }
}