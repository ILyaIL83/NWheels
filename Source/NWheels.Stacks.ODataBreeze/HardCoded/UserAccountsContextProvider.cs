﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Breeze.ContextProvider;
using Hapil;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Csdl;
using Microsoft.Data.Edm.Library;
using NWheels.DataObjects;
using NWheels.Domains.Security;
using NWheels.Extensions;

namespace NWheels.Stacks.ODataBreeze.HardCoded
{
    public class UserAccountsContextProvider : ContextProvider
    {
        private readonly IFramework _framework;
        private readonly ITypeMetadataCache _metadataCache;
        private readonly IUserAccountDataRepository _querySource;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public UserAccountsContextProvider(IFramework framework, ITypeMetadataCache metadataCache)
        {
            _framework = framework;
            _metadataCache = metadataCache;
            _querySource = framework.NewUnitOfWork<IUserAccountDataRepository>(autoCommit: false);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public string GetRepositoryMetadataString(bool fullEdmx)
        {
            var builder = new EdmModelBuilder(_metadataCache);

            builder.AddEntity(_metadataCache.GetTypeMetadata(typeof(IUserAccountEntity)));
            builder.AddEntity(_metadataCache.GetTypeMetadata(typeof(IUserRoleEntity)));
            builder.AddEntity(_metadataCache.GetTypeMetadata(typeof(IOperationPermissionEntity)));
            builder.AddEntity(_metadataCache.GetTypeMetadata(typeof(IEntityAccessRuleEntity)));

            return builder.GetModelXmlString();
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public IUserAccountDataRepository QuerySource
        {
            get { return _querySource; }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected override string BuildJsonMetadata()
        {
            var builder = new BreezeMetadataBuilder(_metadataCache);

            builder.AddDataService("rest/UserAccounts/");
            builder.AddEntity(typeof(IUserAccountEntity));
            builder.AddEntity(typeof(IFrontEndUserAccountEntity));
            builder.AddEntity(typeof(IBackEndUserAccountEntity));

            return builder.GetMetadataJsonString();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected override void CloseDbConnection()
        {
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public override System.Data.IDbConnection GetDbConnection()
        {
            return null;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected override void OpenDbConnection()
        {
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected override void SaveChangesCore(SaveWorkState saveWorkState)
        {
            throw new NotImplementedException();
        }
    }
}
