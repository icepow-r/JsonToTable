using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JsonToTable
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        #region SelectFile
        private void selectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ClearDataGridView();
                var jsonText = ParseJson();
                var values = GetValues(jsonText);

                if (values != null)
                {
                    CreateColumns(jsonText);
                    FillTable(values);
                }
            }
        }

        private void ClearDataGridView()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
        }

        private JObject ParseJson()
        {
            var path = File.ReadAllText(openFileDialog.FileName);
            return JObject.Parse(path);
        }

        private List<List<int>>? GetValues(JObject jsonText)
        {
            var values = new List<List<int>>();
            foreach (var item in jsonText.Properties())
            {
                try
                {
                    values.Add((List<int>)(item.Value?.ToObject(typeof(List<int>)) ?? new List<int>()));
                }
                catch (Newtonsoft.Json.JsonException)
                {
                    MessageBox.Show("Invalid Json file format.");
                    return null;
                }
            }

            return values;
        }

        private void CreateColumns(JObject jsonText)
        {
            foreach (var item in jsonText.Properties())
            {
                dataGridView1.Columns.Add(item.Name, item.Name);
            }
        }

        private void FillTable(List<List<int>> values)
        {
            dataGridView1.Rows.Add(values.Max(x => x.Count));

            for (int column = 0; column < values.Count; column++)
            {
                for (int number = 0; number < dataGridView1.Rows.Count; number++)
                {
                    if (values[column].Count < number + 1)
                    {
                        break;
                    }
                    dataGridView1.Rows[number].Cells[column].Value = values[column][number];
                }
            }
        }

        #endregion

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
