using System;
using System.Collections.Generic;
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
            TimelineComponentState expectedState =
                TimelineComponentState.Error;

            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            string expectedErrorMessage = exceptionMessage;
            string expectedImageUrl = "/imgs/error.jpg";

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

            this.renderedTimelineComponent.Instance.ErrorImage.Url
                .Should().Be(expectedImageUrl);

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
