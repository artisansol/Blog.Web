using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Web.Models.Views.Components.Timelines;
using Blog.Web.Views.Components.Timelines;
using FluentAssertions;
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
    }
}
