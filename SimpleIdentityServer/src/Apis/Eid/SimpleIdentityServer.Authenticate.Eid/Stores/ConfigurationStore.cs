namespace SimpleIdentityServer.Authenticate.Eid.Stores
{
    public class ConfigurationStore
    {
        private static ConfigurationStore _instance;
        private string _eidUrl = null;

        private ConfigurationStore()
        {
            
        }

        public static ConfigurationStore Instance()
        {
            if (_instance == null)
            {
                _instance = new ConfigurationStore();
            }

            return _instance;
        }

        public void SetEidUrl(string eidUrl)
        {
            _eidUrl = eidUrl;
        }

        public string EidUrl
        {
            get
            {
                return _eidUrl;
            }
        }
    }
}
