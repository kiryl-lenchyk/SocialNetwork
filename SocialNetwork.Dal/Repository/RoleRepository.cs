﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using SocialNetwork.Dal.ExpressionMappers;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Interface.Repository;
using SocialNetwork.Dal.Mappers;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Repository
{
    public class RoleRepository : IRoleRepository
    {

        private readonly DbContext context;

        private bool isDisposed;

        public RoleRepository(DbContext context)
        {
            isDisposed = false;
            this.context = context;
        }
        

        public IEnumerable<DalRole> GetAll()
        {
            if (isDisposed) throw new ObjectDisposedException("RoleRepository");

            return context.Set<Role>().ToList().Select(x => x.ToDalRole());
        }

        public DalRole GetById(int key)
        {
            if (isDisposed) throw new ObjectDisposedException("RoleRepository");

            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Id == key);
            return ormRole == null ? null : ormRole.ToDalRole();

        }

        public DalRole GetByPredicate(Expression<Func<DalRole, bool>> predicate)
        {
            if (isDisposed) throw new ObjectDisposedException("RoleRepository");

            Expression<Func<Role, bool>> convertedPredicate =
                 (Expression<Func<Role, bool>>)(new GenericExpressionMapper<DalRole,Role>().Visit(predicate));

            Role ormRole = context.Set<Role>().FirstOrDefault(convertedPredicate);
            return ormRole != null ? ormRole.ToDalRole() : null;
        }

        public IEnumerable<DalRole> GetAllByPredicate(Expression<Func<DalRole, bool>> predicate)
        {
            if (isDisposed) throw new ObjectDisposedException("RoleRepository");

            Expression<Func<Role, bool>> convertedPredicate =
                 (Expression<Func<Role, bool>>)(new GenericExpressionMapper<DalRole, Role>().Visit(predicate));

            return context.Set<Role>().Where(convertedPredicate).ToList().Select(x => x.ToDalRole());
        }

        public DalRole Create(DalRole e)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            Role ormRole = e.ToOrmRole();
            context.Set<Role>().Add(ormRole);
            return ormRole.ToDalRole();
        }

        public void Delete(DalRole e)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            Role ormRole = e.ToOrmRole();
            context.Set<Role>().Remove(ormRole);
        }

        public void Update(DalRole e)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            Role ormRole = e.ToOrmRole();
            context.Set<Role>().AddOrUpdate(ormRole);
        }

        public DalRole GetByName(string roleName)
        {
            if (isDisposed) throw new ObjectDisposedException("RoleRepository");

            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Name == roleName);
            return ormRole == null ? null : ormRole.ToDalRole();
        }

        public IEnumerable<DalRole> GetUserRoles(DalUser user)
        {
            if (isDisposed) throw new ObjectDisposedException("RoleRepository");

            User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == user.Id);
            if (ormUser == null) return new List<DalRole>();
            return ormUser.Roles.Select(x => x.ToDalRole());
        }

        public IEnumerable<DalUser> GetRoleUsers(DalRole role)
        {
            if (isDisposed) throw new ObjectDisposedException("RoleRepository");

            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Id == role.Id);
            if (ormRole == null) return new List<DalUser>();
            return
                context.Set<User>().Where(x => x.Roles.Contains(ormRole)).Select(x => x.ToDalUser());
        }

        public void AddUserToRole(DalUser user, DalRole role)
        {
            if (isDisposed) throw new ObjectDisposedException("RoleRepository");

            User ormUser = GetOrmUserWithRoles(user);
            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Id == role.Id);
            if (ormRole == null) throw new ArgumentException("Role has incorrect id");

            ormUser.Roles.Add(ormRole);
        }

        public void RmoveUserFromRole(DalUser user, DalRole role)
        {
            if (isDisposed) throw new ObjectDisposedException("RoleRepository");

            User ormUser = GetOrmUserWithRoles(user);
            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Id == role.Id);
            if (ormRole == null) throw new ArgumentException("Role has incorrect id");

            ormUser.Roles.Remove(ormRole);
        }

        private User GetOrmUserWithRoles(DalUser dalUser)
        {
            User ormCurrentUser = context.Set<User>().SingleOrDefault(x => x.Id == dalUser.Id);
            if (ormCurrentUser == null) throw new ArgumentException("User has incorrect id");
            context.Entry(ormCurrentUser).Collection(x => x.Roles).Load();
            return ormCurrentUser;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                context.Dispose();
                isDisposed = true;
            }
        }  
    }
}