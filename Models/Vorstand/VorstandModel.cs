namespace Models
{
    public class VorstandModel
    {
        public int VorstandId { get; set; }
        public int MitgliedId { get; set; }
        public int RessortId { get; set; }
        public string RessortName { get; set; }
        public string MitgliedAnrede { get; set; }
        public string MitgliedName { get; set; }
        public string MitgliedVorname { get; set; }
        public string MitgliedStrasse { get; set; }
        public string MitgliedPlz { get; set; }
        public string MitgliedOrt { get; set; }

        public string VorstandName { get; set; }
    }
}
