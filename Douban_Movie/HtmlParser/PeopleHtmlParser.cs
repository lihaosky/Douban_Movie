using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Microsoft.Phone.Shell;
using System.Windows;
using PanoramaApp2.Resources;

namespace PanoramaApp2.HtmlParser
{
    class PeopleHtmlParser
    {
        private People people;
        public ProgressBar peopleProgressBar { get; set; }
        public Grid peopleGrid { get; set; }
        public StackPanel genderPanel { get; set; }
        public StackPanel constPanel { get; set; }
        public StackPanel birthPanel { get; set; }
        public StackPanel birthplacePanel { get; set; }
        public StackPanel occupationPanel { get; set; }
        private WebClient client = null;

        public PeopleHtmlParser(People p)
        {
            people = p;
        }

        public void parsePeople()
        {
            People p = Cache.getPeople(people.id);
            if (p == null)
            {
                client = new WebClient();
                client.DownloadStringCompleted += downloadPeopleCompleted;
                client.DownloadStringAsync(new Uri(People.peopleLinkHeader + people.id));
            }
            else
            {
                peopleGrid.DataContext = people;
                if (people.gender != "")
                {
                    genderPanel.Visibility = Visibility.Visible;
                }
                if (people.birthday != "")
                {
                    birthPanel.Visibility = Visibility.Visible;
                }
                if (people.birthplace != "")
                {
                    birthplacePanel.Visibility = Visibility.Visible;
                }
                if (people.constl != "")
                {
                    constPanel.Visibility = Visibility.Visible;
                }
                if (people.occupation != "")
                {
                    occupationPanel.Visibility = Visibility.Visible;
                }
                if (peopleProgressBar != null)
                {
                    peopleProgressBar.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void downloadPeopleCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled)
                {
                    string page = e.Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);
                    try
                    {
                        getPeople(doc);
                        peopleGrid.DataContext = people;
                        if (people.gender != "")
                        {
                            genderPanel.Visibility = Visibility.Visible;
                        }
                        if (people.birthday != "")
                        {
                            birthPanel.Visibility = Visibility.Visible;
                        }
                        if (people.birthplace != "")
                        {
                            birthplacePanel.Visibility = Visibility.Visible;
                        }
                        if (people.constl != "")
                        {
                            constPanel.Visibility = Visibility.Visible;
                        }
                        if (people.occupation != "")
                        {
                            occupationPanel.Visibility = Visibility.Visible;
                        }
                        Cache.insertPeople(people);
                    }
                    catch (Exception)
                    {
                    }
                    if (peopleProgressBar != null)
                    {
                        peopleProgressBar.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    var wEx = e.Error as WebException;
                    if (wEx.Status == WebExceptionStatus.RequestCanceled)
                    {
                        if (App.isFromDormant)
                        {
                            App.isFromDormant = false;
                            parsePeople();
                        }
                    }
                    else
                    {
                        if (peopleProgressBar != null)
                        {
                            peopleProgressBar.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
            catch (WebException)
            {
                if (peopleProgressBar != null)
                {
                    peopleProgressBar.Visibility = Visibility.Collapsed;
                }
                MessageBoxResult result = MessageBox.Show(AppResources.ConnectionError, "", MessageBoxButton.OK);
            }
        }

        private void getPeople(HtmlDocument doc)
        {
            string gender = "";
            string birthday = "";
            string constl = "";
            string birthplace = "";
            string occupation = "";
            string summary = "";

            try
            {
                //name = doc.DocumentNode.SelectNodes("//div[@id='content']")[0].SelectNodes("h1")[0].InnerText.Trim();
                HtmlNodeCollection ulNode = doc.DocumentNode.SelectNodes("//div[@id='headline']")[0].SelectNodes("div[@class='info']")[0].SelectNodes("ul");
                if (ulNode != null)
                {
                    HtmlNodeCollection liNodes = ulNode[0].SelectNodes("li");
                    if (liNodes != null)
                    {
                        foreach (HtmlNode liNode in liNodes)
                        {
                            HtmlNode spanNode = liNode.SelectNodes("span")[0];
                            string attr = spanNode.InnerText.Trim();
                            spanNode.Remove();
                            if (attr == "性别")
                            {
                                gender = Util.formatShortReview(liNode.InnerText.Trim().Substring(1, liNode.InnerText.Trim().Length - 1));
                            }
                            if (attr == "星座")
                            {
                                constl = Util.formatShortReview(liNode.InnerText.Trim().Substring(1, liNode.InnerText.Trim().Length - 1));
                            }
                            if (attr == "出生日期")
                            {
                                birthday = Util.formatShortReview(liNode.InnerText.Trim().Substring(1, liNode.InnerText.Trim().Length - 1));
                            }
                            if (attr == "出生地")
                            {
                                birthplace = Util.formatShortReview(liNode.InnerText.Trim().Substring(1, liNode.InnerText.Trim().Length - 1));
                            }
                            if (attr == "职业")
                            {
                                occupation = Util.formatShortReview(liNode.InnerText.Trim().Substring(1, liNode.InnerText.Trim().Length - 1));
                            }
                        }
                    }
                }
                HtmlNodeCollection introNodes = doc.DocumentNode.SelectNodes("//div[@id='intro']")[0].SelectNodes("div[@class='bd']")[0].SelectNodes("span[@class='all hidden']");
                if (introNodes != null)
                {
                    summary = Util.formatReview(introNodes[0].InnerHtml);
                }
                else
                {
                    summary = Util.formatReview(doc.DocumentNode.SelectNodes("//div[@id='intro']")[0].SelectNodes("div[@class='bd']")[0].InnerText);
                }

            }
            catch (Exception)
            {
                throw;
            }
            people.gender = gender;
            people.occupation = occupation;
            people.birthplace = birthplace;
            people.birthday = birthday;
            people.constl = constl;
            people.summary = summary;
        }

        public void cancelDownload()
        {
            if (client != null)
            {
                client.CancelAsync();
            }
        }
    }
}
