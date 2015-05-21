﻿using System;
using System.Linq;
using NWheels.Entities;

namespace NWheels.Processing.Workflows.Core
{
    public abstract class WorkflowTypeRegistration
    {
        public abstract IQueryable<IWorkflowInstanceEntity> GetDataEntityQuery(IFramework framework, out IUnitOfWork unitOfWork);
        public abstract Type CodeBehindType { get; }
        public abstract Type DataEntityType { get; }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------

    public class WorkflowTypeRegistration<TCodeBehind, TDataRepository, TDataEntity> : WorkflowTypeRegistration
        where TCodeBehind : class, IWorkflowCodeBehind
        where TDataRepository : class, IApplicationDataRepository
        where TDataEntity : class, IWorkflowInstanceEntity
    {
        private readonly Func<TDataRepository, IQueryable<TDataEntity>> _entityRepositoryFunc;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public WorkflowTypeRegistration(Func<TDataRepository, IQueryable<TDataEntity>> entityRepositoryFunc)
        {
            _entityRepositoryFunc = entityRepositoryFunc;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public override IQueryable<IWorkflowInstanceEntity> GetDataEntityQuery(IFramework framework, out IUnitOfWork unitOfWork)
        {
            var dataRepository = framework.NewUnitOfWork<TDataRepository>();
            var dataEntityQueryable = _entityRepositoryFunc(dataRepository);

            unitOfWork = dataRepository;
            return dataEntityQueryable;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public override Type CodeBehindType
        {
            get
            {
                return typeof(TCodeBehind);
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public override Type DataEntityType
        {
            get
            {
                return typeof(TDataEntity);
            }
        }
    }
}
