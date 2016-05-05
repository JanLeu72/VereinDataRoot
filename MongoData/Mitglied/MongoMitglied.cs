using System;
using VereinDataRoot;

namespace MongoData
{
    using System.Collections.Generic;
    using System.Linq;
    using LinqKit;
    using Models;
    using MongoDB.Driver;

    public static class MongoMitglied
    {
        private static IMongoClient _client;
        private static IMongoDatabase _database;

        public static bool DelMitgliedById(MitgliedModel model, string mandantDb)
        {
            try
            {
                _client = new MongoClient();
                _database = _client.GetDatabase(mandantDb);

                var filter = Builders<MitgliedModel>.Filter.Eq(s => s._id, model._id);
                var collection = _database.GetCollection<MitgliedModel>("Mitglieder");
                collection.DeleteManyAsync(filter);

                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class MongoMitglied DelMitglied: " + ex);
                return false;
            }
        }

        public static bool SetMitglied(MitgliedModel model, string mandantDb)
        {
            try
            {
                _client = new MongoClient();
                _database = _client.GetDatabase(mandantDb);

                var collection = _database.GetCollection<MitgliedModel>("Mitglieder");
                collection.InsertOneAsync(model);

                return true;

            }
            catch (Exception ex)
            {
                Log.Net.Error("class MongoMitglied SetMitglied: " + ex);
                return false;
            }
        }

        public static bool UpdMitglied(MitgliedModel model, string mandantDb)
        {
            try
            {
                _client = new MongoClient();
                _database = _client.GetDatabase(mandantDb);

                var filter = Builders<MitgliedModel>.Filter.Eq(s => s._id, model._id);
                var collection = _database.GetCollection<MitgliedModel>("Mitglieder");
                collection.ReplaceOneAsync(filter, model);

                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class MongoMitglied UpdMitglied: " + ex);
                return false;
            }
        }

        public static bool ImportMitglied(MitgliedModel model, string mandantDb, out MitgliedModel mitglied, out string message)
        {
            try
            {
                _client = new MongoClient();
                _database = _client.GetDatabase(mandantDb);

                var collection = _database.GetCollection<MitgliedModel>("Mitglieder");
                collection.InsertOneAsync(model);


                message = string.Empty;
                mitglied = new MitgliedModel();

                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class MongoMitglied ImportMitglied: " + ex);
                message = ex.Message;
                mitglied = model;
                return false;
            }
        }

        public static int GetMaxMitglieder(string mandantDb)
        {
            try
            {
                _client = new MongoClient();
                _database = _client.GetDatabase(mandantDb);

                var collection = _database.GetCollection<MitgliedModel>("Mitglieder");

                return (from d in collection.AsQueryable() select d).Count();
            }
            catch (Exception ex)
            {
                Log.Net.Error("GetMaxMitglieder: " + ex);
                return 0;
            }
        }

        public static List<MitgliedModel> GetMitglieder(int skip, int take, string mandantDb, List<TableFilter> filter, List<TableSort> sorting, out long count)
        {
            _client = new MongoClient();
            _database = _client.GetDatabase(mandantDb);

            var collection = _database.GetCollection<MitgliedModel>("Mitglieder");

            var predicate = PredicateBuilder.True<MitgliedModel>();

            if (filter.Count > 0)
            {
                if (filter[0].Field == "Anrede")
                {
                    if (filter[0].Operator == "eq")
                    {
                        string s =  filter[0].Value;
                        predicate = predicate.And(p => p.Anrede.Equals(s));
                    }
                }

                if (filter[0].Field == "Vorname")
                {
                    var s = filter[0].Value;
                    if (filter[0].Operator == "eq")
                    {
                        predicate = predicate.And(p => p.Vorname.Equals(s));
                    }

                    if (filter[0].Operator == "startswith")
                    {
                        predicate = predicate.And(p => p.Vorname.StartsWith(s));
                    }
                }

                if (filter[0].Field == "Name")
                {
                    var s = filter[0].Value;
                    if (filter[0].Operator == "eq")
                    {
                        predicate = predicate.And(p => p.Name.Equals(s));
                    }

                    if (filter[0].Operator == "startswith")
                    {
                        predicate = predicate.And(p => p.Name.StartsWith(s));
                    }
                }

                if (filter[0].Field == "Strasse")
                {
                    var s = filter[0].Value;
                    if (filter[0].Operator == "eq")
                    {
                        predicate = predicate.And(p => p.Strasse.Equals(s));
                    }

                    if (filter[0].Operator == "startswith")
                    {
                        predicate = predicate.And(p => p.Strasse.StartsWith(s));
                    }
                }

                if (filter[0].Field == "Plz")
                {
                    var s = filter[0].Value;
                    if (filter[0].Operator == "eq")
                    {
                        predicate = predicate.And(p => p.Plz.Equals(s));
                    }

                    if (filter[0].Operator == "startswith")
                    {
                        predicate = predicate.And(p => p.Plz.StartsWith(s));
                    }
                }

                if (filter[0].Field == "Ort")
                {
                    var s = filter[0].Value;
                    if (filter[0].Operator == "eq")
                    {
                        predicate = predicate.And(p => p.Ort.Equals(s));
                    }

                    if (filter[0].Operator == "startswith")
                    {
                        predicate = predicate.And(p => p.Ort.StartsWith(s));
                    }
                }

                if (filter[0].Field == "MandantBenutzerGruppeName")
                {
                    int id = 0;
                    int.TryParse(filter[0].Value, out id);

                    if (filter[0].Operator == "eq")
                    {
                        predicate = predicate.And(p => p.MandantBenutzerGruppeId.Equals(id));
                    }
                }
            }
            List<MitgliedModel> list = new List<MitgliedModel>();
            count = (from d in collection.AsQueryable().AsExpandable().Where(predicate) select d).Count();

            if (sorting.Count > 0)
            {
                switch (sorting[0].Field)
                {
                    case "Anrede":
                        if (sorting[0].Dir == "asc")
                        {
                            list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                                    orderby d.Anrede ascending
                                    select d).Skip(skip).Take(take).ToList();
                        }
                        else
                        {
                            list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                                    orderby d.Anrede descending
                                    select d).Skip(skip).Take(take).ToList();
                        }
                        break;
                    case "Vorname":
                        if (sorting[0].Dir == "asc")
                        {
                            list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                                    orderby d.Vorname ascending
                                    select d).Skip(skip).Take(take).ToList();
                        }
                        else
                        {
                            list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                                    orderby d.Vorname descending
                                    select d).Skip(skip).Take(take).ToList();
                        }
                        break;
                    case "Name":
                        if (sorting[0].Dir == "asc")
                        {
                            list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                                    orderby d.Name ascending
                                    select d).Skip(skip).Take(take).ToList();
                        }
                        else
                        {
                            list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                                    orderby d.Name descending
                                    select d).Skip(skip).Take(take).ToList();
                        }
                        break;
                    case "Strasse":
                        if (sorting[0].Dir == "asc")
                        {
                            list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                                    orderby d.Strasse ascending
                                    select d).Skip(skip).Take(take).ToList();
                        }
                        else
                        {
                            list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                                    orderby d.Strasse descending
                                    select d).Skip(skip).Take(take).ToList();
                        }
                        break;
                    case "Plz":
                        if (sorting[0].Dir == "asc")
                        {
                            list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                                    orderby d.Plz ascending
                                    select d).Skip(skip).Take(take).ToList();
                        }
                        else
                        {
                            list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                                    orderby d.Plz descending
                                    select d).Skip(skip).Take(take).ToList();
                        }
                        break;
                    case "Ort":
                        if (sorting[0].Dir == "asc")
                        {
                            list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                                    orderby d.Ort ascending
                                    select d).Skip(skip).Take(take).ToList();
                        }
                        else
                        {
                            list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                                    orderby d.Ort descending
                                    select d).Skip(skip).Take(take).ToList();
                        }
                        break;
                }
            }
            else
            {
                list = (from d in collection.AsQueryable().AsExpandable().Where(predicate)
                        orderby d.Name ascending
                        select d).Skip(skip).Take(take).ToList();
            }

            return list;
        }
    }
}
