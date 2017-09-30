using System.Collections.Generic;
using KitchenResponsibleService.Model;
using Xunit;

namespace KitchenResponsibleServiceTests.Model
{   
    public class ResponsibleForWeekTests
    {
        [Fact]
        public void Parse_StringToList() {
            const string WeeksAndResponsibles = "37;U1TBU8336;Runar Ovesen Hjerpbakk\n38;U1TBU8336;Runar Ovesen Hjerpbakk\n39;U1TBU8336;Runar Ovesen Hjerpbakk\n40;U1TBU8336;Runar Ovesen Hjerpbakk\n41;U1TBU8336;Runar Ovesen Hjerpbakk\n42;U1TBU8336;Runar Ovesen Hjerpbakk";
			var expectedWeeksAndResponsibles = new List<ResponsibleForWeek> {
				Create(37),
				Create(38),
				Create(39),
				Create(40),
				Create(41),
				Create(42)
			};

            var actual = ResponsibleForWeek.Parse(WeeksAndResponsibles);

            Assert.Equal(expectedWeeksAndResponsibles, actual);
        }

		[Fact]
		public void Parse_ListToString()
		{
			const string WeeksAndResponsibles = "37;U1TBU8336;Runar Ovesen Hjerpbakk\n38;U1TBU8336;Runar Ovesen Hjerpbakk\n39;U1TBU8336;Runar Ovesen Hjerpbakk\n40;U1TBU8336;Runar Ovesen Hjerpbakk\n41;U1TBU8336;Runar Ovesen Hjerpbakk\n42;U1TBU8336;Runar Ovesen Hjerpbakk";
			var weeksAndResponsibles = new List<ResponsibleForWeek> {
				Create(37),
				Create(38),
				Create(39),
				Create(40),
				Create(41),
				Create(42)
			};

			var actual = ResponsibleForWeek.Parse(weeksAndResponsibles);

			Assert.Equal(WeeksAndResponsibles, actual);
		}

		ResponsibleForWeek Create(ushort week) => new ResponsibleForWeek(week, new Employee("U1TBU8336", "Runar Ovesen Hjerpbakk"));
    }
}
