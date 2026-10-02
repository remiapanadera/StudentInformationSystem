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

        // =========================
        // FORM LOAD
        // =========================
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

        // =========================
        // LOAD STUDENTS
        // =========================
        private void LoadStudents()
        {
            Database db = new Database();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT 
                                        student_id,
                                        name,
                                        gender,
                                        year_level,
                                        program,
                                        contact_number
                                     FROM students";

                    using (MySqlDataAdapter adapter =
                           new MySqlDataAdapter(query, conn))
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

        // =========================
        // ADD STUDENT
        // =========================
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Check empty fields
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

                    using (MySqlCommand cmd =
                           new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@student_id",
                            txtStudentID.Text.Trim()
                        );

                        cmd.Parameters.AddWithValue(
                            "@name",
                            txtName.Text.Trim()
                        );

                        cmd.Parameters.AddWithValue(
                            "@gender",
                            cmbGender.Text
                        );

                        cmd.Parameters.AddWithValue(
                            "@year_level",
                            int.Parse(cmbYearLevel.Text)
                        );

                        cmd.Parameters.AddWithValue(
                            "@program",
                            txtProgram.Text.Trim()
                        );

                        cmd.Parameters.AddWithValue(
                            "@contact_number",
                            txtContactNumber.Text.Trim()
                        );

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
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occurred.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================
        // CLEAR FIELDS
        // =========================
        private void ClearFields()
        {
            txtStudentID.Clear();
            txtName.Clear();

            cmbGender.SelectedIndex = -1;
            cmbYearLevel.SelectedIndex = -1;

            txtProgram.Clear();
            txtContactNumber.Clear();
        }

        // =========================
        // CLEAR BUTTON
        // =========================
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void SearchStudents()
        {
            Database db = new Database();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT
                                student_id,
                                name,
                                gender,
                                year_level,
                                program,
                                contact_number
                             FROM students
                             WHERE student_id LIKE @search
                                OR name LIKE @search";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@search",
                            "%" + txtSearch.Text.Trim() + "%"
                        );

                        using (MySqlDataAdapter adapter =
                               new MySqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            dgvStudents.DataSource = table;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Search failed.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================
        // EXISTING DESIGNER EVENTS
        // =========================

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
        }

        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show(
                    "Please select a student to update.",
                    "No Student Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

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

            string oldStudentID =
                dgvStudents.CurrentRow.Cells["student_id"].Value.ToString();

            Database db = new Database();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"UPDATE students
                             SET student_id = @new_id,
                                 name = @name,
                                 gender = @gender,
                                 year_level = @year_level,
                                 program = @program,
                                 contact_number = @contact_number
                             WHERE student_id = @old_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@new_id", txtStudentID.Text.Trim());
                        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                        cmd.Parameters.AddWithValue("@year_level", int.Parse(cmbYearLevel.Text));
                        cmd.Parameters.AddWithValue("@program", txtProgram.Text.Trim());
                        cmd.Parameters.AddWithValue("@contact_number", txtContactNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@old_id", oldStudentID);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Student updated successfully!",
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
                    "Could not update student.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occurred.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show(
                    "Please select a student to delete.",
                    "No Student Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string studentID = dgvStudents.CurrentRow.Cells["student_id"].Value.ToString();

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this student?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
            {
                return;
            }

            Database db = new Database();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = "DELETE FROM students WHERE student_id = @student_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@student_id", studentID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Student deleted successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadStudents();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not delete student.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnAdd_TextChanged(object sender, EventArgs e)
        {
            SearchStudents();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchStudents();
        }
    }
 }
