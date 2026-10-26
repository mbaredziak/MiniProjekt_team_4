using System;
using Game.Domain;

namespace Game.Application
{
    public class CheckpointService
    {
        public CheckpointData CurrentCheckpoint { get; private set; }
        public event Action<CheckpointData> OnCheckpointSaved;

        public void InitializeStart(CheckpointData startData)
        {
            CurrentCheckpoint = startData;
        }

        public void SaveCheckpoint(CheckpointData data)
        {
            CurrentCheckpoint = data;
            OnCheckpointSaved?.Invoke(data);
        }
    }
}