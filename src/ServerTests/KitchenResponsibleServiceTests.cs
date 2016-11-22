using System;
using Xunit;
using KitchenResponsible.Services;
using Microsoft.Extensions.Options;
using Moq;
using KitchenResponsible.Utils.DateAndTime;
using System.IO;
using KitchenResponsible.Model;
using KitchenResponsible.Data;

namespace Tests {
    public class KitchenResponsibleServiceTests : IClassFixture<DbFixture> {
        readonly ITrondheimRepository trondheimRepository;        

        public KitchenResponsibleServiceTests(DbFixture dbFixture) {
            trondheimRepository = dbFixture.TrondheimRepository;
        }

        // TODO: Split into multiple tests
        [Fact]
        public void GetEmployeeForWeek() {
            var responsibleForWeek = GetResponsibleForWeek(47);

            Assert.Equal(47, responsibleForWeek.Week);
            Assert.Equal("Fredrik", responsibleForWeek.Responsible);
            Assert.Equal("Phuong", responsibleForWeek.OnDeck);

            responsibleForWeek = GetResponsibleForWeek(48);

            Assert.Equal(48, responsibleForWeek.Week);
            Assert.Equal("Phuong", responsibleForWeek.Responsible);
            Assert.Equal("Andreas", responsibleForWeek.OnDeck);

            responsibleForWeek = GetResponsibleForWeek(49);

            Assert.Equal(49, responsibleForWeek.Week);
            Assert.Equal("Andreas", responsibleForWeek.Responsible);
            Assert.Equal("Simon", responsibleForWeek.OnDeck);

            responsibleForWeek = GetResponsibleForWeek(50);

            Assert.Equal(50, responsibleForWeek.Week);
            Assert.Equal("Simon", responsibleForWeek.Responsible);
            Assert.Equal("Ivar LG", responsibleForWeek.OnDeck);

            responsibleForWeek = GetResponsibleForWeek(51);

            Assert.Equal(51, responsibleForWeek.Week);
            Assert.Equal("Ivar LG", responsibleForWeek.Responsible);
            Assert.Equal("Henrik", responsibleForWeek.OnDeck);

            responsibleForWeek = GetResponsibleForWeek(52);

            Assert.Equal(52, responsibleForWeek.Week);
            Assert.Equal("Henrik", responsibleForWeek.Responsible);
            Assert.Equal("Øyvin", responsibleForWeek.OnDeck);

            responsibleForWeek = GetResponsibleForWeek(1);

            Assert.Equal(1, responsibleForWeek.Week);
            Assert.Equal("Øyvin", responsibleForWeek.Responsible);
            Assert.Equal("Bernhard", responsibleForWeek.OnDeck);
        }

        private ResponsibleForWeek GetResponsibleForWeek(ushort week) {
            var weekNumberFinderFake = new Mock<IWeekNumberFinder>();
            weekNumberFinderFake.Setup(w => w.GetIso8601WeekOfYear(It.IsAny<DateTime>())).Returns(week);
            var service = new KitchenResponsibleService(trondheimRepository, weekNumberFinderFake.Object);

            return service.GetEmployeeForWeek();
        }
    }

    public class DbFixture : IDisposable {
        
        
        public DbFixture() {
            
        }

        public void Dispose() {
            
        }

        public ITrondheimRepository TrondheimRepository { get; }
    }
}
