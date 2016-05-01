using MongoDB.Bson;

namespace Models
{
    public class MitgliedGruppe
    {
        public ObjectId _id { get; set; }
        public string MongoId { get; set; }
        public string MitgliedGruppeName { get; set; }
    }
}
