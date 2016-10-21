using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Collections.ObjectModel;

namespace autoChallenge7
{
    class crawler
    {
        static IWebDriver driver = new ChromeDriver("C://chromedriver//");
        static string skiurl = "https://www.skiutah.com/";
        StreamWriter writefile = new StreamWriter("F:\\links.txt");
        static Stack<string> linkstack = new Stack<string>();
        static Stack<string> verifystack = new Stack<string>();
        static List<IWebElement> elementlist = new List<IWebElement>();
        static ReadOnlyCollection<IWebElement> readelement = new ReadOnlyCollection<IWebElement>(elementlist);

        static void initial()
        {
            driver.Manage().Window.Maximize();
            driver.Url = skiurl;
        }

        void frontpagelink()
        {
            string line;
            readelement = driver.FindElements(By.TagName("a"));
            foreach (IWebElement elem in readelement)
            {
                string filter = elem.GetAttribute("href").ToString();

                if ((filter.StartsWith("https")) && (filter.Substring(12, 7) == "skiutah"))
                {
                    writefile.WriteLine(filter);
                }
            }
            writefile.Close();

            StreamReader readfile = new StreamReader("F:\\links.txt");
            while ((line = readfile.ReadLine()) != null)
            {
                linkstack.Push(line);
            }
            readfile.Close();

            StreamWriter writefile2 = new StreamWriter("F:\\links.txt");
            while (linkstack.Count != 0)
            {

                string link = linkstack.Pop().ToString();
                if (linkstack.Contains(link) == false) { writefile2.WriteLine(link); verifystack.Push(link); }
            }
            writefile2.Close();
        }

        void linkfollower()
        {
            string line;
            StreamReader readfile = new StreamReader("F:\\links.txt");
            while ((line = readfile.ReadLine()) != null)
            {
                Console.WriteLine(line);
                driver.Url = line;
            }
            readfile.Close();
        }

        void linkscanner(string parentlink)
        {
            Console.WriteLine(parentlink);
            driver.Url = parentlink;
            readelement = driver.FindElements(By.TagName("a"));
            StreamWriter appendfile = File.AppendText("F:\\links.txt");
            foreach (IWebElement elem in readelement)
            {
                string filter = elem.GetAttribute("href");

                if ((filter != null) && (filter.StartsWith("https")) && (filter.Substring(12, 7) == "skiutah") && (linkstack.Contains(filter) == false))
                {
                    appendfile.WriteLine(filter);
                    linkstack.Push(filter);
                    verifystack.Push(filter);
                }
            }
            appendfile.Close();
        }

        static void Main()
        {
            int line;
            crawler crawl = new crawler();
            initial();
            crawl.frontpagelink();

            while ((line = verifystack.Count) != 0)
            {
                crawl.linkscanner(verifystack.Pop());
            }

        }
    }
}
