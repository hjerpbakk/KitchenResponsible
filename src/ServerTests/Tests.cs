using System;
using Xunit;
using KitchenResponsible.Services;
using Microsoft.Extensions.Options;
using Moq;
using KitchenResponsible.Utils.DateAndTime;

namespace Tests
{
    public class FileBasedKitchenResponsibleServiceTests
    {
        readonly IOptions<Paths> paths;

        public FileBasedKitchenResponsibleServiceTests() {
            paths = Options.Create(new Paths { FilePath = "" });
        }

        [Fact]
        public void SameWeekAsLastTime() {
            var weekNumberFinderFake = new Mock<IWeekNumberFinder>();
            weekNumberFinderFake.Setup(w => w.GetIso8601WeekOfYear(It.IsAny<DateTime>())).Returns(47);
            var service = new FileBasedKitchenResponsibleService(paths, weekNumberFinderFake.Object);

            var responsibleForWeek = service.GetEmployeeForWeek();

            Assert.Equal(47, responsibleForWeek.Week);
            Assert.Equal("Fredrik", responsibleForWeek.Responsible);
            Assert.Equal("Phuong", responsibleForWeek.OnDeck);
        }
    }
}
