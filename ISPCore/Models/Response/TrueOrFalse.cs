namespace ISPCore.Models.Response
{
    public class TrueOrFalse
    {
        public TrueOrFalse(bool res)
        {
            result = res;
        }

        public bool result { get; private set; }
    }
}
