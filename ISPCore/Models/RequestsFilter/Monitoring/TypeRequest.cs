namespace ISPCore.Models.RequestsFilter.Monitoring
{
    public enum TypeRequest
    {
        unknown = 0,
        _200 = 1,
        _303 = 2,
        _403 = 3,
        _401 = 4,
        _500 = 5,
        _2fa = 6,
        All = 7,
        IPtables = 8
    }
}
