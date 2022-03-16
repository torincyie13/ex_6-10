using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace ex_6_10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection booksConnection;
            SqlCommand ISBNCommand;
            SqlDataAdapter ISBNAdapter;
            DataTable ISBNTable;
            string path = Path.GetFullPath("SQLBooksDB.mdf");
            // connect to books database
            booksConnection = new
                SqlConnection("Data Source=.\\SQLEXPRESS; AttachDBFilename=" + path + ";" +
                "Integrated Security=True; Connect Timeout=30; User Instance=True");
            booksConnection.Open();
            // establish command object
            ISBNCommand = new SqlCommand("Select * from Title_Author ORDER BY ISBN", booksConnection);
            // establish data adapter/data table
            ISBNAdapter = new SqlDataAdapter();
            ISBNAdapter.SelectCommand = ISBNCommand;
            ISBNTable = new DataTable();
            ISBNAdapter.Fill(ISBNTable);
            // Count Authors
            int author;
            int[] authorCount = new int[11];
            string lastISBN = "";
            // Allow for up to 10 authors per title
            for (author = 1; author <=10; author++)
            {
                authorCount[author] = 0;
            }
            author = 1;
            // Check each listing for repeated ISBN
            foreach (DataRow myRow in ISBNTable.Rows)
            {
                if (myRow["ISBN"].Equals(lastISBN))
                {
                    // If ISBN repeated, additional author
                    author++;
                }
                else
                {
                    // No more authors for this ISBN
                    authorCount[author]++;
                    author = 1;
                    lastISBN = myRow["ISBN"].ToString();
                }
            }
            // dispaly results
            for (author = 1; author <=10; author++)
            {
                listBox1.Items.Add(authorCount[author].ToString() + "Books with " + author.ToString() + " Authors");
            }
            // dispose
            booksConnection.Close();
            booksConnection.Dispose();
            ISBNCommand.Dispose();
            ISBNAdapter.Dispose();
            ISBNTable.Dispose();
        }
    }
}
