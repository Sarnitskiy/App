using App.DbAccess.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace App.DbAccess.Services
{
    public class Account : IAccount
    {
        private readonly string _connectionString;

        public Account(string connectionString) => _connectionString = connectionString;

        public async Task<AccountUser> GetUserAsync(string email, string password)
        {
            AccountUser user = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@email", email));
                string passwordHash = GetPasswordHash(password);
                cmd.Parameters.Add(new SqlParameter("@password", passwordHash));
                con.Open();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    user = new AccountUser();
                    user.Id = Convert.ToInt32(rdr["Id"]);
                    user.FirstName = rdr["FirstName"].ToString();
                    user.LastName = rdr["LastName"].ToString();
                    user.Email = rdr["Email"].ToString();
                }
                con.Close();
            }

            return user;
        }

        private string GetPasswordHash(string password)
        {
            byte[] salt = System.Text.Encoding.UTF8.GetBytes("salt");

            string hashPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashPassword;
        }

        public async Task<IEnumerable<AccountRole>> GetUserRolesAsync(int userId)
        {
            var roles = new List<AccountRole>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetUserRoles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@userId", userId));
                con.Open();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    var role = new AccountRole();
                    role.Id = Convert.ToInt32(rdr["Id"]);
                    role.Name = rdr["Name"].ToString();

                    roles.Add(role);
                }
                con.Close();
            }

            return roles;
        }

        public async Task AddUserRoleAsync(int userId, int roleId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddUserRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@userId", userId));
                cmd.Parameters.Add(new SqlParameter("@roleId", roleId));
                con.Open();

                await cmd.ExecuteNonQueryAsync();
                con.Close();
            }
        }

        public async Task DeleteUserRoleAsync(int userId, int roleId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteUserRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@userId", userId));
                cmd.Parameters.Add(new SqlParameter("@roleId", roleId));
                con.Open();

                await cmd.ExecuteNonQueryAsync();
                con.Close();
            }
        }
    }
}
