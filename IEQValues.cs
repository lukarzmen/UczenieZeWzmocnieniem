namespace Assets.Skrypty
{
    interface IEQValues
    {
        float GetEValue(int action);
        float GetQValue(int action);
        void increaseEValue(int action);
        void SetEQValues(int action, float e, float q);
        void SetEValue(int action, float e);
        void SetQValue(int action, float q);
    }
}