using System.Collections.Generic;

namespace ISPCore.Models.Triggers.Blueprint
{
    public class BlueprintModel
    {
        public Position position { get; set; } = new Position();
        public List<Parent> parents { get; set; } = new List<Parent>();
        public UserData userData { get; set; } = new UserData();
        public VarsData varsData { get; set; } = new VarsData();
        public string worker { get; set; }
        public string uid { get; set; }
    }
}
