namespace Runner.Core
{
    public interface IGameState
    {
        public bool IsStart { get; }
        public bool IsGame { get; }
        public bool IsFinish { get; }
        public bool IsKick { get; }
    }
}