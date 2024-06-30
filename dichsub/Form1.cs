using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Selenium.Extensions;
using SeleniumExtras.WaitHelpers;
using SeleniumUndetectedChromeDriver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dichsub
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public List<string[]> groupedLines = new List<string[]>();
        private ChromeDriver driver;
        private int timeDelay = 70;
        private CancellationTokenSource cancellationTokenSource;

        private void button1_Click(object sender, EventArgs e)
        {
            Openchorme(driver);
        }

        public List<Cookie> setCookie(string cookie)
        {
            var list = new List<Cookie>();

            var cookies = cookie.Split(';').ToList();

            cookies.ForEach(x =>
            {
                var a = new Cookie
                {
                    key = x.Split('=')[0].Trim(),
                    value = x.Split('=')[1].Trim()
                };

                list.Add(a);
            });

            return list;
        }

        [STAThread]
        private static string GetClipboardText()
        {
            string result = string.Empty;
            Thread staThread = new Thread(
                delegate ()
                {
                    try
                    {
                        result = Clipboard.GetText();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error accessing clipboard: " + ex.Message);
                    }
                });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            return result;
        }

        public class Cookie
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Select a file";
            openFileDialog.Filter = "All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupedLines = new List<string[]>();
            if (File.Exists(textBox1.Text))
            {
                string[] lines = File.ReadAllLines(textBox1.Text);

                for (int i = 0; i < lines.Length; i += 200)
                {
                    string[] group = new string[Math.Min(200, lines.Length - i)];
                    Array.Copy(lines, i, group, 0, group.Length);
                    groupedLines.Add(group);
                }
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var options = new ChromeOptions();
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://www.perplexity.ai/");
        }

        private void Openchorme(ChromeDriver driver)
        {
            txtCookie.Text = "pplx.visitor-id=e362f9d2-7962-4032-8a13-050b521e3ecf; _ga=GA1.1.161429365.1717431814; intercom-device-id-l2wyozh0=055cd918-843d-479a-8e32-4aa727268f83; cf_clearance=Cm0yz6pQ73oTW_9yzDOv7FHUdQ1kKJXWR5dxKgaTgCM-1717946781-1.0.1.1-0_TkIQH3k50M2WxOe08ktgQVaTqGJ8ELQw.SV4.2mxeDTR0rDaHHAdnfr7wCCOqV.KOFnzJzmsujXcjlMkgi1Q; next-auth.csrf-token=b6363b01179feaa3d92ac8805a795a546ef60e9dc74d47ce9961336b9726cca9%7C390de0c76bce51709dc469a750fd157c8770b1aa19587337c8aaff4fb19ee6a0; __cflb=02DiuDyvFMmK5p9jVbVnMNSKYZhUL9aGkhE3968PYwzH2; next-auth.callback-url=https%3A%2F%2Fwww.perplexity.ai%2Fapi%2Fauth%2Fsignin-callback%3Fredirect%3Dhttps%253A%252F%252Fwww.perplexity.ai; intercom-session-l2wyozh0=b05QdllreHBta3dhMHRHUG1RekIwbUNLdTNrV3krTUg4M2tUOXo3L3RLeUJkdW5CWVdkZzA4RTdIc1dhdlEwZi0tMXU1cU1IR01hdXFjaDc1VldXZitXZz09--2522e7300d445c23464f6dc9cb1d98bdaf5875ad; AWSALB=CHF3JWpwmxfSyqTU7R7/dDIEBMuBxSVjD7XgJ+rk5VRapQzw9p9/eFLFG+WUX6Eh68zQwHJLQJLMzJKvxAkC90BhNg8Eqckeka+qqkQhT8fhgBh9lrioiIeqiazVaJCFXovNnyxLBQWHjvPizVu1sL5Rozz6QJI8W5lWCPdn6pZL7WDTqePldtpiaa0SKg==; AWSALBCORS=CHF3JWpwmxfSyqTU7R7/dDIEBMuBxSVjD7XgJ+rk5VRapQzw9p9/eFLFG+WUX6Eh68zQwHJLQJLMzJKvxAkC90BhNg8Eqckeka+qqkQhT8fhgBh9lrioiIeqiazVaJCFXovNnyxLBQWHjvPizVu1sL5Rozz6QJI8W5lWCPdn6pZL7WDTqePldtpiaa0SKg==; __Secure-next-auth.session-token=eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2R0NNIn0..hNbV9jwDzirWPZXu.zpwrCylh7mTIxRJse2f6Wv5aoFo6lLK5c7muZ4Q5WMqz3orumiKi-KU3JzLLbj3QuubkuKetxruIkW7jeeFKrTtzl-4sWZLUKawzw6s-DzOAY00PJHchjbErj-R1s9-c3fdsE6L0i-7QA5EtL8mkd709z9uNx5wf8dJuP5tGjX-UFm3tW9-rYNOfc8XXXG_LrwLu6x_IlEz2F9qxzmts5csLbpUn2gtJhWR-iWLe8NUpWa5XEXq2k-lIKGFrqHfjp7nbGoPZRqiV28CUJ0rxF_Pj-IZ32XkL01DDCis6mYPB17dF7Jw7JDB2z3Jpl2aGu-qZEM_iF99MPUMOMIvvQjCan8cmzeMBwTPubDZDg33ULsWVb2JA4Ro.Wnubrzh3EGfknYuTUfbnSQ; _ga_SH9PRBQG23=GS1.1.1717946797.11.1.1717947953.0.0.0";
            string cookie = txtCookie.Text;
            var cookies = setCookie(cookie);
            driver.Navigate().GoToUrl("https://www.perplexity.ai/");
            cookies.ForEach(x =>
            {
                driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(x.key, x.value));
            });
            Thread.Sleep(3000);

            driver.Navigate().GoToUrl("https://www.perplexity.ai/");
            SendKey(true);
        }

        private void SendKey(bool firstRun)
        {
            Thread.Sleep(3000);
            WebDriverWait wait1 = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            string itemElement = "//textarea[@placeholder='Ask anything...']";
            int cn = 0;
            bool first = true;
            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            if (firstRun == false)
            {
                first = false;
                itemElement = "//textarea[@placeholder='Ask follow-up']";
            }
            IWebElement element = wait1.Until(drv => drv.FindElement(By.XPath(itemElement)));
            Task.Run(() =>
            {
                foreach (var item in groupedLines)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    string sub = "dịch tiếp cho tôi";
                    if (cn > 0)
                    {
                        element = driver.FindElement(By.XPath("//textarea[@placeholder='Ask follow-up']"));
                    }
                    cn++;
                    if (first == false)
                    {
                        element.SendKeys(sub);
                        element.SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Enter);
                        element.SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Enter);
                    }
                    foreach (var it in item)
                    {
                        element.SendKeys(it);
                        element.SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Enter);
                    }
                    if (first == true)
                    {
                        first = false;
                        element.SendKeys("dịch file sau từ tiếng trung sang tiếng việt kể cả tên nhân vật và địa danh. giữ nguyên định dạng file, số dòng không được phép thay đổi. ngữ cảnh là một bộ phim. Nhớ dịch tên ra tiếng việt");
                    }
                    try
                    {
                        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(600));
                        IWebElement button = wait.Until(drv => drv.FindElement(By.XPath("(//button[@aria-label='Submit'])")));
                        button.Click();
                        Thread.Sleep(timeDelay * 1000);
                        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                        var buttonCopy = wait.Until(drv => drv.FindElements(By.CssSelector("svg[data-icon='clipboard']")));
                        buttonCopy[buttonCopy.Count - 1].Click();

                        System.Threading.Thread.Sleep(1000);
                        string copiedText = GetClipboardText();
                        string part = "";
                        if (chkP1.Checked)
                        {
                            part = "P1";
                        }
                        else
                        {
                            part = "P2";
                        }
                        string srtFilePath = $@"C:\Users\Administrator\Documents\Project\{DateTime.Now.ToString("yyyyMMdd")}+{part}.srt";
                        File.AppendAllText(srtFilePath, copiedText + Environment.NewLine + "\n");
                        System.Threading.Thread.Sleep(2000);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }, cancellationToken);
        }

        private int i = 0;
        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            i++;
            foreach (var line in groupedLines[i])
            {
                textBox2.Text += line + "\n";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int newDelay;
            if (int.TryParse(textBox3.Text, out newDelay))
            {
                timeDelay = newDelay;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string path = textBox4.Text;
            driver.Navigate().GoToUrl(path);
            SendKey(false);
        }
    }
}
