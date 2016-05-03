using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml;

namespace XMLWeather
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = "Today";
       
            // get information about current and forecast weather from the internet
            GetData();

            // take info from the current weather file and display it to the screen
            ExtractCurrent();

            // take info from the forecast weather file and display it to the screen
            ExtractForecast();
        }

        private static void GetData()
        {
            WebClient client = new WebClient();

            string currentFile = "http://api.openweathermap.org/data/2.5/weather?q=Stratford,CA&mode=xml&units=metric&appid=3f2e224b815c0ed45524322e145149f0";
            string forecastFile = "http://api.openweathermap.org/data/2.5/forecast/daily?q=Stratford,CA&mode=xml&units=metric&cnt=7&appid=3f2e224b815c0ed45524322e145149f0";
            
            client.DownloadFile(currentFile, "WeatherData.xml");
            client.DownloadFile(forecastFile, "WeatherData7Day.xml");
        }

        private void ExtractCurrent()
        {


            XmlDocument doc = new XmlDocument();
            doc.Load("WeatherData.xml");

            //create a node variable to represent the parent element
            XmlNode parent;
            parent = doc.DocumentElement;

            //check each child of the parent element
            foreach (XmlNode child in parent.ChildNodes)
            {
                // TODO if the "city" element is found display the value of it's "name" attribute
                if (child.Name == "city")
                {
                    cityOutput.Text = child.Attributes["name"].Value;
                }

                if (child.Name == "temperature")
                {
                    currentTempOut.Text = child.Attributes["value"].Value;
                }

                if (child.Name == "wind")
                {
                    foreach (XmlNode grandChild in child.ChildNodes)
                    {
                        if (grandChild.Name == "speed")
                        {
                            windDescOut.Text = grandChild.Attributes["name"].Value;
                        }
                    }
                }

                if (child.Name == "precipitation")
                {

                    precLabel.Text = child.Attributes["mode"].Value;
                    if (precLabel.Text == "no")
                    {
                        this.BackgroundImage = Properties.Resources.sunnyBackround;
                    }
                }
                
            }
            
        }

        private void ExtractForecast()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("WeatherData7Day.xml");

            //create a node variable to represent the parent element
            XmlNode parent;
            parent = doc.DocumentElement;

            int day = 1;

            //check each child of the parent element
            foreach (XmlNode child in parent.ChildNodes)
            {
                if (child.Name == "forecast")
                {
                    foreach (XmlNode grandChild in child.ChildNodes)
                    {
                        foreach (XmlNode greatGrandChild in grandChild.ChildNodes)
                        {
                            if (greatGrandChild.Name == "windSpeed")
                            {
                                switch (day)
                                {
                                    case 1:
                                        windDescOut.Text = greatGrandChild.Attributes["name"].Value;
                                       
                                        break;
                                    case 2:
                                        windDescOut2.Text = greatGrandChild.Attributes["name"].Value;
                                        
                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (greatGrandChild.Name == "temperature")
                            {
                                switch (day)
                                {
                                    case 1:
                                        minOutput.Text = greatGrandChild.Attributes["min"].Value;
                                        maxOutput.Text = greatGrandChild.Attributes["max"].Value; 
                                                                
                                        break;
                                    case 2:
                                        tommorowTemp.Text = greatGrandChild.Attributes["day"].Value;
                                        min2Output.Text = greatGrandChild.Attributes["min"].Value;
                                        max2Output.Text = greatGrandChild.Attributes["max"].Value;
                                        break;
                                    default:
                                        break;
                                }
                            }         
                            if (greatGrandChild.Name == "clouds")
                            {
                                switch (day)
                                {
                                    case 1:
                                        day1Clouds.Text = greatGrandChild.Attributes["value"].Value;
                                        day++;
                                        break;
                                    case 2:
                                        day2Clouds.Text = greatGrandChild.Attributes["value"].Value;
                                        day++;
                                        break;
                                    default:
                                        break;
                                }
                            } 
                        }
                    }
                }
            }
        }

        private void MakePictureParent()
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

     
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Today")
            {
                tommorowTemp.Hide();
                windDescOut2.Hide();
                min2Output.Hide();
                max2Output.Hide();
                day2Clouds.Hide();
                maxOutput.Show();
                minOutput.Show();
                day1Clouds.Show();
                currentTempOut.Show();
                windDescOut.Show();
            }
            
            if (comboBox1.Text == "Tommorow")
            {
                tommorowTemp.Show();
                min2Output.Show();
                max2Output.Show();
                day2Clouds.Show();
                day1Clouds.Hide();
                currentTempOut.Hide();
                windDescOut.Hide();
                minOutput.Hide();
                maxOutput.Hide();
                windDescOut2.Show();
            }
        }
    }
}
