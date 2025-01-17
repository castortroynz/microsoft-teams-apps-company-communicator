﻿// <copyright file="RepositoryOptions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories
{
    /// <summary>
    /// Options used for creating repositories.
    /// </summary>
    public class RepositoryOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryOptions"/> class.
        /// </summary>
        public RepositoryOptions() {
            // Default this option to true as ensuring the table exists is technically
            // more robust.
            this.EnsureTableExists = true;
        }

        /// <summary>
        /// Gets or sets the storage account connection string.
        /// </summary>
        public string StorageAccountConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the optional storage account connection string for 
        /// Blob Storages when using CosmosDb as table storage
        /// </summary>
        public string BlobStorageAccountConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the table should be created
        /// if it does not already exist.
        /// </summary>
        public bool EnsureTableExists { get; set; }

        /// <summary>
        /// Gets or sets a value indicating tha maximum number of sent notifications to retrieve from storage.
        /// </summary>
        public int MaxSentNotificationsCount { get; set; }
    }
}