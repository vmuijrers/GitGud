namespace ExampleProject
{
    [System.Flags]
    public enum UpdateFlags
    {
        Manual = 0,
        Pause = 1 << 0,
        Update = 1 << 1,
        Menu = 1 << 2
    }

}
