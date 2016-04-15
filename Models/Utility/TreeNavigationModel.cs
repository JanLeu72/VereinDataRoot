namespace Models
{
    using System.Collections.Generic;

    public class TreeNavigationModel
    {
        public int TreeId { get; set; }
        public int TreeNavigationId { get; set; }
        public string DisplayName { get; set; }
        public string ActionMvc { get; set; }
        public string ControllerMvc { get; set; }
        public List<TreeNavigationModel> SubNavigations { get; set; }
        public int Sort { get; set; }
    }
}
