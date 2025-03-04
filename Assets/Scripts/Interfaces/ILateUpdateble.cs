namespace Runner.Core
{
    public interface ILateUpdateble: IGameSystem
    {
        public void LateLocalUpdate(float deltaTime);
    }
}