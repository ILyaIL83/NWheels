﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWheels.Processing;
using Quartz;

namespace NWheels.Puzzle.QuartzNet
{
    [DisallowConcurrentExecution]
    internal class QuartzJobAdapter : IInterruptableJob
    {
        private readonly IApplicationJob _jobComponent;
        private readonly IJobLogger _logger;
        private readonly IJobDetail _jobDetail;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public QuartzJobAdapter(IJobDetail jobDetail, IApplicationJob jobComponent, Auto<IJobLogger> logger)
        {
            _jobDetail = jobDetail;
            _logger = logger.Instance;
            _jobComponent = jobComponent;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public void Execute(IJobExecutionContext context)
        {
            using ( _logger.ExecutingJob(jobName: context.JobDetail.Key.Name, triggerName: context.Trigger.Key.Name) )
            {
                try
                {
                    _logger.JobInvocationDetails(context.FireInstanceId, context.FireTimeUtc, context.PreviousFireTimeUtc, context.NextFireTimeUtc);

                    using ( var adaptedContext = new QuartzJobContextAdapter(context) )
                    {
                        _jobComponent.Execute(adaptedContext);
                    }

                    _logger.JobCompleted(context.JobDetail.Key.Name);
                }
                catch ( Exception e )
                {
                    _logger.JobFailed(context.JobDetail.Key.Name, e);
                    throw;
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public void Interrupt()
        {
            
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public IApplicationJob JobComponent
        {
            get { return _jobComponent; }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------


    }
}
