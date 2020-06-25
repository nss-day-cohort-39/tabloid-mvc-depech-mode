using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
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
                                        c.Content,
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
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName"))
                        };
                        comments.Add(comment);
                    }

                    reader.Close();

                    return comments;
                }
            }

        }

        public void AddComment(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Comment (PostId, 
                                        UserProfileId, 
                                        [Subject], 
                                        Content,
                                        CreateDateTime)
                    OUTPUT INSERTED.ID
                    VALUES (@postId, @userProfileId, @subject, @content, @createDateTime);
                ";

                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@userProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@createDateTime", DateAndTime.Now);
                    cmd.Parameters.AddWithValue("@postId", comment.PostId);


                    int id = (int)cmd.ExecuteScalar();

                    comment.Id = id;
                }
            }
        }

        public void DeleteComment(int commentId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Comment
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", commentId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Comment GetCommentById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT c.Id, 
                               c.PostId, 
                               c.UserProfileId, 
                               c.[Subject], 
                               c.Content,
                               c.UserProfileId, 
                               up.DisplayName AS DisplayName
                        FROM Comment c
                        LEFT JOIN UserProfile up ON up.Id = c.UserProfileId
                        WHERE c.PostId = @PostId                            
                    ";

                    cmd.Parameters.AddWithValue("@PostId", id);
                    

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {

                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName"))
                        };

                        reader.Close();
                        return comment;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }


            }
        }
    }
}
