using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId, u.Active,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                         ORDER BY DisplayName";

                    List<UserProfile> users = new List<UserProfile>();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        UserProfile user = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            Active = reader.GetBoolean(reader.GetOrdinal("Active")),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                        users.Add(user);
                    }

                    reader.Close();

                    return users;
                }
            }
        }
        public List<UserProfile> GetByActivation(bool deactivated)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId, u.Active,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                         WHERE Active = @activated
                         ORDER BY DisplayName";
                    cmd.Parameters.AddWithValue("@activated", !deactivated);

                    List<UserProfile> users = new List<UserProfile>();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        UserProfile user = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            Active = reader.GetBoolean(reader.GetOrdinal("Active")),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                        users.Add(user);
                    }

                    reader.Close();

                    return users;
                }
            }
        }
        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId, u.Active,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email AND Active = 1";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            Active = reader.GetBoolean(reader.GetOrdinal("Active")),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }
        public UserProfile GetById(int Id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id AS userProfileId, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId, u.Active,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE u.Id = @id";
                    cmd.Parameters.AddWithValue("@id", Id);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("userProfileId")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            Active = reader.GetBoolean(reader.GetOrdinal("Active")),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }
        public void DeactivateUser(UserProfile user)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            DECLARE @ActiveAdmins INT
                            SELECT @ActiveAdmins = Count(*)
                            FROM UserProfile
                            WHERE UserTypeId = 1 AND Active = 1

                            IF @ActiveAdmins = 1 AND @userType = 1
                                RAISERROR('System must have at least one administrator.', 16, 1)
                            ELSE
                                UPDATE UserProfile
                                SET Active = 0
                                WHERE Id = @id";

                        cmd.Parameters.AddWithValue("@id", user.Id);
                        cmd.Parameters.AddWithValue("@userType", user.UserTypeId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(SqlException ex)
            {
                throw new Errors.AdministratorTypeException(ex.Message);
            }
        }
        public void ReactivateUser(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE UserProfile
                            SET Active = 1
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUserType(UserProfile user)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            DECLARE @ActiveAdmins INT
                            SELECT @ActiveAdmins = Count(*)
                            FROM UserProfile
                            WHERE UserTypeId = 1 AND Active = 1

                            IF @ActiveAdmins = 1 AND @userType = 2
                                RAISERROR('System must have at least one administrator.', 16, 1)
                            ELSE
                                UPDATE UserProfile
                                SET UserTypeId = @userType
                                WHERE Id = @id";

                        cmd.Parameters.AddWithValue("@userType", user.UserTypeId);
                        cmd.Parameters.AddWithValue("@id", user.Id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(SqlException ex)
            {
                throw new Errors.AdministratorTypeException(ex.Message);
            }
        }
    }
}
