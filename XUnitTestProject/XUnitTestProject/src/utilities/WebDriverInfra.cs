using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace XUnitTestProject.src.utilities
{
    internal static class WebDriverInfra
    {
        public static IWebDriver Create_Browser(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Chrome:
                    return new ChromeDriver();
                case BrowserType.Firefox:
                    FirefoxOptions options = new FirefoxOptions();
                    options.SetPreference("browser.download.folderList", 2);
                    options.SetPreference("browser.download.dir", @"%USERPROFILE%\Downloads\");
                    options.SetPreference("browser.download.useDownloadDir", true);
                    options.SetPreference("browser.download.viewableInternally.enabledTypes", "");
                    options.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/zip");

                    return new FirefoxDriver(options);
                default:
                    throw new ArgumentOutOfRangeException(nameof(browserType), browserType, null);
            }
        }
    }
}
