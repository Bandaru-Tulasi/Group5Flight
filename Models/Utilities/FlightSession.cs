using Group5Flight.Models.ExtensionMethods;

namespace Group5Flight.Models.Utilities
{
    public class FlightSession
    {
        private const string FromKey = "from";
        private const string ToKey = "to";
        private const string DateKey = "date";
        private const string CabinKey = "cabin";
        private const string AirlineIdKey = "airlineId";
        private const string SelectedFlightsKey = "selectedflights";

        private ISession session;

        public FlightSession(ISession session)
        {
            this.session = session;
        }

        public void SetFrom(string from)
        {
            session.SetString(FromKey, from ?? "");
        }

        public string GetFrom()
        {
            return session.GetString(FromKey) ?? "";
        }

        public void SetTo(string to)
        {
            session.SetString(ToKey, to ?? "");
        }

        public string GetTo()
        {
            return session.GetString(ToKey) ?? "";
        }

        public void SetDate(string date)
        {
            session.SetString(DateKey, date ?? "");
        }

        public string GetDate()
        {
            return session.GetString(DateKey) ?? "";
        }

        public void SetCabinType(string cabin)
        {
            session.SetString(CabinKey, cabin ?? "All");
        }

        public string GetCabinType()
        {
            return session.GetString(CabinKey) ?? "All";
        }

        public void SetAirlineId(int? airlineId)
        {
            session.SetString(AirlineIdKey, airlineId?.ToString() ?? "");
        }

        public int? GetAirlineId()
        {
            var value = session.GetString(AirlineIdKey);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return int.Parse(value);
        }

        public List<int> GetSelectedFlights()
        {
            return session.GetObject<List<int>>(SelectedFlightsKey) ?? new List<int>();
        }

        public void SetSelectedFlights(List<int> ids)
        {
            session.SetObject(SelectedFlightsKey, ids);
        }

        public int? GetSelectedFlightCount()
        {
            return GetSelectedFlights().Count;
        }

        public void AddSelectedFlight(int id)
        {
            var ids = GetSelectedFlights();
            if (!ids.Contains(id))
            {
                ids.Add(id);
                SetSelectedFlights(ids);
            }
        }

        public void RemoveSelectedFlight(int id)
        {
            var ids = GetSelectedFlights();
            ids.Remove(id);
            SetSelectedFlights(ids);
        }

        public void ClearSelectedFlights()
        {
            session.Remove(SelectedFlightsKey);
        }

        public void ClearAllFilters()
        {
            session.Remove(FromKey);
            session.Remove(ToKey);
            session.Remove(DateKey);
            session.Remove(CabinKey);
            session.Remove(AirlineIdKey);
        }
    }
}