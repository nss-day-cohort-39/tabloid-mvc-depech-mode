using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;

namespace TabloidMVC.Repositories
{
    public class ReactionRepository : BaseRepository
    {
        public ReactionRepository(IConfiguration config) : base(config) { }
        public List<Reaction> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, name, ImageLocation FROM Reaction";
                    var reader = cmd.ExecuteReader();

                    var reactions = new List<Reaction>();

                    while (reader.Read())
                    {
                        reactions.Add(new Reaction()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("name"))
                        });
                    }

                    reader.Close();

                    return reactions;
                }
            }
        }
        public List<Reaction> GetReactionsByPostId(int postId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT r.id AS reationId, r.name AS reactionName 
                                        FROM Reaction r
                    `                   JOIN PostReaction pr ON pr.ReactionId = r.Id
                                        JOIN Post p ON p.id = pr.PostId
                                        WHERE p.Id = @postId";
                    cmd.Parameters.AddWithValue("@postId", postId);
                    var reader = cmd.ExecuteReader();

                    var reactions = new List<Reaction>();

                    while (reader.Read())
                    {
                        reactions.Add(new Reaction()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("name"))
                        });
                    }

                    reader.Close();

                    return reactions;
                }
            }
        }
        public void Add(Reaction reaction)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Reaction (Name),
                        OUTPUT INSERTED.ID
                        VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", reaction.Name);

                    reaction.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Delete(Reaction reaction)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Reaction
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", reaction.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Reaction GetReactionById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT Id, [Name] 
                         FROM Reaction
                         WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    Reaction category = null;

                    if (reader.Read())
                    {
                        category = new Reaction();
                        category.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        category.Name = reader.GetString(reader.GetOrdinal("Name"));
                    }

                    reader.Close();

                    return category;
                }
            }
        }

        public void Update(Reaction reaction)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Reaction
                            SET [Name] = @name 
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", reaction.Name);
                    cmd.Parameters.AddWithValue("@id", reaction.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
