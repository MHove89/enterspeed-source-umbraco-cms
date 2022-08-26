﻿using Enterspeed.Source.UmbracoCms.V10.Data.Migration;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Enterspeed.Source.UmbracoCms.V10.Components
{
    public class EnterspeedJobsComponent : IComponent
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IKeyValueService _keyValueService;
        private readonly IMigrationPlanExecutor _migrationPlanExecutor;

        public EnterspeedJobsComponent(
            IScopeProvider scopeProvider,
            IKeyValueService keyValueService,
            IMigrationPlanExecutor migrationPlanExecutor)
        {
            _scopeProvider = scopeProvider;
            _keyValueService = keyValueService;
            _migrationPlanExecutor = migrationPlanExecutor;
        }

        public void Initialize()
        {
            var migrationPlan = new MigrationPlan("EnterspeedJobs");
            migrationPlan.From(string.Empty)
                .To<EnterspeedJobsTableMigration>("enterspeedjobs-db")
                .To<AddEntityTypeToJobsTable>("enterspeedjobs-db-v2")
                .To<AddContentStateToJobsTable>("enterspeedjobs-db-v3");

            var upgrader = new Upgrader(migrationPlan);
            upgrader.Execute(_migrationPlanExecutor, _scopeProvider, _keyValueService);
        }

        public void Terminate()
        {
        }
    }
}