// <copyright file="RepositoryOptionsExtensions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories
{
    public static class RepositoryOptionsExtensions
    {
        public static IServiceCollection AddRepositoryOptions(this IServiceCollection services, bool isItExpectedThatTableAlreadyExists = true)
        {
            services.AddOptions<RepositoryOptions>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                settings.StorageAccountConnectionString = configuration.GetValue<string>("StorageAccountConnectionString");
                settings.BlobStorageAccountConnectionString = configuration.GetValue<string>("BlobStorageAccountConnectionString");
                if (string.IsNullOrWhiteSpace(settings.BlobStorageAccountConnectionString))
                {
                    settings.BlobStorageAccountConnectionString = settings.StorageAccountConnectionString;
                }

                settings.EnsureTableExists = !configuration.GetValue<bool>("IsItExpectedThatTableAlreadyExists", isItExpectedThatTableAlreadyExists);
                settings.MaxSentNotificationsCount = configuration.GetValue<int>("MaxSentNotificationsCount", 20);
            });
            return services;
        }
    }
}