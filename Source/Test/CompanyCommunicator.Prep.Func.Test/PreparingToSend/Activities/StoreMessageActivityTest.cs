﻿// <copyright file="StoreMessageActivityTest.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Prep.Func.Test.PreparingToSend.Activities
{
    using System;
    using System.Threading.Tasks;
    using AdaptiveCards;
    using FluentAssertions;
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.NotificationData;
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Services.AdaptiveCard;
    using Microsoft.Teams.Apps.CompanyCommunicator.Prep.Func.PreparingToSend;
    using Moq;
    using Xunit;

    /// <summary>
    /// StoreMessageActivity test class.
    /// </summary>
    public class StoreMessageActivityTest
    {
        private readonly Mock<AdaptiveCardCreator> adaptiveCardCreator = new Mock<AdaptiveCardCreator>();
        private readonly Mock<ISendingNotificationDataRepository> sendingNotificationDataRepository = new Mock<ISendingNotificationDataRepository>();

        /// <summary>
        /// Constructor tests.
        /// </summary>
        [Fact]
        public void StoreMessageActivityConstructorTest()
        {
            // Arrange
            Action action1 = () => new StoreMessageActivity(null /*notificationRepo*/, this.adaptiveCardCreator.Object);
            Action action2 = () => new StoreMessageActivity(this.sendingNotificationDataRepository.Object, null /*cardCreator*/);
            Action action3 = () => new StoreMessageActivity(this.sendingNotificationDataRepository.Object, this.adaptiveCardCreator.Object);

            // Act and Assert.
            action1.Should().Throw<ArgumentNullException>("notificationRepo is null.");
            action2.Should().Throw<ArgumentNullException>("cardCreator is null.");
            action3.Should().NotThrow();
        }

        /// <summary>
        /// Store message in repository.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [Fact]
        public async Task StoreMessageActivitySuccessTest()
        {
            // Arrange
            NotificationDataEntity notification = new NotificationDataEntity()
            {
                NotificationId = "123",
            };
            var activityContext = this.GetStoreMessageActivity();
            AdaptiveCard adaptiveCard = new AdaptiveCard("1.2");
            this.adaptiveCardCreator
                .Setup(x => x.CreateAdaptiveCard(It.IsAny<NotificationDataEntity>()))
                .Returns(adaptiveCard);
            this.sendingNotificationDataRepository
                .Setup(x => x.CreateOrUpdateAsync(It.IsAny<SendingNotificationDataEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> task = async () => await activityContext.RunAsync(notification);

            // Assert
            await task.Should().NotThrowAsync();
            this.adaptiveCardCreator.Verify(x => x.CreateAdaptiveCard(It.Is<NotificationDataEntity>(x => x.NotificationId == notification.NotificationId)));
            this.sendingNotificationDataRepository.Verify(x => x.CreateOrUpdateAsync(It.Is<SendingNotificationDataEntity>(x => x.NotificationId == notification.NotificationId)));
        }

        /// <summary>
        /// Failure test for the Store message repository. ArgumentNullException thrown for NofiticationDataEntity null.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [Fact]
        public async Task StoreMessageActivityFailureTest()
        {
            // Arrange
            var activityContext = this.GetStoreMessageActivity();
            AdaptiveCard adaptiveCard = new AdaptiveCard();
            this.adaptiveCardCreator
                .Setup(x => x.CreateAdaptiveCard(It.IsAny<NotificationDataEntity>()))
                .Returns(adaptiveCard);
            this.sendingNotificationDataRepository
                .Setup(x => x.CreateOrUpdateAsync(It.IsAny<SendingNotificationDataEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> task = async () => await activityContext.RunAsync(null);

            // Assert
            await task.Should().ThrowAsync<ArgumentNullException>("notification is null");
            this.sendingNotificationDataRepository.Verify(x => x.CreateOrUpdateAsync(It.IsAny<SendingNotificationDataEntity>()), Times.Never());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreMessageActivity"/> class.
        /// </summary>
        /// <returns>return the instance of StoreMessageActivity.</returns>
        private StoreMessageActivity GetStoreMessageActivity()
        {
            return new StoreMessageActivity(this.sendingNotificationDataRepository.Object, this.adaptiveCardCreator.Object);
        }
    }
}
