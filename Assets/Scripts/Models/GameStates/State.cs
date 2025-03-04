namespace Runner.Core
{
    public class State : IGameState
    {
        private bool _isStart;
        private bool _isPlay;
        private bool _isFinish;
        private bool _isKick;

        
        public bool IsStart => _isStart;
        public bool IsGame => _isPlay;
        public bool IsFinish => _isFinish;
        public bool IsKick => _isKick;

        protected State (bool isStart, bool isPlay, bool isFinish, bool isKick)
        {
            _isStart = isStart;
            _isPlay = isPlay;
            _isFinish = isFinish;
            _isKick = isKick;
        }
    }
}