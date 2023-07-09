using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Models.PostViews;
using Blog.Web.Models.Views.Components.Timelines;
using Blog.Web.Views.Bases;
using Blog.Web.Views.Components.Timelines;
using Bunit;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace Blog.Web.Unit.Tests.Components.Timelines
{
    public partial class TimelineComponentTests
    {
        [Fact]
        public void ShouldInitComponent()
        {
            // given
            TimelineComponentState expectedState = 
                TimelineComponentState.Loading;

            // when
            var initialTimelineComponent = 
                new TimelineComponent();

            // then
            initialTimelineComponent.PostViewService.Should().BeNull();
            initialTimelineComponent.PostViews.Should().BeNull();
            initialTimelineComponent.Label.Should().BeNull();
            initialTimelineComponent.ErrorMessage.Should().BeNull();
            initialTimelineComponent.State.Should().Be(expectedState);
        }

        [Fact]
        public async void ShouldRenderPosts()
        {
            // given
            TimelineComponentState expectedState = 
                TimelineComponentState.Content;

            List<PostView> randomPostViews = 
                CreateRandomPostViews();

            List<PostView> retrievedPostViews = 
                randomPostViews;

            List<PostView> expectedPostViews = 
                retrievedPostViews.DeepClone();

            this.postViewServiceMock.Setup(service =>
                service.RetrieveAllPostViewsAsync())
                    .ReturnsAsync(retrievedPostViews);

            // when
            this.renderedTimelineComponent =
                RenderComponent<TimelineComponent>();

            // then
            this.renderedTimelineComponent.Instance.State
                .Should().Be(expectedState);

            this.renderedTimelineComponent.Instance.PostViews
                .Should().BeEquivalentTo(expectedPostViews);

            IReadOnlyList<IRenderedComponent<CardBase>> postComponents =
                this.renderedTimelineComponent.FindComponents<CardBase>();

            postComponents.Should().HaveCount(expectedPostViews.Count);

            this.postViewServiceMock.Verify(service => 
                service.RetrieveAllPostViewsAsync(),
                    Times.Once());

            this.postViewServiceMock.VerifyNoOtherCalls();
        }
    }
}
