namespace BBQN.AutoEnrolled.API.Integrations
{
    public interface IIntegration
    {
        public Task<Stream> PutData(string url, Dictionary<string, string> headerParameters, object obj);
        public Task<Stream> PostData(string url, Dictionary<string, string> headerParameters,object obj);
        public Task<Stream> GetData(string url, Dictionary<string, string> headerParameters, bool isSBCall=true);
        public Task<Stream> DeleteData(string url, Dictionary<string, string> headerParameters);
    }
}
