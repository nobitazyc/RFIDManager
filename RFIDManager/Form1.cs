using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFIDManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            using (var db = new rfidEntities())
            {
                var query = from u in db.Users select u;
                var users = query.ToList();
                dataGridView1.DataSource = users;
            }
        }



        private void save_Click(object sender, EventArgs e)
        {
            using (var db = new rfidEntities())
            {
                try
                {

                    string tag = tagName.Text;
                    var oldUser = db.Users.Where(u=>u.Tagname == tag).SingleOrDefault();
                    // if exist update the row
                    if (oldUser != null)
                    {
                        oldUser.Username = userName.Text;
                        oldUser.Type = Convert.ToInt32(type.Text);
                        db.Entry(oldUser).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    // if not exist create a new one
                    else
                    {
                        User newUser = new User();
                        newUser.Username = userName.Text;
                        newUser.Tagname = tagName.Text;
                        newUser.Type = Convert.ToInt32(type.Text);
                        db.Users.Add(newUser);
                        db.SaveChanges();
                    }

                }
                catch(Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }
            refreshGridView();
            
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            resetText();
        }




        private void delete_Click(object sender, EventArgs e)
        {
            using (var db = new rfidEntities())
            {
                string tag = tagName.Text;
                if (tag.Trim() == null)
                {
                    return;
                }
                else
                {
                    var oldUser = db.Users.Where(u => u.Tagname == tag).SingleOrDefault();
                    // if exist update the row
                    if (oldUser != null)
                    {
                        db.Users.Remove(oldUser);
                        db.SaveChanges();
                    }

                   
                }
            }

            refreshGridView();
        }


        private void reset_Click(object sender, EventArgs e)
        {
            resetText();
        }



        private void resetText()
        {
            int row = dataGridView1.CurrentCell.RowIndex;
            userName.Text = dataGridView1.Rows[row].Cells[1].Value.ToString();
            tagName.Text = dataGridView1.Rows[row].Cells[2].Value.ToString();
            type.Text = dataGridView1.Rows[row].Cells[3].Value.ToString();
        }


        private void refreshGridView()
        {
            using (var db = new rfidEntities())
            {
                // refresh the datagridview
                var query = from u in db.Users select u;
                var users = query.ToList();
                dataGridView1.DataSource = users;
                dataGridView1.EndEdit();
                dataGridView1.Refresh();
            }
        }
    }
}
