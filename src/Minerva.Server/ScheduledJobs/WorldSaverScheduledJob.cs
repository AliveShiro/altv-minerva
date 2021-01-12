﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minerva.Server.ScheduledJobs.Base;
using Minerva.Server.ServerJobs.Base;

namespace Minerva.Server.ScheduledJobs
{
    public class WorldSaverScheduledJob
        : ScheduledJob
    {
        private readonly IEnumerable<IServerJob> _serverJobs;

        private ILogger<WorldSaverScheduledJob> Logger { get; }

        public WorldSaverScheduledJob(ILogger<WorldSaverScheduledJob> logger, IConfiguration serverConfig, IEnumerable<IServerJob> serverJobs)
            : base(TimeSpan.FromSeconds(double.Parse(serverConfig.GetSection("World:SaveInterval")?.Value ?? "300")))
        {
            // if config not present then default of 5 minutes is applied in base constructor

            Logger = logger;
            _serverJobs = serverJobs;
        }

        public override async Task Action()
        {
            Logger.LogTrace("World save initiated at {CurrentDate}", DateTime.Now);

            var taskList = new List<Task>();

            foreach (var job in _serverJobs)
            {
                taskList.Add(job.OnSave());
            }

            await Task.WhenAll(taskList.ToArray());

            Logger.LogTrace("World save completed at {CurrentDate}", DateTime.Now);
        }
    }
}