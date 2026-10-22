namespace Game.Domain
{
    public struct CheckpointData
    {
        public float X;
        public float Y;
        public float Z;
        public BallWeight SavedWeight;

        public CheckpointData(float x, float y, float z, BallWeight weight)
        {
            X = x;
            Y = y;
            Z = z;
            SavedWeight = weight;
        }
    }
}