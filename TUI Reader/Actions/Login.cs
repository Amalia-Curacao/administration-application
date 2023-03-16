using OpenQA.Selenium;
using TUI_Reader.Extensions;

namespace TUI_Reader.Actions;

internal sealed class Login
{
    private readonly string     _email;
    private readonly string     _password;
    private readonly IWebDriver _driver;
    public Login(string email, string password, IWebDriver driver)
    {
        _email = email;
        _password = password;
        _driver = driver;
    }
    /// <summary>
    /// Will attempt to login to the jil website.
    /// </summary>
    /// <param name="maxLoginAttempt">The maximum amount of login attempts.</param>
    /// <exception cref="Exception">Failed login attempt.</exception>
    public void Run(int maxLoginAttempt = 1)
    {
        Console.WriteLine("Login: start");
        
        var successfulLogin = AttemptLogin(maxLoginAttempt);
        
        if (!successfulLogin) 
            throw new Exception("Login: failed");
        else
            Console.WriteLine("Login: successful");
    }
    /// <summary>
    /// Gets the email input field on the login page.
    /// </summary>
    /// <returns>Email input field.</returns>
    /// <exception cref="Exception">Email input field could not be found.</exception>
    private IWebElement GetEmailInputField()
    => _driver.FindElement(By.ClassName(@"jilcode")) ?? throw new Exception("Email input field could not be found.");
    /// <summary>
    /// Gets the password input field on the login page.
    /// </summary>
    /// <returns>Password input field.</returns>
    /// <exception cref="Exception">Password input field could not be found.</exception>
    private IWebElement GetPasswordInputField()
    => _driver.FindElement(By.ClassName(@"password")) ?? throw new Exception("Password input field could not be found.");
    
    /// <summary>
    /// Gets the login button 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private IWebElement GetLoginButton()
    => _driver.FindElement(By.Id(@"loginButton")) ?? throw new Exception("Login button could not be found.");

    /// <summary>
    /// Tries to login.
    /// </summary>
    /// <param name="maxLoginAttempts">The amount of times it will attempt to login.</param>
    /// <returns>One login attempt was successfully.</returns>
    private bool AttemptLogin(int maxLoginAttempts)
    {
        _driver.GoToLoginPage();
        for (var i = 0; i < maxLoginAttempts; i++)
        {
            ClearCredentials();
            FillCredentials();
            GetLoginButton().Click();
            if(_driver.Wait().Until(driver => driver.IsOnHomePage())) return true;
        }
        return false;
    }
    /// <summary>
    /// Fills the email and password input fields.
    /// </summary>
    private void FillCredentials()
    {
        GetEmailInputField().SendKeys(_email);
        GetPasswordInputField().SendKeys(_password);
    }
    /// <summary>
    /// Clears the email and password input field.
    /// </summary>
    private void ClearCredentials()
    {
        GetEmailInputField().Clear();
        GetPasswordInputField().Clear();
    }

}