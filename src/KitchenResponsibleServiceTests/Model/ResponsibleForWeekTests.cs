using System.Collections.Generic;
using KitchenResponsibleService.Model;
using Xunit;

namespace KitchenResponsibleServiceTests.Model
{   
    public class ResponsibleForWeekTests
    {
        [Fact]
        public void Parse_StringToList() {
            const string WeeksAndResponsibles = "37 U1TBU8336\n38 U1TBU8336\n39 U1TBU8336\n40 U1TBU8336\n41 U1TBU8336\n42 U1TBU8336";
			var expectedWeeksAndResponsibles = new List<ResponsibleForWeek> {
				new ResponsibleForWeek(37, "U1TBU8336"),
				new ResponsibleForWeek(38, "U1TBU8336"),
				new ResponsibleForWeek(39, "U1TBU8336"),
                new ResponsibleForWeek(40, "U1TBU8336"),
                new ResponsibleForWeek(41, "U1TBU8336"),
                new ResponsibleForWeek(42, "U1TBU8336"),
			};

            var actual = ResponsibleForWeek.Parse(WeeksAndResponsibles);

            Assert.Equal(expectedWeeksAndResponsibles, actual);
        }

		[Fact]
		public void Parse_ListToString()
		{
			const string ExpectedWeeksAndResponsibles = "37 U1TBU8336\n38 U1TBU8336\n39 U1TBU8336\n40 U1TBU8336\n41 U1TBU8336\n42 U1TBU8336";
			var weeksAndResponsibles = new List<ResponsibleForWeek> {
				new ResponsibleForWeek(37, "U1TBU8336"),
				new ResponsibleForWeek(38, "U1TBU8336"),
				new ResponsibleForWeek(39, "U1TBU8336"),
				new ResponsibleForWeek(40, "U1TBU8336"),
				new ResponsibleForWeek(41, "U1TBU8336"),
				new ResponsibleForWeek(42, "U1TBU8336"),
			};

			var actual = ResponsibleForWeek.Parse(weeksAndResponsibles);

			Assert.Equal(ExpectedWeeksAndResponsibles, actual);
		}
    }
}
