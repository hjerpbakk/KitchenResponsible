using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KitchenResponsibleService;
using KitchenResponsibleService.Db;
using KitchenResponsibleService.Model;
using KitchenResponsibleService.Services;
using Moq;
using Xunit;

namespace KitchenResponsibleServiceTests
{
    public class KitchenServiceTests
    {
        [Fact]
        public async Task AddNewEmployee()
        {
            const string NewEmployee = "Annette";
            ConfigurableDateTime.CurrentTime = new DateTime(2017, 9, 20);
            var employees = new List<string> { "Runar", "Phuong", "Malin" };
            var initialWeeksAndResponsibles = new List<ResponsibleForWeek> {
                new ResponsibleForWeek(37, "Malin"),
                new ResponsibleForWeek(38, "Runar")
            };
			var expectedWeeksAndResponsibles = new List<ResponsibleForWeek> {
				new ResponsibleForWeek(38, "Runar"),
                new ResponsibleForWeek(39, "Phuong"),
                new ResponsibleForWeek(40, "Malin"),
                new ResponsibleForWeek(41, "Annette")
			};

            var storageFake = new Mock<IStorage>();
            storageFake.Setup(s => s.GetEmployees()).ReturnsAsync(() => employees.ToArray());
            storageFake.Setup(s => s.GetWeeksAndResponsibles()).ReturnsAsync(initialWeeksAndResponsibles);
            storageFake.Setup(s => s.AddNewEmployee("Annette")).Returns(Task.CompletedTask).Callback(() => { employees.Add("Annette"); });
            Action<List<ResponsibleForWeek>> verifySave = (weeksAndResponsibles) =>
            {
                Assert.Equal(expectedWeeksAndResponsibles, weeksAndResponsibles);
            };
            storageFake.Setup(s => s.Save(It.IsAny<List<ResponsibleForWeek>>())).Returns(Task.CompletedTask).Callback(verifySave);

            var kitchenService = new KitchenService(storageFake.Object);

            await kitchenService.AddNewEmployee(NewEmployee);

            storageFake.Verify(s => s.Save(It.IsAny<List<ResponsibleForWeek>>()), Times.Once());
        }
    }
}
