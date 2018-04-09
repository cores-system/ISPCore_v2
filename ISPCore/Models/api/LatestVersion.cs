namespace ISPCore.Models.api
{
    public class LatestVersion
    {
        public int Id { get; set; }
        public double Version { get; set; }
        public int Patch { get; set; }


        public override string ToString()
        {
            return string.Format("{0:N1}.{1}", Version, Patch).Replace(",", ".");
        }
    }
}
