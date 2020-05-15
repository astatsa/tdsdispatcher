using System;
using System.Collections.Generic;
using System.Text;
using TDSDispatcher.Models;

namespace TDSDispatcher.Services
{
    class PermissionServiceBuilder
    {
        private readonly SessionContext sessionContext;

        public PermissionServiceBuilder(SessionContext sessionContext)
        {
            this.sessionContext = sessionContext;
        }

        public PermissionService Build(EntityInfo entityInfo)
        {
            return new PermissionService(sessionContext, entityInfo.Permission);
        }
    }

    class PermissionService
    {
        private readonly SessionContext sessionContext;
        private readonly string obj;

        public PermissionService(SessionContext sessionContext, string obj)
        {
            this.sessionContext = sessionContext;
            this.obj = obj;
        }

        public bool HasPermission(EntityOperations operation)
        {
            return sessionContext.Permissions.Contains($"{obj}{operation}");
        }
    }

    enum EntityOperations
    {
        Read,
        Edit
    }
}
