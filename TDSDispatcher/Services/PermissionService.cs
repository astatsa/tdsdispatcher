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
            return new PermissionService(sessionContext, entityInfo.ModelName);
        }
    }

    class PermissionService
    {
        private readonly SessionContext sessionContext;
        private readonly string modelName;

        public PermissionService(SessionContext sessionContext, string modelName)
        {
            this.sessionContext = sessionContext;
            this.modelName = modelName;
        }

        public bool HasPermission(EntityOperations operation)
        {
            return sessionContext.Permissions.Contains($"{modelName}{operation}");
        }
    }

    enum EntityOperations
    {
        Read,
        Edit
    }
}
