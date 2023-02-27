using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

using System.Data.Common;

namespace ls02_PicTable04
{
  public partial class GridForm : Form
  {
    DbDataAdapter adapter;
    DataTable table;
    DataGridView grid;

    public GridForm(DbConnection connection, string query,
      DbProviderFactory factory)
    {
      InitializeComponent();

      grid = new();
      Controls.Add(grid);

      try
      {
        DbCommand command = factory.CreateCommand();
        command.Connection = connection;
        command.CommandText = query;

        adapter = factory.CreateDataAdapter();
        adapter.SelectCommand = command;
        var commandBuilder = factory.CreateCommandBuilder();
        table = new();
        adapter.Fill(table);
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }

      StartPosition = FormStartPosition.CenterParent;
      Load += (s, e) => SetGrid();
      FormClosed += (s, e) => SaveData();
    }

    virtual protected void SetGrid()
    {
      grid.Dock = DockStyle.Fill;
      grid.DataSource = table;
      grid.ColumnHeadersDefaultCellStyle.Alignment = 
        DataGridViewContentAlignment.MiddleCenter;

      grid.Columns["Id"].ReadOnly = true;
      grid.Columns["Id"].HeaderText = "Код";
      grid.Columns["Id"].DefaultCellStyle.Alignment =
          DataGridViewContentAlignment.MiddleCenter;

      grid.Columns["Id"].DisplayIndex = 0;
    }

    void SaveData()
    {
      adapter.Update(table);
    }
  }
}
