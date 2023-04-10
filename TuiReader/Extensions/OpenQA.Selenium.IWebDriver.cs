using OpenQA.Selenium.Support.UI;
using TuiReader;
using TuiReader.WebElements;

namespace OpenQA.Selenium;

/// <summary>
/// Extensions for <see cref="IWebDriver"/>.
/// </summary>
internal static class IWebDriverExtensions
{
	/// <summary>
	/// Goes to a page with all of the read notifications.
	/// </summary>
	public static void GoToOpenedNotificationPage(this IWebDriver driver) 
		=> driver.Navigate().GoToUrl(@"https://www.jil.travel/jilhpp/messenger/read/page/0/sm/1/generalproductid/32185");
	/// <summary>
	/// Creates a <see cref="WebDriverWait"/> with a default wait time of 5 second.
	/// </summary>
	public static WebDriverWait Wait(this IWebDriver driver, TimeSpan? time = null) => new (driver, time ?? TimeSpan.FromSeconds(5));
	
	/// <summary>
	/// Goes to JIL's login page.
	/// </summary>
	public static void GoToLoginPage(this IWebDriver webDriver) 
		=> webDriver.Navigate().GoToUrl(@"https://www.jil.travel/");
	/// <summary>
	/// Fills the email and password input fields.
	/// </summary>
	public static void FillCredentials(this IWebDriver webDriver, LoginContext context)
	{
		webDriver.GetEmailInputField().SendKeys(context.Email);
		webDriver.GetPasswordInputField().SendKeys(context.Password);
	}
	/// <summary>
	/// Gets the email input field on the login page.
	/// </summary>
	/// <returns>Email input field.</returns>
	/// <exception cref="Exception">Email input field could not be found.</exception>
	private static IWebElement GetEmailInputField(this IWebDriver webDriver)
		=> webDriver.FindElement(By.ClassName(@"jilcode")) ?? throw new Exception("Email input field could not be found.");
	/// <summary>
	/// Gets the password input field on the login page.
	/// </summary>
	/// <returns>Password input field.</returns>
	/// <exception cref="Exception">Password input field could not be found.</exception>
	private static IWebElement GetPasswordInputField(this IWebDriver webDriver)
		=> webDriver.FindElement(By.ClassName(@"password")) ?? throw new Exception("Password input field could not be found.");
	
	/// <summary>
	/// Clears the email and password input field.
	/// </summary>
	public static void ClearCredentials(this IWebDriver webDriver)
	{
		webDriver.GetEmailInputField().Clear();
		webDriver.GetPasswordInputField().Clear();
	}
	
	/// <summary>
	/// Checks if the webDriver is currently at the home page.
	/// </summary>
	public static bool IsOnHomePage(this IWebDriver webDriver) 
		=> webDriver.Title.Equals(@"JIL Hotel Partner Platform");
	
	/// <summary>
	/// Gets the login button 
	/// </summary>
	/// <returns>Login button.</returns>
	/// <exception cref="Exception"></exception>
	public static IWebElement GetLoginButton(this IWebDriver webDriver)
		=> webDriver.FindElement(By.Id(@"loginButton")) ?? throw new Exception("Login button could not be found.");
	/// <summary>
	/// Gets all <see cref="LinkElement"/> on the page.
	/// </summary>
	public static IEnumerable<LinkElement> GetLinkElements(this IWebDriver webDriver)
		=> (webDriver.FindElements(By.TagName("a")) ?? throw new Exception("There were no links on the page."))
			.Select(element => new LinkElement{ Element = element});

	/// <summary>
	/// Gets every "pre" element from the page.
	/// </summary>
	/// <returns>A list of "pre: elements.</returns>
	public static PreElement GetPreElement(this IWebDriver webDriver)
		=> new() { Element = webDriver.FindElement(By.TagName("pre")) ?? throw new Exception("No pre element could be found on this page") };

	/// <summary>
	/// Gets every "tr" element from the page.
	/// </summary>
	/// <returns>A list of "tr" elements.</returns>
	public static IEnumerable<RowElement> GetTrElements(this IWebDriver webDriver)
		=> (webDriver.FindElements(By.TagName("tr")) ?? throw new Exception("No 'tr' element found on the page"))
			.Select(element => new RowElement { Element = element });

}