using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using XUnitTestProject.src.utilities;
using System.IO;

namespace XUnitTestProject
{
    public class UnitTest
    {
        String test_url = "https://www.medicalgorithmics.pl/";      

        [Theory]     
        [InlineData(BrowserType.Firefox)]
        [InlineData(BrowserType.Chrome)]

        public void Test_1(BrowserType browserType)
        {

            System.Diagnostics.Trace.Indent();
            using (var driver = WebDriverInfra.Create_Browser(browserType))
            {
                driver.Navigate().GoToUrl(test_url);
                driver.Manage().Window.Maximize();                

                //check if page is loaded (1)
                IWebElement wiecej = driver.FindElement(By.XPath("//div[contains(@id,'slide-3-layer-9')]"));
                SeleniumHelper.WaitForPageToLoad(driver, wiecej, "WIĘCEJ");

                //accept cookies
                IWebElement acceptCookies = driver.FindElement(By.XPath("//a[contains(@id,'accept-cookie')]"));
                acceptCookies.Click();

                //Check if Kontakt changing its color when hover
                IWebElement kontakt = driver.FindElement(By.XPath("//a[.='Kontakt']"));
                //check 'kontakt' color befor hover
                switch (browserType)
                {
                    case BrowserType.Chrome:
                        Assert.Equal("rgba(86, 86, 85, 1)", kontakt.GetCssValue("color"));
                        break;
                    case BrowserType.Firefox:
                        Assert.Equal("rgb(86, 86, 85)", kontakt.GetCssValue("color"));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(browserType), browserType, null);
                }
                
                //hover over Kontakt
                IWebElement menuKontakt = driver.FindElement(By.XPath("//nav[contains(@class,'main_menu')]//li[@id= 'mega-menu-item-29']"));                
                SeleniumHelper.HoverOver(driver, menuKontakt);
                //check 'kontakt' color after hover (checked only if it has changed)                
                switch (browserType)
                {
                    case BrowserType.Chrome:
                        Assert.NotEqual("rgba(86, 86, 85, 1)", kontakt.GetCssValue("color"));
                        break;
                    case BrowserType.Firefox:
                        Assert.NotEqual("rgb(86, 86, 85)", kontakt.GetCssValue("color"));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(browserType), browserType, null);
                }

                // Click on 'Kontakt'
                kontakt.Click();

                //wait for page to load
                SeleniumHelper.WaitForJSandJQueryToLoad(driver);

                //// Click on 'Media pack'
                By mediaPackBy = By.XPath("//h3/a[.='Media pack']");
                IWebElement mediaPack = driver.FindElement(mediaPackBy);
                SeleniumHelper.ScrollToElementAndClick(driver, mediaPack, mediaPackBy);

                //wait for page to load
                SeleniumHelper.WaitForJSandJQueryToLoad(driver);

                //// Click on 'Logotypy'
                IWebElement logotypy = driver.FindElement(By.XPath("//h1/a[contains(@href,'logotypy.zip')]"));
                logotypy.Click();

                //====================================================================================             
                String expectedFilePath = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Downloads\logotypy.zip");
                bool fileExists = false;
                          
                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(180));
                    wait.Until <bool> (x => fileExists = File.Exists(expectedFilePath));

                    System.Diagnostics.Trace.WriteLine("File exists : " + fileExists);

                    FileInfo fileInfo = new FileInfo(expectedFilePath);

                    System.Diagnostics.Trace.WriteLine("File Length : " + fileInfo.Length);
                    System.Diagnostics.Trace.WriteLine("File Name : " + fileInfo.Name);
                    System.Diagnostics.Trace.WriteLine("File Full Name :" + fileInfo.FullName);

                    Assert.Equal(3336484, fileInfo.Length);
                    Assert.Equal("logotypy.zip", fileInfo.Name);
                    Assert.Equal(expectedFilePath, fileInfo.FullName);

                }
                catch (Exception e)
                {                   
                    System.Diagnostics.Trace.WriteLine(e.Message);
                    Assert.True(false, "Test failed - "+ e.Message);
                }
                finally
                {
                    if (File.Exists(expectedFilePath))
                        File.Delete(expectedFilePath);
                }

                //========================================================================================
                driver.Quit();
            }
        }

        //------------------------------- TEST 2 -----------------------------------
        [Theory]
        [InlineData(BrowserType.Firefox)]
        [InlineData(BrowserType.Chrome)]

        public void Test_2(BrowserType browserType)
        {

            System.Diagnostics.Trace.Indent();
            using (var driver = WebDriverInfra.Create_Browser(browserType))
            {
                driver.Navigate().GoToUrl(test_url);
                driver.Manage().Window.Maximize();

                //check if page is loaded (1)
                IWebElement wiecej = driver.FindElement(By.XPath("//div[contains(@id,'slide-3-layer-9')]"));
                SeleniumHelper.WaitForPageToLoad(driver, wiecej, "WIĘCEJ");

                //check if page is loaded (2)
                SeleniumHelper.WaitForJSandJQueryToLoad(driver);

                //accept cookies
                IWebElement acceptCookies = driver.FindElement(By.XPath("//a[contains(@id,'accept-cookie')]"));
                acceptCookies.Click();
                //============================================================================================
                //click on search loop icon
                IWebElement searchLoopIcon = driver.FindElement(By.XPath("//span[contains(@class, 'icon_search')]"));
                searchLoopIcon.Click();

                //// Enter search phrase
                IWebElement textfield = driver.FindElement(By.XPath("//input[contains(@class, 'qode_search_field')]"));
                textfield.SendKeys("Pocket ECG CRS");
                textfield.SendKeys(Keys.Enter);
                //check if page is loaded
                SeleniumHelper.WaitForJSandJQueryToLoad(driver);

                //verifying that thre are 10 search results on 1st page (every search result has a post_date)                
                IList<IWebElement> resultsPostDates = driver.FindElements(By.XPath("//div[@class='post_date']")).ToList();
                Assert.Equal(10, resultsPostDates.Count);

                //verifying that thre are 2 search results pages (all pagination minus 'prev' and 'next' )
                IList<IWebElement> allPagination = driver.FindElements(By.XPath("//div[@class='pagination']/ul/li")).ToList();
                Assert.Equal(2, allPagination.Count-2);

                //verifying that phrase 'PocketECG CRS – telerehabilitacja kardiologiczna' exists only once in search results               
                //1st page
                IList<IWebElement> phrases = driver.FindElements(By.XPath("//a[contains(.,'PocketECG CRS – telerehabilitacja kardiologiczna')]")).ToList();
                int phrasesTotalAmount = phrases.Count;
                //2nd page  
                //click next page
                driver.FindElement(By.XPath("//li[@class='next']")).Click();
                //check if page is loaded
                SeleniumHelper.WaitForJSandJQueryToLoad(driver);
                phrases = driver.FindElements(By.XPath("//a[contains(.,'PocketECG CRS – telerehabilitacja kardiologiczna')]")).ToList();
                phrasesTotalAmount += phrases.Count;
                Assert.Equal(1, phrasesTotalAmount);

                //============================================================================================

                driver.Quit();
            }
        }

    }
}
