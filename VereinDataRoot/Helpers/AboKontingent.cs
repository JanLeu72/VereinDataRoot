namespace VereinDataRoot.Helpers
{
    using System.Collections.Generic;
    using Models;

    public static class AboKontingent
    {
        public static List<KeyValueModel> Get()
        {
            List<KeyValueModel> l = new List<KeyValueModel>();
            l.Add(new KeyValueModel
            {
                Id = "",
                Value = ""
            });

            return l;
        }
    }
}