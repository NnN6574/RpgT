namespace Mistave.Client.Data
{
    public interface IDataContainer
    {
        public void Save();
        public string ToJson();
        public void FromJson(string json);
    }
}