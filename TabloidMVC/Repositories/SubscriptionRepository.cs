using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class SubscriptionRepository : BaseRepository
    {
        public SubscriptionRepository(IConfiguration config) : base(config) { }
        public List<Subscription> GetAll(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM Subscription
                                        WHERE SubscriberUserProfileId = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    var subscriptions = new List<Subscription>();

                    while (reader.Read())
                    {
                        subscriptions.Add(new Subscription()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            SubscriberUserProfileId = reader.GetInt32(reader.GetOrdinal("SubscriberUserProfileId")),
                            ProviderUserProfileId = reader.GetInt32(reader.GetOrdinal("ProviderUserProfileId")),
                            BeginDateTime = reader.GetDateTime(reader.GetOrdinal("BeginDateTime")),
                            EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime"))
                        });
                    }

                    reader.Close();

                    return subscriptions;
                }
            }
        }

        public Subscription GetSubscription(SubscribeViewModel vm)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT TOP 1 * FROM Subscription 
                                        WHERE SubscriberUserProfileId = @subscriberId AND ProviderUserProfileId = @providerId";
                    cmd.Parameters.AddWithValue("@subscriberId", vm.SubscriberUserId);
                    cmd.Parameters.AddWithValue("@providerId", vm.ProviderUserId);
                    var reader = cmd.ExecuteReader();

                    Subscription subscription = new Subscription();

                    if (reader.Read())
                    {
                        subscription.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        subscription.SubscriberUserProfileId = reader.GetInt32(reader.GetOrdinal("SubscriberUserProfileId"));
                        subscription.ProviderUserProfileId = reader.GetInt32(reader.GetOrdinal("ProviderUserProfileId"));
                        subscription.BeginDateTime = reader.GetDateTime(reader.GetOrdinal("BeginDateTime"));
                        subscription.EndDateTime = DbUtils.GetNullableDateTime(reader, "EndDateTime");
                    }

                    reader.Close();

                    return subscription;
                }
            }
        }

        public bool IsSubscribed(SubscribeViewModel vm)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT COUNT(Id) AS SubNum FROM Subscription 
                                        WHERE SubscriberUserProfileId = @subscriberId AND ProviderUserProfileId = @providerId";
                    cmd.Parameters.AddWithValue("@subscriberId", vm.SubscriberUserId);
                    cmd.Parameters.AddWithValue("@providerId", vm.ProviderUserId);
                    var reader = cmd.ExecuteReader();

                    bool isSubscribed = false;

                    if (reader.Read())
                    {
                        int SubNum = reader.GetInt32(reader.GetOrdinal("SubNum"));

                        
                        if (SubNum > 0)
                        {
                            isSubscribed = true;
                        }
                        else
                        {
                            isSubscribed = false;
                        }
                        
                    }

                    reader.Close();

                    return isSubscribed;
                }
            }
        }

        public void Add(Subscription subscription)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Subscription (SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime)
                        OUTPUT INSERTED.ID
                        VALUES (@subscriberUserProfileId, @providerUserProfileId, @beginDateTime)";
                    cmd.Parameters.AddWithValue("@subscriberUserProfileId", subscription.SubscriberUserProfileId);
                    cmd.Parameters.AddWithValue("@providerUserProfileId", subscription.ProviderUserProfileId);
                    cmd.Parameters.AddWithValue("@beginDateTime", DateTime.Now);

                    subscription.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Delete(Subscription sub)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Subscription WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", sub.Id);

                    var reader = cmd.ExecuteReader();
                }
            }
        }
    }
}
