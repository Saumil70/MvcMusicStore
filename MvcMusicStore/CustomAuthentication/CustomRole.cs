using MvcMusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MvcMusicStore.CustomAuthentication
{
    public class CustomRole : RoleProvider
    {
        MusicStoreEntites db = new MusicStoreEntites();












        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
                                                                                                                                                                                          public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
        }











        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
                                                                                                                                              public override string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userRoles = new string[] { };

            using (MusicStoreEntites dbContext = new MusicStoreEntites())
            {
                var selectedUser = (from us in dbContext.Users.Include("Roles")
                                    where string.Compare(us.Username, username, StringComparison.OrdinalIgnoreCase) == 0
                                    select us).FirstOrDefault();


                if (selectedUser != null)
                {
                    userRoles = new[] { selectedUser.Roles.Select(r => r.RoleName).ToString() };
                }

                return userRoles.ToArray();
            }


        }



        #region Overrides of Role Provider

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {

            if (!Roles.RoleExists("Admin"))
            {
                Roles.CreateRole("Admin");
            }

            if (!Roles.RoleExists("User"))
            {
                Roles.CreateRole("User");
            }

            if (usernames == null || roleNames == null || usernames.Length == 0 || roleNames.Length == 0)
            {
                throw new ArgumentException("Usernames and role names must not be null or empty.");
            }

            // Validate that all roles exist before attempting to add users to them
            if (roleNames.Any(roleName => !RoleExists(roleName)))
            {
                throw new InvalidOperationException("One or more specified roles do not exist.");
            }

            // Implement your logic to add users to roles.
            // In this example, we assume you have a method called AddUsersToRolesInDatabase to perform the operation.

            AddUsersToRolesInDatabase(usernames, roleNames);
        }

        public override void CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
            }

            if (RoleExists(roleName))
            {
                throw new InvalidOperationException($"Role '{roleName}' already exists.");
            }

            // Implement your logic to create the role.
            // In this example, we assume you have a method called CreateRoleInDatabase to perform the role creation.

            CreateRoleInDatabase(roleName);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }


        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
            }

            bool roleExists = YourRoleExistenceCheckMethod(roleName);

            return roleExists;
        }
        private bool YourRoleExistenceCheckMethod(string roleName)
        {
            return db.Roles.Any(r => r.RoleName == roleName);

        }
        private void CreateRoleInDatabase(string roleName)
        {
            var roleManager = Roles.Provider as CustomRole;
            if (!Roles.RoleExists(roleName))
            {
                db.Roles.Add(new Role { RoleName = roleName });
            }
            db.SaveChanges();
        }
        private void AddUsersToRolesInDatabase(string[] usernames, string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                // Check if the role exists in the database
                var role = db.Roles.FirstOrDefault(r => r.RoleName == roleName);

                if (role != null)
                {
                    foreach (var username in usernames)
                    {
                        // Retrieve the user from the database based on the username
                        var user = db.Users.FirstOrDefault(u => u.Username == username);

                        if (user != null)
                        {
                            // Check if the user is not already in the role
                            if (!user.Roles.Any(ur => ur.RoleName == roleName))
                            {
                                // Add the existing role to the user
                                user.Roles.Add(role);
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            db.SaveChanges();
        }



        #endregion
                       }
}