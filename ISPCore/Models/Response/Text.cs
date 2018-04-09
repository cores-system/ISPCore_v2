namespace ISPCore.Models.Response
{
    public class Text
    {
        public Text(string _msg)
        {
            msg = _msg;
        }

        public string msg { get; private set; }
    }
}
