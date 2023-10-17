using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Data;

namespace DemoDisconnectedLayer1
{
    public partial class frmMyStore : Form
    {
        public frmMyStore()
        {
            InitializeComponent();
        }

        DataSet dsMystore = new DataSet();

        private string GetConnectionString()
        {

            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();

            var strConnection = config["ConnectionStrings:MyStoreDB"];

            return strConnection;

        }
        private void frmMyStore_Load(object sender, EventArgs e)
        {
            DbProviderFactory factory = SqlClientFactory.Instance;

            using DbConnection connection = factory.CreateConnection();

            if (connection == null)
            {

                Console.WriteLine($"Unable to create the connection object.");
                return;

            }

            connection.ConnectionString = GetConnectionString();
            connection.Open();
            string SQL = "Select * From productlist; Select * From categorieslist";
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(SQL, GetConnectionString());
                adapter.Fill(dsMystore);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Data From Database");
            }

        }

        private void btnViewProducts_Click(object sender, EventArgs e)
        {
            dgvData.DataSource = dsMystore.Tables[0];
        }

        private void btnViewCategories_Click(object sender, EventArgs e)
        {
            dgvData.DataSource = dsMystore.Tables[1];
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();
        
    }
}
