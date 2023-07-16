﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Models.Views.Components.Timelines;
using Blog.Web.Views.Bases;
using Blog.Web.Views.Components.Timelines;
using Bunit;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blog.Web.Unit.Tests.Components.Timelines
{
    public partial class TimelineComponentTests
    {
        [Fact]
        public void ShouldRenderErrorIfExceptionOccurs()
        {
            // given
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            string expectedErrorMessage = exceptionMessage;
            TimelineComponentState expectedState = TimelineComponentState.Error;

            var exception = 
                new Exception(message: exceptionMessage);

            this.postViewServiceMock.Setup(service => 
                service.RetrieveAllPostViewsAsync())
                    .ThrowsAsync(exception);

            // when
            this.renderedTimelineComponent = 
                RenderComponent<TimelineComponent>();

            // then
            this.renderedTimelineComponent.Instance.State
                .Should().Be(expectedState);

            this.renderedTimelineComponent.Instance.ErrorMessage
                .Should().Be(expectedErrorMessage);

            this.renderedTimelineComponent.Instance.Label.Value
                .Should().Be(expectedErrorMessage);

            IReadOnlyList<IRenderedComponent<CardBase>> postComponents =
                this.renderedTimelineComponent.FindComponents<CardBase>();

            postComponents.Should().HaveCount(0);

            this.renderedTimelineComponent.Instance.Spinner
                .Should().BeNull();

            this.postViewServiceMock.Verify(service => 
                service.RetrieveAllPostViewsAsync(),
                Times.Once());

            this.postViewServiceMock.VerifyNoOtherCalls();
        }
    }
}
