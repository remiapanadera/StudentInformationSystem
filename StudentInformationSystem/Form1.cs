using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace StudentInformationSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // LOAD ALL STUDENTS
        private void LoadStudents()
        {
            Database db = new Database();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT student_id, name, gender, year_level, program, contact_number FROM students";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        dgvStudents.DataSource = table;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load students.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ADD STUDENT
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Check for empty fields
            if (string.IsNullOrWhiteSpace(txtStudentID.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                cmbGender.SelectedIndex == -1 ||
                cmbYearLevel.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(txtProgram.Text) ||
                string.IsNullOrWhiteSpace(txtContactNumber.Text))
            {
                MessageBox.Show(
                    "Please fill in all fields.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            Database db = new Database();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"INSERT INTO students
                                     (student_id, name, gender, year_level, program, contact_number)
                                     VALUES
                                     (@student_id, @name, @gender, @year_level, @program, @contact_number)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@student_id", txtStudentID.Text.Trim());
                        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                        cmd.Parameters.AddWithValue("@year_level", int.Parse(cmbYearLevel.Text));
                        cmd.Parameters.AddWithValue("@program", txtProgram.Text.Trim());
                        cmd.Parameters.AddWithValue("@contact_number", txtContactNumber.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Student added successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadStudents();
                ClearFields();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Could not add student.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // CLEAR INPUT FIELDS
        private void ClearFields()
        {
            txtStudentID.Clear();
            txtName.Clear();
            cmbGender.SelectedIndex = -1;
            cmbYearLevel.SelectedIndex = -1;
            txtProgram.Clear();
            txtContactNumber.Clear();
        }

        // FORM LOAD
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void dgvStudents_Load(object sender, EventArgs e)
        {

        }
    }
}