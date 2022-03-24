using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataCollectionTrigger;
using System;
using FluentAssertions;

namespace UnitTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void IfSeasonDateIsInSecondYear_EndYearShouldBeYearBefore()
    {
        var currentDate = DateTime.Parse("02/02/2022");
        var cut = new DataCollection();

        int endSeason = cut.GetEndSeason(currentDate);
        endSeason.Should().Be(2021);
    }
    [TestMethod]
    public void IfSeasonDateIsInFirstYear_EndYearShouldBeCurrentYear()
    {
        var currentDate = DateTime.Parse("11/11/2021");
        var cut = new DataCollection();

        int endSeason = cut.GetEndSeason(currentDate);
        endSeason.Should().Be(2021);
    }
}
