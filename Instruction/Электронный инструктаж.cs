using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Instruction
{
    public partial class Form1 : Form
    {
        string constring = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Tim\Documents\Visual Studio 2013\Projects\Instruction\Instruction\InstructionsDB.mdf;Integrated Security=True";
        DataSet dataset;
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 24; i++)
                dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void btnMakeInstruction_Click(object sender, EventArgs e)
        {
            string dateTime = DateTime.Now.ToString("dd.MM.yy - HH:mm:ss");
            using (SqlConnection connection = new SqlConnection(constring))
            {
                SqlCommand cmd = new SqlCommand("insert into instructions (data_vydachi_zayavki, dolzhnost_vidav_zayavku, rukovod_rabot, teh_karta, data_nomer_prikaza, fio_signal_1, fio_signal_2, fio_signal_3, fio_signal_4, mesto_km_1, mesto_pk_1, mesto_put_1, mesto_km_2, mesto_pk_2, plan_data_rabot, po_soglasovaniyou, date1, data_c_chasov_1, data_min_1, date2, data_c_chasov_2, data_min_2, fio_dispetchera, fio_instructirushego, time_record) values (@data_vydachi_zayavki, @dolzhnost_vidav_zayavku, @rukovod_rabot, @teh_karta, @data_nomer_prikaza, @fio_signal_1, @fio_signal_2, @fio_signal_3, @fio_signal_4, @mesto_km_1, @mesto_pk_1, @mesto_put_1, @mesto_km_2, @mesto_pk_2, @plan_data_rabot, @po_soglasovaniyou, @date1, @data_c_chasov_1, @data_min_1, @date2, @data_c_chasov_2, @data_min_2, @fio_dispetchera, @fio_instructirushego, @time_record)");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@data_vydachi_zayavki", dtp_data_vydachi_zayavki.Value.ToShortDateString().Trim());
                cmd.Parameters.AddWithValue("@dolzhnost_vidav_zayavku", cb_dolzhnost_vidav_zayavku.SelectedItem.ToString().Trim());
                cmd.Parameters.AddWithValue("@rukovod_rabot", cb_rukovod_rabot.SelectedItem.ToString().Trim());
                cmd.Parameters.AddWithValue("@teh_karta", cb_teh_karta.SelectedItem.ToString().Trim());
                cmd.Parameters.AddWithValue("@data_nomer_prikaza", cb_data_nomer_prikaza.SelectedItem.ToString().Trim());
                cmd.Parameters.AddWithValue("@fio_signal_1", cm_fio_signal_1.SelectedItem.ToString().Trim());
                cmd.Parameters.AddWithValue("@fio_signal_2", cm_fio_signal_2.SelectedItem.ToString().Trim());
                cmd.Parameters.AddWithValue("@fio_signal_3", cm_fio_signal_3.SelectedItem.ToString().Trim());
                cmd.Parameters.AddWithValue("@fio_signal_4", cm_fio_signal_4.SelectedItem.ToString().Trim());
                cmd.Parameters.AddWithValue("@mesto_km_1", tb_mesto_km_1.Text.Trim());
                cmd.Parameters.AddWithValue("@mesto_pk_1", tb_mesto_pk_1.Text.Trim());
                cmd.Parameters.AddWithValue("@mesto_put_1", tb_mesto_put_1.Text.Trim());
                cmd.Parameters.AddWithValue("@mesto_km_2", tb_mesto_km_2.Text.Trim());
                cmd.Parameters.AddWithValue("@mesto_pk_2", tb_mesto_pk_2.Text.Trim());
                cmd.Parameters.AddWithValue("@plan_data_rabot", dtp_plan_data_rabot.Value.ToShortDateString().Trim());
                cmd.Parameters.AddWithValue("@po_soglasovaniyou", cb_po_soglasovaniyou.SelectedItem.ToString().Trim());
                cmd.Parameters.AddWithValue("@date1", dtm_date1.Value.ToShortDateString().Trim());
                cmd.Parameters.AddWithValue("@data_c_chasov_1", tb_data_c_chasov_1.Text.Trim());
                cmd.Parameters.AddWithValue("@data_min_1", tb_data_min_1.Text.Trim());
                cmd.Parameters.AddWithValue("@date2", dtm_date2.Value.ToShortDateString().Trim());
                cmd.Parameters.AddWithValue("@data_c_chasov_2", tb_data_c_chasov_2.Text.Trim());
                cmd.Parameters.AddWithValue("@data_min_2", tb_data_min_2.Text.Trim());
                cmd.Parameters.AddWithValue("@fio_dispetchera", cb_fio_dispetchera.SelectedItem.ToString().Trim());
                cmd.Parameters.AddWithValue("@fio_instructirushego", cb_fio_instructirushego.SelectedItem.ToString().Trim());
                cmd.Parameters.AddWithValue("@time_record", dateTime.Trim());
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                RefreshDataGridView();
                SendDataToServer();
                MessageBox.Show("Инструктаж успешно сформирован!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "instructionsDBDataSet.instructions". При необходимости она может быть перемещена или удалена.
            this.instructionsTableAdapter.Fill(this.instructionsDBDataSet.instructions);
        }

        public void RefreshDataGridView() // method for refreshing dataGridView1
        {
            SqlConnection sqlcon = new SqlConnection(constring);
            SqlDataAdapter sqladap = new SqlDataAdapter("select * from instructions", sqlcon);
            dataset = new DataSet();
            sqladap.Fill(dataset, "instructions");
            dataGridView1.DataSource = dataset.Tables["instructions"].DefaultView;
        }

        public void SendDataToServer()
        {
            ArrayList arrayInstruction = new ArrayList();
            DataTable dtable = dataset.Tables["instructions"];

            for (int i = 0; i < dtable.Rows.Count; i++) // заполняем коллекцию данными с бд
            {
                DataRow drow = dtable.Rows[i];
                arrayInstruction.Add(drow["data_vydachi_zayavki"].ToString().Trim());
                arrayInstruction.Add(drow["dolzhnost_vidav_zayavku"].ToString().Trim());
                arrayInstruction.Add(drow["rukovod_rabot"].ToString().Trim());
                arrayInstruction.Add(drow["teh_karta"].ToString().Trim());
                arrayInstruction.Add(drow["data_nomer_prikaza"].ToString().Trim());
                arrayInstruction.Add(drow["fio_signal_1"].ToString().Trim());
                arrayInstruction.Add(drow["fio_signal_2"].ToString().Trim());
                arrayInstruction.Add(drow["fio_signal_3"].ToString().Trim());
                arrayInstruction.Add(drow["fio_signal_4"].ToString().Trim());
                arrayInstruction.Add(drow["mesto_km_1"].ToString().Trim());
                arrayInstruction.Add(drow["mesto_pk_1"].ToString().Trim());
                arrayInstruction.Add(drow["mesto_put_1"].ToString().Trim());
                arrayInstruction.Add(drow["mesto_km_2"].ToString().Trim());
                arrayInstruction.Add(drow["mesto_pk_2"].ToString().Trim());
                arrayInstruction.Add(drow["plan_data_rabot"].ToString().Trim());
                arrayInstruction.Add(drow["po_soglasovaniyou"].ToString().Trim());
                arrayInstruction.Add(drow["date1"].ToString().Trim());
                arrayInstruction.Add(drow["data_c_chasov_1"].ToString().Trim());
                arrayInstruction.Add(drow["data_min_1"].ToString().Trim());
                arrayInstruction.Add(drow["date2"].ToString().Trim());
                arrayInstruction.Add(drow["data_c_chasov_2"].ToString().Trim());
                arrayInstruction.Add(drow["data_min_2"].ToString().Trim());
                arrayInstruction.Add(drow["fio_dispetchera"].ToString().Trim());
                arrayInstruction.Add(drow["fio_instructirushego"].ToString().Trim());
                arrayInstruction.Add(drow["time_record"].ToString().Trim());
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://stockcom.ru/accept");
            request.Method = "POST";
            string sendIdToServer = "";
            for (int s = 0; s < arrayInstruction.Count; s++)
            {
                // string sendIdToServer = s + "=" + arrayInstruction.ge +
                //   "&FormValue2=" + someValue2 +
                //   "&FormValue=" + someValue2; */

                if (s == 0)
                {  //для начала строки (строка начинается)
                    sendIdToServer = s + "=" + (string)arrayInstruction[s];
                }
                else
                { //продолжение строки (строка продолжается)
                    sendIdToServer += "&" + s + "=" + (string)arrayInstruction[s];
                }
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(sendIdToServer);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
        }

        private void btn_get_json_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://stockcom.ru");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string data = string.Empty;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();

                dynamic jsonData = JsonConvert.DeserializeObject(data);

                foreach (var dataResult in jsonData.result)
                {
                    using (SqlConnection connection = new SqlConnection(constring))
                    {
                        SqlCommand cmd = new SqlCommand("insert into instructions (data_vydachi_zayavki, dolzhnost_vidav_zayavku, rukovod_rabot, teh_karta, data_nomer_prikaza, fio_signal_1, fio_signal_2, fio_signal_3, fio_signal_4, mesto_km_1, mesto_pk_1, mesto_put_1, mesto_km_2, mesto_pk_2, plan_data_rabot, po_soglasovaniyou, date1, data_c_chasov_1, data_min_1, date2, data_c_chasov_2, data_min_2, fio_dispetchera, fio_instructirushego, time_record) values (@data_vydachi_zayavki, @dolzhnost_vidav_zayavku, @rukovod_rabot, @teh_karta, @data_nomer_prikaza, @fio_signal_1, @fio_signal_2, @fio_signal_3, @fio_signal_4, @mesto_km_1, @mesto_pk_1, @mesto_put_1, @mesto_km_2, @mesto_pk_2, @plan_data_rabot, @po_soglasovaniyou, @date1, @data_c_chasov_1, @data_min_1, @date2, @data_c_chasov_2, @data_min_2, @fio_dispetchera, @fio_instructirushego, @time_record)");
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = connection;

                        SqlDataAdapter sqladap = new SqlDataAdapter("select time_record from instructions where time_record = '" + (string)dataResult.time_record + "'", connection);
                        DataTable dataTable = new DataTable();
                        sqladap.Fill(dataTable);
                        if (dataTable.Rows.Count == 0)
                        {
                            cmd.Parameters.AddWithValue("@data_vydachi_zayavki", (string)dataResult.data_vydachi_zayavki);
                            cmd.Parameters.AddWithValue("@dolzhnost_vidav_zayavku", (string)dataResult.dolzhnost_vidav_zayavku);
                            cmd.Parameters.AddWithValue("@rukovod_rabot", (string)dataResult.rukovod_rabot);
                            cmd.Parameters.AddWithValue("@teh_karta", (string)dataResult.teh_karta);
                            cmd.Parameters.AddWithValue("@data_nomer_prikaza", (string)dataResult.data_nomer_prikaza);
                            cmd.Parameters.AddWithValue("@fio_signal_1", (string)dataResult.fio_signal_1);
                            cmd.Parameters.AddWithValue("@fio_signal_2", (string)dataResult.fio_signal_2);
                            cmd.Parameters.AddWithValue("@fio_signal_3", (string)dataResult.fio_signal_3);
                            cmd.Parameters.AddWithValue("@fio_signal_4", (string)dataResult.fio_signal_4);
                            cmd.Parameters.AddWithValue("@mesto_km_1", (string)dataResult.mesto_km_1);
                            cmd.Parameters.AddWithValue("@mesto_pk_1", (string)dataResult.mesto_pk_1);
                            cmd.Parameters.AddWithValue("@mesto_put_1", (string)dataResult.mesto_put_1);
                            cmd.Parameters.AddWithValue("@mesto_km_2", (string)dataResult.mesto_km_2);
                            cmd.Parameters.AddWithValue("@mesto_pk_2", (string)dataResult.mesto_pk_2);
                            cmd.Parameters.AddWithValue("@plan_data_rabot", (string)dataResult.plan_data_rabot);
                            cmd.Parameters.AddWithValue("@po_soglasovaniyou", (string)dataResult.po_soglasovaniyou);
                            cmd.Parameters.AddWithValue("@date1", (string)dataResult.date1);
                            cmd.Parameters.AddWithValue("@data_c_chasov_1", (string)dataResult.data_c_chasov_1);
                            cmd.Parameters.AddWithValue("@data_min_1", (string)dataResult.data_min_1);
                            cmd.Parameters.AddWithValue("@date2", (string)dataResult.date2);
                            cmd.Parameters.AddWithValue("@data_c_chasov_2", (string)dataResult.data_c_chasov_2);
                            cmd.Parameters.AddWithValue("@data_min_2", (string)dataResult.data_min_2);
                            cmd.Parameters.AddWithValue("@fio_dispetchera", (string)dataResult.fio_dispetchera);
                            cmd.Parameters.AddWithValue("@fio_instructirushego", (string)dataResult.fio_instructirushego);
                            cmd.Parameters.AddWithValue("@time_record", (string)dataResult.time_record);
                            connection.Open();
                            cmd.ExecuteNonQuery();
                        }
                        connection.Close();
                    }        
                }
                MessageBox.Show("Данные успешно синхронизированы!");
                RefreshDataGridView(); 
            }
        }
    }
}
