CREATE DATABASE student_information_system;

USE student_information_system;

CREATE TABLE students (
    student_id VARCHAR(20) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    gender VARCHAR(20) NOT NULL,
    year_level INT NOT NULL,
    program VARCHAR(100) NOT NULL,
    contact_number VARCHAR(20) NOT NULL
);

INSERT INTO students
(student_id, name, gender, year_level, program, contact_number)
VALUES
('2026-001', 'Juan Dela Cruz', 'Male', 2, 'BSIT', '09123456789'),
('2026-002', 'Maria Santos', 'Female', 1, 'BSCS', '09987654321'),
('2026-003', 'Pedro Reyes', 'Male', 3, 'BSIT', '09112223333');

SELECT * FROM students;