using System.Text.Json;
using OpenQA.Selenium;
using TUI_Reader.Drivers;
using TUI_Reader.Extensions;
using TUI_Reader.Properties;
using DriverOptions = TUI_Reader.Contracts.DriverOptions;

namespace TUI_Reader.Actions;

internal static class Login
{
    /// <summary>
    /// Will attempt to login to the jil website.
    /// </summary>
    /// <remarks>
    /// To use different credentials than the appsettings.json credentials use <see cref="Run(TUI_Reader.Drivers.Driver,int)"/> instead.
    /// </remarks>
    /// <exception cref="Exception">Failed login attempt.</exception>
    public static void Run(Driver driver, int maxLoginAttempt = int.MaxValue)
    {
        var appSettings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(@"./Properties/appsettings.json"))!;
        Login.Run(driver, appSettings.Email, appSettings.Password, maxLoginAttempt);
    }
    /// <summary>
    /// Will attempt to login to the jil website.
    /// </summary>
    /// <remarks>
    /// To use the credentials in the appsettings.json use <see cref="Run(TUI_Reader.Drivers.Driver,int)"/> instead.
    /// </remarks>
    /// <exception cref="Exception">Failed login attempt.</exception>
    public static void Run(Driver driver, string email, string password, int maxLoginAttempt = int.MaxValue)
    {
        Console.WriteLine($"{driver.Id}: login start");
        
        var successfulLogin = driver.AttemptLogin(email, password, maxLoginAttempt);
        
        if (!successfulLogin) 
            throw new Exception($"{driver.Id}: login failed");
        else
            Console.WriteLine($"{driver.Id}: login successful");
    }
    /// <summary>
    /// Gets the email input field on the login page.
    /// </summary>
    /// <returns>Email input field.</returns>
    /// <exception cref="Exception">Email input field could not be found.</exception>
    private static IWebElement GetEmailInputField(this IWebDriver driver)
    => driver.FindElement(By.ClassName(@"jilcode")) ?? throw new Exception("Email input field could not be found.");
    /// <summary>
    /// Gets the password input field on the login page.
    /// </summary>
    /// <returns>Password input field.</returns>
    /// <exception cref="Exception">Password input field could not be found.</exception>
    private static IWebElement GetPasswordInputField(this IWebDriver driver)
    => driver.FindElement(By.ClassName(@"password")) ?? throw new Exception("Password input field could not be found.");
    
    /// <summary>
    /// Gets the login button 
    /// </summary>
    /// <returns>Login button.</returns>
    /// <exception cref="Exception"></exception>
    private static IWebElement GetLoginButton(this IWebDriver driver)
    => driver.FindElement(By.Id(@"loginButton")) ?? throw new Exception("Login button could not be found.");

    /// <summary>
    /// Tries to login.
    /// </summary>
    /// <returns>One login attempt was successfully.</returns>
    private static bool AttemptLogin(this Driver driver, string email, string password, int maxLoginAttempts)
    {
        var webDriver = driver.WebDriver;
        webDriver.GoToLoginPage();
        for (var i = 0; i < maxLoginAttempts; i++)
        {
            webDriver.ClearCredentials();
            webDriver.FillCredentials(email, password);
            webDriver.GetLoginButton().Click();
            try
            {
                return driver.Wait().Until(wd => wd.IsOnHomePage());
            }
            catch (Exception) { /*ignored*/ }
        }
        return false;
    }
    /// <summary>
    /// Checks if the driver is currently at the home page.
    /// </summary>
    private static bool IsOnHomePage(this IWebDriver webDriver) 
        => webDriver.Title.Equals(@"JIL Hotel Partner Platform");
    
    /// <summary>
    /// Goes to JIL's login page.
    /// </summary>
    private static void GoToLoginPage(this IWebDriver webDriver) 
        => webDriver.Navigate().GoToUrl(@"https://www.jil.travel/");
    /// <summary>
    /// Fills the email and password input fields.
    /// </summary>
    private static void FillCredentials(this IWebDriver webDriver, string email, string password)
    {
        webDriver.GetEmailInputField().SendKeys(email);
        webDriver.GetPasswordInputField().SendKeys(password);
    }
    /// <summary>
    /// Clears the email and password input field.
    /// </summary>
    private static void ClearCredentials(this IWebDriver webDriver)
    {
        webDriver.GetEmailInputField().Clear();
        webDriver.GetPasswordInputField().Clear();
    }

}