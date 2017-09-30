using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KitchenResponsibleService;
using KitchenResponsibleService.Db;
using KitchenResponsibleService.Model;
using KitchenResponsibleService.Services;
using Moq;
using Xunit;

namespace KitchenResponsibleServiceTests.Services
{
    public class KitchenServiceTests
    {
        [Fact]
        public async Task AddNewEmployee()
        {
            var newEmployee = new Employee("Annette", "Annette Lillegaard");
            ConfigurableDateTime.CurrentTime = new DateTime(2017, 9, 20);
            var employees = new List<Employee> { new Employee("Runar", "Runar Ovesen Hjerpbakk"), new Employee("Phuong", "Pedersen"), new Employee("Malin", "Malin AB") };
            var initialWeeksAndResponsibles = new List<ResponsibleForWeek> {
                new ResponsibleForWeek(37, new Employee("Malin", "Malin AB")),
                new ResponsibleForWeek(38, new Employee("Runar", "Runar Ovesen Hjerpbakk"))
            };
			var expectedWeeksAndResponsibles = new List<ResponsibleForWeek> {
				new ResponsibleForWeek(38, new Employee("Runar", "Runar Ovesen Hjerpbakk")),
                new ResponsibleForWeek(39, new Employee("Phuong", "Pedersen")),
                new ResponsibleForWeek(40, new Employee("Malin", "Malin AB")),
                new ResponsibleForWeek(41, new Employee("Annette", "Annette Lillegaard"))
			};

            var storageFake = new Mock<IStorage>();
            storageFake.Setup(s => s.GetEmployees()).ReturnsAsync(() => employees.ToArray());
            storageFake.Setup(s => s.GetWeeksAndResponsibles()).ReturnsAsync(initialWeeksAndResponsibles);
            storageFake.Setup(s => s.AddNewEmployee(newEmployee)).Returns(Task.CompletedTask).Callback(() => { employees.Add(newEmployee); });
            Action<List<ResponsibleForWeek>> verifySave = (weeksAndResponsibles) =>
            {
                Assert.Equal(expectedWeeksAndResponsibles, weeksAndResponsibles);
            };
            storageFake.Setup(s => s.Save(It.IsAny<List<ResponsibleForWeek>>())).Returns(Task.CompletedTask).Callback(verifySave);

            var kitchenService = new KitchenService(storageFake.Object);

            await kitchenService.AddNewEmployee(newEmployee);

            storageFake.Verify(s => s.Save(It.IsAny<List<ResponsibleForWeek>>()), Times.Once());
        }

		[Fact]
		public async Task GetWeeksAndResponsibles()
		{
			ConfigurableDateTime.CurrentTime = new DateTime(2017, 9, 20);
			var employees = new List<Employee> { new Employee("Runar", "Runar Ovesen Hjerpbakk"), new Employee("Phuong", "Pedersen"), new Employee("Malin", "Malin AB") };
			var initialWeeksAndResponsibles = new List<ResponsibleForWeek> {
				new ResponsibleForWeek(37, new Employee("Malin", "Malin AB")),
				new ResponsibleForWeek(38, new Employee("Runar", "Runar Ovesen Hjerpbakk"))
			};
			var expectedWeeksAndResponsibles = new List<ResponsibleForWeek> {
				new ResponsibleForWeek(38, new Employee("Runar", "Runar Ovesen Hjerpbakk")),
				new ResponsibleForWeek(39, new Employee("Phuong", "Pedersen")),
				new ResponsibleForWeek(40, new Employee("Malin", "Malin AB"))
			};

			var storageFake = new Mock<IStorage>();
			storageFake.Setup(s => s.GetEmployees()).ReturnsAsync(() => employees.ToArray());
			storageFake.Setup(s => s.GetWeeksAndResponsibles()).ReturnsAsync(initialWeeksAndResponsibles);

			var kitchenService = new KitchenService(storageFake.Object);

            var weeksAndResponsibles = await kitchenService.GetWeeksAndResponsibles();

            Assert.Equal(expectedWeeksAndResponsibles, weeksAndResponsibles);
		}

        [Fact]
        public async Task GetWeekForUser()
        {
			ConfigurableDateTime.CurrentTime = new DateTime(2017, 9, 20);
			var employees = new List<Employee> { new Employee("Runar", "Runar Ovesen Hjerpbakk"), new Employee("Phuong", "Pedersen"), new Employee("Malin", "Malin AB") };
			var initialWeeksAndResponsibles = new List<ResponsibleForWeek> {
				new ResponsibleForWeek(37, new Employee("Malin", "Malin AB")),
				new ResponsibleForWeek(38, new Employee("Runar", "Runar Ovesen Hjerpbakk")),
                new ResponsibleForWeek(39, new Employee("Phuong", "Pedersen")),
			};
	
			var storageFake = new Mock<IStorage>();
			storageFake.Setup(s => s.GetEmployees()).ReturnsAsync(() => employees.ToArray());
			storageFake.Setup(s => s.GetWeeksAndResponsibles()).ReturnsAsync(initialWeeksAndResponsibles);

			var kitchenService = new KitchenService(storageFake.Object);

            var weekForPhuong = await kitchenService.GetWeekAndResponsibleForEmployee("Phuong");

            Assert.Equal(39, weekForPhuong.WeekNumber);
            Assert.Equal("Phuong", weekForPhuong.SlackUser);
			Assert.Equal("Pedersen", weekForPhuong.Name);
        }

		[Fact]
		public async Task GetWeekForUser_UserHasNoWeek()
		{
			ConfigurableDateTime.CurrentTime = new DateTime(2017, 9, 20);
			var employees = new List<Employee> { new Employee("Runar", "Runar Ovesen Hjerpbakk"), new Employee("Malin", "Malin AB") };
			var initialWeeksAndResponsibles = new List<ResponsibleForWeek> {
				new ResponsibleForWeek(37, new Employee("Malin", "Malin AB")),
				new ResponsibleForWeek(38, new Employee("Runar", "Runar Ovesen Hjerpbakk"))
			};

			var storageFake = new Mock<IStorage>();
			storageFake.Setup(s => s.GetEmployees()).ReturnsAsync(() => employees.ToArray());
			storageFake.Setup(s => s.GetWeeksAndResponsibles()).ReturnsAsync(initialWeeksAndResponsibles);

			var kitchenService = new KitchenService(storageFake.Object);

			var weekForPhuong = await kitchenService.GetWeekAndResponsibleForEmployee("Phuong");

			Assert.Equal(0, weekForPhuong.WeekNumber);
			Assert.Equal("Phuong", weekForPhuong.SlackUser);
			Assert.Null(weekForPhuong.Name);
		}

		[Fact]
        public async Task GetWeekAndResponsibleForWeek()
        {
			ConfigurableDateTime.CurrentTime = new DateTime(2017, 9, 20);
			var employees = new List<Employee> { new Employee("Runar", "Runar Ovesen Hjerpbakk"), new Employee("Phuong", "Pedersen"), new Employee("Malin", "Malin AB") };
			var initialWeeksAndResponsibles = new List<ResponsibleForWeek> {
				new ResponsibleForWeek(37, new Employee("Malin", "Malin AB")),
				new ResponsibleForWeek(38, new Employee("Runar", "Runar Ovesen Hjerpbakk")),
				new ResponsibleForWeek(39, new Employee("Phuong", "Pedersen")),
			};

			var storageFake = new Mock<IStorage>();
			storageFake.Setup(s => s.GetEmployees()).ReturnsAsync(() => employees.ToArray());
			storageFake.Setup(s => s.GetWeeksAndResponsibles()).ReturnsAsync(initialWeeksAndResponsibles);

			var kitchenService = new KitchenService(storageFake.Object);

			var weekForPhuong = await kitchenService.GetWeekAndResponsibleForWeek(39);

			Assert.Equal(39, weekForPhuong.WeekNumber);
			Assert.Equal("Phuong", weekForPhuong.SlackUser);
			Assert.Equal("Pedersen", weekForPhuong.Name);
        }

		[Fact]
		public async Task GetWeekAndResponsibleForWeek_NoResponsible()
		{
			ConfigurableDateTime.CurrentTime = new DateTime(2017, 9, 20);
			var employees = new List<Employee> { new Employee("Runar", "Runar Ovesen Hjerpbakk"), new Employee("Malin", "Malin AB") };
			var initialWeeksAndResponsibles = new List<ResponsibleForWeek> {
				new ResponsibleForWeek(37, new Employee("Malin", "Malin AB")),
				new ResponsibleForWeek(38, new Employee("Runar", "Runar Ovesen Hjerpbakk"))
			};

			var storageFake = new Mock<IStorage>();
			storageFake.Setup(s => s.GetEmployees()).ReturnsAsync(() => employees.ToArray());
			storageFake.Setup(s => s.GetWeeksAndResponsibles()).ReturnsAsync(initialWeeksAndResponsibles);

			var kitchenService = new KitchenService(storageFake.Object);

			var weekForPhuong = await kitchenService.GetWeekAndResponsibleForWeek(40);

			Assert.Equal(40, weekForPhuong.WeekNumber);
            Assert.Null(weekForPhuong.SlackUser);
			Assert.Null(weekForPhuong.Name);
		}

		[Fact]
		public async Task GetWeekAndResponsibleForCurrentWeek()
		{
			ConfigurableDateTime.CurrentTime = new DateTime(2017, 9, 20);
			var employees = new List<Employee> { new Employee("Runar", "Runar Ovesen Hjerpbakk"), new Employee("Phuong", "Pedersen"), new Employee("Malin", "Malin AB") };
			var initialWeeksAndResponsibles = new List<ResponsibleForWeek> {
				new ResponsibleForWeek(37, new Employee("Malin", "Malin AB")),
				new ResponsibleForWeek(38, new Employee("Runar", "Runar Ovesen Hjerpbakk")),
				new ResponsibleForWeek(39, new Employee("Phuong", "Pedersen")),
			};

			var storageFake = new Mock<IStorage>();
			storageFake.Setup(s => s.GetEmployees()).ReturnsAsync(() => employees.ToArray());
			storageFake.Setup(s => s.GetWeeksAndResponsibles()).ReturnsAsync(initialWeeksAndResponsibles);

			var kitchenService = new KitchenService(storageFake.Object);

            var weekAndResponsibleForCurrentWeek = await kitchenService.GetWeekAndResponsibleForCurrentWeek();

			Assert.Equal(38, weekAndResponsibleForCurrentWeek.WeekNumber);
			Assert.Equal("Runar", weekAndResponsibleForCurrentWeek.SlackUser);
			Assert.Equal("Runar Ovesen Hjerpbakk", weekAndResponsibleForCurrentWeek.Name);
		}

		[Fact]
		public async Task GetWeekAndResponsibleForCurrentWeek_NoResponsible()
		{
			ConfigurableDateTime.CurrentTime = new DateTime(2017, 10, 20);
			var employees = new List<Employee> { new Employee("Runar", "Runar Ovesen Hjerpbakk"), new Employee("Malin", "Malin AB") };
			var initialWeeksAndResponsibles = new List<ResponsibleForWeek> {
				new ResponsibleForWeek(37, new Employee("Malin", "Malin AB")),
				new ResponsibleForWeek(38, new Employee("Runar", "Runar Ovesen Hjerpbakk"))
			};

			var storageFake = new Mock<IStorage>();
			storageFake.Setup(s => s.GetEmployees()).ReturnsAsync(() => employees.ToArray());
			storageFake.Setup(s => s.GetWeeksAndResponsibles()).ReturnsAsync(initialWeeksAndResponsibles);

			var kitchenService = new KitchenService(storageFake.Object);

			var weekAndResponsibleForCurrentWeek = await kitchenService.GetWeekAndResponsibleForCurrentWeek();

			Assert.Equal(42, weekAndResponsibleForCurrentWeek.WeekNumber);
			Assert.Null(weekAndResponsibleForCurrentWeek.SlackUser);
			Assert.Null(weekAndResponsibleForCurrentWeek.Name);
		}
    }
}
