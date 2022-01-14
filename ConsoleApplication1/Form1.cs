using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            initComBoBox();


            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(100, 100);
            this.Text = "Google地圖Html產生器";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void err(Exception ex)
        {
            label2.Text = ex.ToString();
        }

        private void initComBoBox()
        {
            try
            {
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox1.Items.Clear();
                for(int i = 0; i <= 21; i++)
                {
                    comboBox1.Items.Add(i);
                }
                comboBox1.SelectedIndex = 16;
            }
            catch (Exception ex)
            {
                err(ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter sw = null;
            try
            {
                string zoom = (comboBox1.SelectedIndex).ToString();
                string latlng = textBox1.Text;
                latlng = latlng.Replace(" ", "");
                var latlngArr = latlng.Split(',');
                if (latlngArr.Length != 2)
                {
                    throw new Exception("地圖中心經緯度錯誤");
                }
                string lat = latlngArr[0];
                string lng = latlngArr[1];

                string nL = "\r\n";
                StringBuilder sb = new StringBuilder();
                sb.Append("<!DOCTYPE html>" + nL);
                sb.Append("<html>" + nL);
                sb.Append("<body>" + nL);
                sb.Append("<div id=\"map\" style=\"width: 1300px; height: 600px; \"></div>" + nL);
                sb.Append("<script>" + nL);

                sb.Append("function myMap() {" + nL);

                sb.Append("var mapOptions = {" + nL);
                sb.Append("center: new google.maps.LatLng(" + lat + ", " + lng + ")," + nL);
                sb.Append("zoom: " + zoom + "," + nL);
                //sb.Append("fullscreenControl:false," + nL);

                if (checkBox3.Checked)
                {
                    sb.Append("disableDefaultUI:true," + nL);
                    sb.Append("draggable:false," + nL);
                    sb.Append("scrollwheel:false," + nL);
                }

                sb.Append("mapTypeId: google.maps.MapTypeId.roadmap}" + nL);
                sb.Append("var map = new google.maps.Map(document.getElementById(\"map\"), mapOptions);" + nL);

                if (checkBox4.Checked)
                {
                    sb.Append("var uluru = {lat: " + lat + ", lng: " + lng + "};" + nL);
                    if (checkBox1.Checked)
                    {
                        sb.Append("var marker = new google.maps.Marker({position: uluru, map: map,animation: google.maps.Animation.BOUNCE,draggable: true})" + nL);
                    }
                    else
                    {
                        sb.Append("var marker = new google.maps.Marker({position: uluru, map: map,draggable: true})" + nL);
                    }
                }
                
                int itCount = 0;
                foreach (var it in listBox1.Items)
                {
                    latlng = it.ToString();
                    latlng = latlng.Replace(" ", "");
                    latlngArr = latlng.Split(',');
                    if (latlngArr.Length == 2)
                    {
                        lat = latlngArr[0];
                        lng = latlngArr[1];
                        sb.Append("var uluru" + itCount.ToString() + " = {lat: " + lat + ", lng: " + lng + "};" + nL);

                        if (checkBox1.Checked)
                        {
                            sb.Append("var marker" + itCount.ToString() + " = new google.maps.Marker({position: uluru" + itCount.ToString() + ", map: map,animation: google.maps.Animation.BOUNCE,draggable: true})" + nL);
                        }
                        else
                        {
                            sb.Append("var marker" + itCount.ToString() + " = new google.maps.Marker({position: uluru" + itCount.ToString() + ", map: map,draggable: true})" + nL);
                        }
                        itCount++;
                    }
                }

                if (checkBox2.Checked && textBox3.Text != "")
                {
                    sb.Append("var infowindow = new google.maps.InfoWindow({" + nL);
                    sb.Append("content: \"" + textBox3.Text + "\"});" + nL);
                    sb.Append("infowindow.open(map,marker);" + nL);
                }
                

                sb.Append("}" + nL);
                sb.Append("</script>" + nL);
                sb.Append("<script src=\"https://maps.googleapis.com/maps/api/js?callback=myMap\"></script>" + nL);
                sb.Append("</body>" + nL);
                sb.Append("</html>" + nL);
                
                //取得目前程式位置
                string path = System.Environment.CurrentDirectory + "\\";
                string target = path + textBox4.Text + ".html";
                sw = new StreamWriter(target);
                sw.Write(sb.ToString());


                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }

                if (checkBox5.Checked)
                {
                    System.Diagnostics.Process.Start(target);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                err(ex);
            }
            finally
            {
                
            }

        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Add(textBox2.Text);
                textBox2.Text = "";
            }
            catch (Exception ex)
            {
                err(ex);
            }
        }
    }
}
