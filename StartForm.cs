using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Forms;

using Microsoft.Data.SqlClient;
using ls02_PicTable04;

namespace ls03_ProviderFactories01
{
  public partial class StartForm : Form
  {
    int unit = 100;

    const string sqlProvider = "SqlClient";
    const string accProvider = "MS Access";

    ComboBox providers;

    public StartForm()
    {
      InitializeComponent();

      providers = new ComboBox();
      Controls.Add(providers);
      providers.Items.Add(sqlProvider);
      providers.Items.Add(accProvider);
      SetForm();

      providers.SelectedIndexChanged += (s, e) => ViewData();
    }

    void SetForm()
    {
      //providers.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      providers.Top = 0;
      providers.Left = unit;
      providers.Width = 2 * unit;

      Text = "Data Base Providers";
      ClientSize = new Size(4 * unit, unit);
      this.StartPosition = FormStartPosition.CenterScreen;
    }

    void ViewData()
    {
      string? connectionString = null;
      DbProviderFactory? factory = null;

      switch(providers.SelectedItem.ToString())
      {
        case sqlProvider:
          connectionString = ConfigurationManager.AppSettings
            .Get("sqlConnectionString");
          factory = SqlClientFactory.Instance;
          break;
        case accProvider:
          connectionString = ConfigurationManager.AppSettings
            .Get("accessConnectionString");
          factory = OleDbFactory.Instance;
          break;
      }

      var connection = factory.CreateConnection();
      connection.ConnectionString = connectionString;
      GridForm authorForm = new GridForm(connection, "Select * from Authors", factory);
      authorForm.ShowDialog();
    }
  }
}
