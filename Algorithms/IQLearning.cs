namespace Assets.Skrypty
{
    public interface IQLearning
    {
        int GetRandomAction(State actualState);
        void LearningAlgorithm();
    }
}