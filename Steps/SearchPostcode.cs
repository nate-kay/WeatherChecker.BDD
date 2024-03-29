﻿using OpenQA.Selenium;
using WeatherChecker.BDD.PageObjects;
using TechTalk.SpecFlow;
using System;
using FluentAssertions;
using RestSharp.Serialization.Json;
using WeatherChecker.BDD.ApiHelper;

namespace WeatherChecker.BDD.StepDefinitions
{
    [Binding]
    public class SearchPostcode
    {
        private readonly SearchPostCodePage _page;
        readonly Random random = new();


        private readonly string[] validPostcode = { "W6 0NW", "W6 0NW", "W6 0NW", "W6 0NW", "W6 0NW"};
        private readonly string[] invalidPostcode = { "B99 9AA", "B99 9AB", "B99 9AB", "B99 9AB", "B99 9AB" };
        private readonly string[] nonExistingInvalidPostcode = { "INV LID", "TE ST", "TE ST", "TE ST", "TE ST" };


        /**
        private readonly string postCodeNotFoundMessage = "Unable to find the postcode.";
        private readonly string postCodeNotValidMessage = "Invalid postcode.";
        **/

        private readonly string postCodeNotFoundMessage = "Postcode not found!";
        private readonly string postCodeNotValidMessage = "Postcode not valid.";

        public SearchPostcode(IWebDriver driver)
        {
            _page = new SearchPostCodePage(driver);
        }

        [When(@"User open Weather Checker application")]
        public void GivenUserOpenWeatherCheckerApplication()
        {
            _page.OpenApplication();
        }

        [When(@"User search valid existing postcode")]
        public void WhenUserSearchValidExistingPostcode()
        {
            _page.SearchPostcode(validPostcode[random.Next(5)]);
        }

        [Then(@"Weather details should be presented to the user")]
        public void ThenWeatherDetailsShouldBePresentedToTheUser()
        {
            _page.IsWeatherDetailsDisplayed().Should().BeTrue();
        }

        [Then(@"Weather details should display Time in correct format")]
        public void ThenWeatherDetailsShouldDisplayTimeInCorrectFormat()
        {
            _page.CheckWeatherTimeFormat().Should().BeTrue();
            
        }

        [Then(@"Weather details should display Temperature and Humidity")]
        public void ThenWeatherDetailsShouldDisplayTemperatureAndHumidity()
        {
            _page.CheckTemprateureHumidityDisplayed().Should().BeTrue();
        }

        [When(@"User search valid non-existing postcode")]
        public void WhenUserSearchValidNon_ExistingPostcode()
        {
            _page.SearchPostcode(invalidPostcode[random.Next(5)]);
        }

        [When(@"User search invalid postcode")]
        public void WhenUserSearchInvalidPostcode()
        {
            _page.SearchPostcode(nonExistingInvalidPostcode[random.Next(5)]);
        }

        [Then(@"Weather details should not display properties without value")]
        public void ThenWeatherDetailsShouldNotDisplayPropertiesWithoutValue()
        {
            _page.CheckAllWeatherPropeties().Should().BeTrue();
        }

        [Then(@"App should inform the user that postcode not found")]
        public void ThenAppShouldInformTheUserThatItIsUnanbleFindThePostcode()
        {
            _page.CheckErrorMessage().Should().Equals(postCodeNotFoundMessage);
        }

        [Then(@"App should inform the user that postcode not valid")]
        public void ThenAppShouldInformTheUserThatPostcodeIsInvalid()
        {
            _page.CheckErrorMessage().Should().Equals(postCodeNotValidMessage);
        }

        [Then(@"Weather details should match with the Weather App API response")]
        public void ThenWeatherDetailsShouldMatchWithTheWeatherAppAPIResponse()
        {
            WeatherAppResponse weatherAppResponse = new JsonDeserializer().Deserialize<WeatherAppResponse>(WeatherAppApi.actualResponse);
            weatherAppResponse.currently.temperature.Should().Equals(_page.weatherTemprature);
            weatherAppResponse.currently.humidity.Should().Equals(_page.weatherHumidity);
        }
    }
}