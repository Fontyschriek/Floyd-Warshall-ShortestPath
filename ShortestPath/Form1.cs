using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortestPath
{
    public partial class Form1: Form
    {
        private Matrix matrix;

        public Form1( )
        {
            InitializeComponent();
        }

        private void btnGenerateMatrix_Click(object sender, EventArgs e)
        {
            GenerateMatrix();
        }

        private void GenerateMatrix( )
        {
            panel1.Controls.Clear();
            shortPathLabel.Text = "result will be shown here";
            matrix = new Matrix(txtVertices.Text, txtStart.Text, txtEnd.Text);
            int tbWidth = 35, tbHeigth = 22, space = 3, x = 45, y = 3;
            Label lb;
            foreach (string vertex in matrix.Vertices)
            {
                lb = new Label();
                lb.Location = new Point(x, y);
                lb.Size = new Size(tbWidth, tbHeigth);
                lb.Text = vertex;
                panel1.Controls.Add(lb);
                x += tbWidth + space;
            }
            tbWidth = 30;
            tbHeigth = 22;
            space = 3;
            x = 2;
            y = 25;
            foreach (string vertex in matrix.Vertices)
            {
                lb = new Label();
                lb.Location = new Point(x, y);
                lb.Size = new Size(tbWidth, tbHeigth);
                lb.Text = vertex;
                panel1.Controls.Add(lb);
                y += tbHeigth + space;
            }
            tbWidth = 35;
            tbHeigth = 22;
            space = 3;
            x = 36;
            y = 25;

            TextBox tb;
            int rowNum = 1,columnNum = 1;
            foreach (List<Edge> row in matrix.Edges)
            {
                foreach (Edge edge in row)
                {
                    tb = new TextBox();
                    tb.Location = new Point(x, y);
                    tb.Size = new Size(tbWidth, tbHeigth);
                    tb.Text = edge.Weight.ToString();
                    tb.Tag = edge;
                    tb.Click += SelectMe;
                    tb.Leave += Deselect;
                    panel1.Controls.Add(tb);
                    x += tbWidth + space;
                    if (rowNum == columnNum)
                        tb.Enabled = false;
                    columnNum++;
                }
                x = 36;
                y += tbHeigth + space;
                columnNum = 1;
                rowNum++;
            }
            btnCalculate.Enabled = true;
            btnGenerateAll.Enabled = true;
        }

        private void SelectMe(object sender, EventArgs e)
        {
            ( (TextBox)sender ).Text = "";
        }

        private void Deselect(object sender, EventArgs e)
        {
            TextBox tx = ((TextBox) sender);
            if (tx.Text == "")
                tx.Text = "-1";
            else
            {
                ( (Edge)tx.Tag ).Weight = Convert.ToInt32(tx.Text);
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            shortPathLabel.Text = matrix.GetShortestPath();
        }

        public void AddEdge(string code)
        {
            List<string> command = code.Split().ToList();
            foreach (Control item in this.panel1.Controls)
            {
                if (item is TextBox)
                    if (( (Edge)item.Tag ).Name == command[0])
                    {
                        ( (Edge)item.Tag ).Weight = Convert.ToInt32(command[1]);
                        item.Text = command[1];
                    }
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddEdge(textBox1.Text);
                textBox1.Clear();
                textBox1.Select();
            }
        }

        private void btnGenerateAll_Click(object sender, EventArgs e)
        {
            SetListSource(matrix.GetAllPaths());
        }

        private void SetListSource(List<string> list)
        {
            listBox1.DataSource = null;
            listBox1.DataSource = list;
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<string> filtered = new List<string>();
                foreach (string item in matrix.GetAllPaths())
                {
                    if (item.Contains(txtSearch.Text))
                        filtered.Add(item);
                }
                SetListSource(filtered);
            }
        }

        private void txtVertices_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GenerateMatrix();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName;
            if (path.Length > 0 && path.Contains(".txt"))
            {
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));
                    string row = sr.ReadLine();
                    txtVertices.Text = row;
                    GenerateMatrix();
                    row = sr.ReadLine();
                    while (row != null)
                    {
                        AddEdge(row);
                        row = sr.ReadLine();
                    }
                    try
                    {
                        SetListSource(matrix.GetAllPaths());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Please make sure you have ecerything right!");
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (sr != null) sr.Close();
                }
            } else
            {
                MessageBox.Show("Please select a valid file!");
            }
        }
    }
}