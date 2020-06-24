using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;

namespace TabloidMVC.Repositories
{
    public class PostTagRepository : BaseRepository
    {
        public PostTagRepository(IConfiguration config) : base(config) { }

        public List<Tag> GetPostTags(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT PostTag.id, PostTag.PostId, PostTag.TagId, Tag.Name 
                                        FROM PostTag
                                        JOIN Tag on Tag.Id = PostTag.TagId
                                        WHERE PostTag.id = 1";
                    var reader = cmd.ExecuteReader();

                    var tags = new List<Tag>();

                    while (reader.Read())
                    {
                        tags.Add(new Tag()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("TagId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        });
                    }

                    reader.Close();

                    return tags;
                }
            }
        }

        public void UpdateTags(PostTagViewModel vm)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    //delete all old tags from the db
                    cmd.CommandText = "DELETE FROM PostTag WHERE PostId = @id";
                    cmd.Parameters.AddWithValue("@id", vm.Post.Id);
                    var reader = cmd.ExecuteReader();

                    reader.Close();

                }

                if (vm.TagString != "")
                {
                    //split the string of tags into a list
                    List<string> textTags = vm.TagString.Split(new char[] { ',' }).ToList();

                    //match the tag names with the corresponding tag
                    IEnumerable<Tag> tagList = textTags.Select(tagtext => vm.TagList.First(tag => tag.Name == tagtext));

                    foreach (Tag tag in tagList)
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "INSERT INTO PostTag (PostId, TagId) VALUES (@id, @tagId)";
                            cmd.Parameters.AddWithValue("@id", vm.Post.Id);
                            cmd.Parameters.AddWithValue("@tagId", tag.Id);
                            var reader = cmd.ExecuteReader();

                            reader.Close();
                        }

                    }
                }
                

            }

        }
        }

    }

