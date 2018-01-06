namespace Assets.Skrypty
{
    public interface IState
    {
        int State1 { get; set; }
        int State2 { get; set; }
        int State3 { get; set; }
        int State4 { get; set; }

        int[] GetState();
    }
}