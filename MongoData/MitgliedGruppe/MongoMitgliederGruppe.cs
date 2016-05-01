using LinqKit;

namespace MongoData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Models;
    using MongoDB.Driver;
    using VereinDataRoot;

    public static class MongoMitgliederGruppe
    {
        private static IMongoClient _client;
        private static IMongoDatabase _database;

        public static List<MitgliedGruppe> GetMitgliederGruppen(string mandantDb)
        {
            _client = new MongoClient();
            _database = _client.GetDatabase(mandantDb);

            var collection = _database.GetCollection<MitgliedGruppe>("MitgliederGruppen");

            return (from d in collection.AsQueryable()
                    orderby d.MitgliedGruppeName 
                    select d).ToList();
        }

        public static bool SetMitgliedGruppe(MitgliedGruppe model, string mandantDb)
        {
            try
            {
                _client = new MongoClient();
                _database = _client.GetDatabase(mandantDb);

                var collection = _database.GetCollection<MitgliedGruppe>("MitgliederGruppen");
                collection.InsertOneAsync(model);

                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class MongoMitgliederGruppe SetMitgliedGruppe: " + ex);
                return false;
            }
        }

        public static bool UpdMitgliedGruppe(MitgliedGruppe model, string mandantDb)
        {
            try
            {
                _client = new MongoClient();
                _database = _client.GetDatabase(mandantDb);

                var filter = Builders<MitgliedGruppe>.Filter.Eq(s => s._id, model._id);
                var collection = _database.GetCollection<MitgliedGruppe>("MitgliederGruppen");
                collection.ReplaceOneAsync(filter, model);

                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class MongoMitgliederGruppe UpdMitgliedGruppe: " + ex);
                return false;
            }
        }

        public static bool DelMitgliedGruppe(MitgliedGruppe model, string mandantDb)
        {
            try
            {
                _client = new MongoClient();
                _database = _client.GetDatabase(mandantDb);

                var filter = Builders<MitgliedModel>.Filter.Eq(s => s._id, model._id);
                var collection = _database.GetCollection<MitgliedModel>("MitgliederGruppen");
                collection.DeleteManyAsync(filter);

                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class MongoMitgliederGruppe DelMitgliedGruppe: " + ex);
                return false;
            }
        }
    }
}
