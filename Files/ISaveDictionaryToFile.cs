namespace Assets.Skrypty.Files
{
    public interface ISaveDictionaryToFile
    {
        string Path { get; set; }

        void DictionaryToFile();
    }
}