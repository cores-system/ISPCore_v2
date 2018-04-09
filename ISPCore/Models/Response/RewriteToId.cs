namespace ISPCore.Models.Response
{
    public class RewriteToId
    {
        public RewriteToId(int _id)
        {
            Id = _id;
        }

        public int Id { get; private set; }
    }
}
