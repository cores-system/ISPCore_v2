namespace ISPCore.Models.Response
{
    public class Modal
    {
        public Modal(string _msg, bool res = false)
        {
            msg = _msg;
            result = res;
        }

        public bool result { get; private set; }
        public string msg { get; private set; }
    }
}
