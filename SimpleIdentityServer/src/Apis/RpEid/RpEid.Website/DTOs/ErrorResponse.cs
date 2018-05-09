using Newtonsoft.Json.Linq;

namespace RpEid.Website.DTOs
{
    internal sealed class ErrorResponse
    {
        private string _error;
        private string _errorDescription;

        public ErrorResponse(string error, string errorDescription)
        {
            _error = error;
            _errorDescription = errorDescription;
        }

        public JObject GetJson()
        {
            var result = new JObject();
            result.Add(Constants.ErrorResponseNames.Code, _error);
            result.Add(Constants.ErrorResponseNames.Message, _errorDescription);
            return result;
        }
    }
}
