namespace ExampleProject
{
    public interface IUpdate
    {
        UpdateFlags Flags { get; }
        void OnUpdate();
    }

}
