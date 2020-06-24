using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }
        public List<Comment> GetByPostId(int PostId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, 
                                        c.PostId, 
                                        c.UserProfileId, 
                                        c.[Subject], 
                                        c.UserProfileId, 
                                        up.DisplayName AS DisplayName
                                        FROM Comment c
                                        LEFT JOIN UserProfile up ON up.Id = c.UserProfileId
                                        WHERE c.PostId = @PostId
                                        ORDER BY c.CreateDateTime ASC";

                    cmd.Parameters.AddWithValue("@PostId", PostId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                    while (reader.Read())
                    {
                        Comment comment = new Comment()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName"))
                        };
                        comments.Add(comment);
                    }

                    reader.Close();

                    return comments;
                }
            }

        }
    }
}
