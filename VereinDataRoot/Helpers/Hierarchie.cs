namespace VereinDataRoot.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public static class Hierarchie
    {
        public static List<TreeNavigationModel> FillRecursive(List<TreeNavigationModel> list, int parentId)
        {
            List<TreeNavigationModel> newList = new List<TreeNavigationModel>();

            foreach (TreeNavigationModel item in list.Where(x => x.TreeNavigationId.Equals(parentId)))
            {
                newList.Add(new TreeNavigationModel
                {
                    DisplayName = item.DisplayName,
                    TreeId = item.TreeId,
                    ActionMvc = item.ActionMvc,
                    ControllerMvc = item.ControllerMvc,
                    SubNavigations = FillRecursive(list, item.TreeId)
                });
            }

            return newList;
        }
    }
}