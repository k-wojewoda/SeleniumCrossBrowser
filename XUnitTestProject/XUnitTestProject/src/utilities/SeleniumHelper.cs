using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace XUnitTestProject.src.utilities
{
    public static class SeleniumHelper
    {
        public static void ScrollToElementAndClick(IWebDriver driver, IWebElement element, By mediaPackBy)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(mediaPackBy));
            element.Click();
        }
        public static void WaitForPageToLoad(IWebDriver driver, IWebElement elementOnNewPage, string textOnNewPage)
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 60));
            wait.Until(ExpectedConditions.TextToBePresentInElement(elementOnNewPage, textOnNewPage));
        }
        public static void HoverOver(IWebDriver driver, IWebElement element)
        {
            Actions actionBuilder = new Actions(driver);
            actionBuilder.MoveToElement(element).Build().Perform();
        }

        public static Boolean WaitForJSandJQueryToLoad(IWebDriver driver)
        {
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 60));            

                    wait.Until(driver =>
                    {
                        bool isAjaxFinished = (bool)((IJavaScriptExecutor)driver).
                            ExecuteScript("return jQuery.active == 0");
                                        
                        bool isJavascriptLoaded = (bool)((IJavaScriptExecutor)driver).
                            ExecuteScript("return document.readyState").Equals("complete");
                        return isAjaxFinished & isJavascriptLoaded;
                    });

                return false;      
        }
    }
}
