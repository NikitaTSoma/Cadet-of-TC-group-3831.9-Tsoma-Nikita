using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using NLog;




namespace KursValut
{   
    public partial class Form1 : Form
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public SqlConnection sqlConnection = null;
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            //создание связи с БД
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBKValuts"].ConnectionString);
            sqlConnection.Open();
            if (sqlConnection.State == ConnectionState.Open)
            {
                logger.Info("Связь с localDb установлена");
            };
        }
        public void button1_Click(object sender, EventArgs e)
        {
            // получение все старых сводок данных БД
          string sql = "SELECT * FROM ValuteCursDynamic";
          SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlConnection);
          DataSet ds = new DataSet();
          adapter.Fill(ds);
          dataGrid1.DataSource = ds.Tables[0];
          logger.Info("получил все старые сводки данных");
        }


        public void button2_Click(object sender, EventArgs e)
        {
            // получение список кодов валют
            CBRS.DailyInfo di = new CBRS.DailyInfo();
            DataSet DSC = (System.Data.DataSet)di.EnumValutes(false); 
            System.Data.DataTable tbl = DSC.Tables["EnumValutes"];
            dataGrid2.DataSource = tbl;
            logger.Info("получил кодов валют");
        }

        public void button3_Click(object sender, EventArgs e)
        {
                // получение списока  курса валют выведение ее на экран и сохранение в БД
                CBRS.DailyInfo di = new CBRS.DailyInfo();
                System.DateTime DateFrom, DateTo;
                DateFrom = dateTimePicker1.Value;
                DateTo = dateTimePicker2.Value;
                string zds;
                zds = textBox1.Text;
                DataSet Ds = (System.Data.DataSet)di.GetCursDynamic(DateFrom, DateTo, zds);
                Ds.Tables[0].Columns[0].ColumnName = "Дата";
                Ds.Tables[0].Columns[1].ColumnName = "код валюты";
                Ds.Tables[0].Columns[2].ColumnName = "Номинал";
                Ds.Tables[0].Columns[3].ColumnName = "Курс";
                logger.Info("получил  динамику курса валют");
                dataGrid1.SetDataBinding(Ds, "ValuteCursDynamic");
                logger.Info("Вывел динамику курса валют");
                DataTable dt = new DataTable();
                dt = Ds.Tables[0];
             using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
            {
                bulkCopy.DestinationTableName = "dbo.ValuteCursDynamic";
                
                    bulkCopy.WriteToServer(dt);
                
                logger.Info("динамика курса валют сохранена в базу данных");
            }

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            logger.Info("выставил время начала отбора");
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            logger.Info("выставил время конца отбора");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            logger.Info("Ввел код валюты");
        }

        public void button4_Click(object sender, EventArgs e)
        {
            // Сохранение данных из БД в XML
            string sql = "SELECT * FROM ValuteCursDynamic";
            string NameXML;
            NameXML = textBox2.Text;
            SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            ds.WriteXml( NameXML +".xml" );
            logger.Info("Сохранил фаил XML");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            logger.Info("Ввел имя файла XML");
        }

        public void dataGrid1_Navigate(object sender, NavigateEventArgs ne)
        {

        }

        public void dataGrid2_Navigate(object sender, NavigateEventArgs ne)
        {

        }
    }
} 
