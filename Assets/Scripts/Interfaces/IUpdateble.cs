using System;

namespace Runner.Core
{
    public interface IUpdateble : IGameSystem
    {
        public void LocalUpdate(float deltaTime);
    }
}