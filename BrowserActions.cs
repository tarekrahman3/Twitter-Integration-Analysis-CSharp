﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium
{
    public class BrowserActions
    {
        public IWebDriver WebDriver { get; }
        public IJavaScriptExecutor jsExecutor { get; }

        public BrowserActions(IWebDriver webDriver)
        {
            WebDriver = webDriver;
            jsExecutor = (IJavaScriptExecutor)WebDriver;
        }

        public IReadOnlyCollection<IWebElement> GetTweets(string url)
        {

            WebDriver.Navigate().GoToUrl(url);

            Thread.Sleep(6000);

            for (int i = 0; i < 3; i++)
            {
                jsExecutor.ExecuteScript("window.scrollBy(0,window.innerHeight)");
            }

            return WebDriver.FindElements(By.XPath(Constants.tweetsXpath));
        }

        public List<string> GetNonRetweetedUrlFormTwitterProfile(IReadOnlyCollection<IWebElement> elements)
        {
            var urls = new List<string>();

            foreach (var element in elements)
            {
                var isRetweet = IsRetweet(element);

                if (!isRetweet)
                {
                    var tweetUrl = element.FindElement(By.XPath(Constants.retweetTestXpath)).GetAttribute("href");

                    urls.Add(tweetUrl);
                }
            }

            return urls;
        }


        public bool IsRetweet(IWebElement element)
        {
            try
            {
                var result = element.FindElement(By.XPath(Constants.retweetXpath));

                if (result is IWebElement) return true;

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<string> GetTweetSource(List<string> tweetUrls)
        {
            var tweetSources = new List<string>();

            foreach (var nonRetweetedUrl in tweetUrls)
            {
                WebDriver.Navigate().GoToUrl(nonRetweetedUrl);
                Thread.Sleep(2000);

                var tweetSource = WebDriver.FindElement(By.XPath(Constants.tweetSourceXpath)).Text;

                tweetSources.Add(tweetSource);
            }

            return tweetSources;
        }
    }
}
