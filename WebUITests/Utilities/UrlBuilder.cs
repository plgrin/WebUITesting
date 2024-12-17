namespace WebUITests.Utilities
{
    public class UrlBuilder
    {
        private string _baseUrl;
        private Dictionary<string, string> _queryParams = new Dictionary<string, string>();

        public UrlBuilder SetBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
            return this;
        }

        public UrlBuilder AddQueryParam(string key, string value)
        {
            _queryParams[key] = value;
            return this;
        }

        public string Build()
        {
            if (string.IsNullOrEmpty(_baseUrl))
                throw new InvalidOperationException("Base URL must be set.");

            var uriBuilder = new UriBuilder(_baseUrl);
            if (_queryParams.Any())
            {
                uriBuilder.Query = string.Join("&", _queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            }

            return uriBuilder.ToString();
        }
    }
}
