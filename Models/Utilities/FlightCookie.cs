namespace Group5Flight.Models.Utilities
{
    public class FlightCookie
    {
        private const string SelectedFlightsKey = "selectedflights";
        private const string Delimiter = "-";
        private const int DaysToKeep = 14;

        private IRequestCookieCollection? requestCookies;
        private IResponseCookies? responseCookies;

        public FlightCookie(IRequestCookieCollection requestCookies)
        {
            this.requestCookies = requestCookies;
        }

        public FlightCookie(IResponseCookies responseCookies)
        {
            this.responseCookies = responseCookies;
        }

        public List<int> GetSelectedFlightIds()
        {
            string cookie = requestCookies?[SelectedFlightsKey] ?? string.Empty;
            if (string.IsNullOrEmpty(cookie))
            {
                return new List<int>();
            }
            return cookie.Split(Delimiter).Select(int.Parse).ToList();
        }

        public void SetSelectedFlightIds(IEnumerable<int> ids)
        {
            string value = string.Join(Delimiter, ids);
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(DaysToKeep),
                HttpOnly = true,
                IsEssential = true
            };
            responseCookies?.Delete(SelectedFlightsKey);
            responseCookies?.Append(SelectedFlightsKey, value, options);
        }

        public void RemoveSelectedFlightIds()
        {
            responseCookies?.Delete(SelectedFlightsKey);
        }
    }
}