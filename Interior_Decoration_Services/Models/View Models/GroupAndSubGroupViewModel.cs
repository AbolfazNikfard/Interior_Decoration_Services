using System.Text.RegularExpressions;

namespace Interior_Decoration_Services.Models
{
    public class GroupAndSubGroupViewModel
    {
        public GroupAndSubGroupViewModel()
        {
            subGroops = new List<SubGroup>();
        }
        public int groupId { get; set; }
        public string groupName { get; set; }
        public List<SubGroup> subGroops { get; set; }
    }
}
