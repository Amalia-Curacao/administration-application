using OpenQA.Selenium;
using TUI_Reader.Drivers;
using TUI_Reader.Extensions;

namespace TUI_Reader.Actions;

/// <summary>
/// Is used login to jil.travel.
/// </summary>
internal class Login
{
    /// <summary>
    /// <inheritdoc cref="LoginContext"/>
    /// </summary>
    public LoginContext LoginContext { get; init; } = new();
    /// <summary>
    /// Will attempt to login to the jil website.
    /// </summary>
    /// <exception cref="Exception">Failed login attempt.</exception>
    public void Run(Driver driver, bool logging = true)
    {
        if(logging) Console.WriteLine($"{driver.Id}: login start");
        
        var successfulLogin = AttemptLogin(driver, LoginContext.Email, LoginContext.Password);
        
        if (!successfulLogin) 
            throw new Exception($"{driver.Id}: login failed");
        else
            if(logging) Console.WriteLine($"{driver.Id}: login successful");
    }
    /// <summary>
    /// Gets the email input field on the login page.
    /// </summary>
    /// <returns>Email input field.</returns>
    /// <exception cref="Exception">Email input field could not be found.</exception>
    private static IWebElement GetEmailInputField(IWebDriver driver)
    => driver.FindElement(By.ClassName(@"jilcode")) ?? throw new Exception("Email input field could not be found.");
    /// <summary>
    /// Gets the password input field on the login page.
    /// </summary>
    /// <returns>Password input field.</returns>
    /// <exception cref="Exception">Password input field could not be found.</exception>
    private static IWebElement GetPasswordInputField(IWebDriver driver)
    => driver.FindElement(By.ClassName(@"password")) ?? throw new Exception("Password input field could not be found.");
    
    /// <summary>
    /// Gets the login button 
    /// </summary>
    /// <returns>Login button.</returns>
    /// <exception cref="Exception"></exception>
    private static IWebElement GetLoginButton(IWebDriver driver)
    => driver.FindElement(By.Id(@"loginButton")) ?? throw new Exception("Login button could not be found.");

    /// <summary>
    /// Tries to login.
    /// </summary>
    /// <returns>One login attempt was successfully.</returns>
    private static bool AttemptLogin(Driver driver, string email, string password)
    {
        var webDriver = driver.WebDriver;
        GoToLoginPage(webDriver);
        // Will attempt to login 20 times before giving up. This is added because TUI has a shit login system.
        for (var i = 0; i < 20; i++)
        {
            ClearCredentials(webDriver);
            FillCredentials(webDriver, email, password);
            GetLoginButton(webDriver).Click();
            try
            {
                return driver.Wait().Until(IsOnHomePage);
            }
            catch (Exception) { /*ignored*/ }
        }
        throw new Exception("Email address and password combination is incorrect");
    }
    /// <summary>
    /// Checks if the driver is currently at the home page.
    /// </summary>
    private static bool IsOnHomePage(IWebDriver webDriver) 
        => webDriver.Title.Equals(@"JIL Hotel Partner Platform");
    
    /// <summary>
    /// Goes to JIL's login page.
    /// </summary>
    private static void GoToLoginPage(IWebDriver webDriver) 
        => webDriver.Navigate().GoToUrl(@"https://www.jil.travel/");
    /// <summary>
    /// Fills the email and password input fields.
    /// </summary>
    private static void FillCredentials(IWebDriver webDriver, string email, string password)
    {
        GetEmailInputField(webDriver).SendKeys(email);
        GetPasswordInputField(webDriver).SendKeys(password);
    }
    /// <summary>
    /// Clears the email and password input field.
    /// </summary>
    private static void ClearCredentials(IWebDriver webDriver)
    {
        GetEmailInputField(webDriver).Clear();
        GetPasswordInputField(webDriver).Clear();
    }

}