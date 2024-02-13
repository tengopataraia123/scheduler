import React, { useState } from "react";
import {
  Grid,
  TextField,
  Button,
  Paper,
  Typography,
  Chip,
  InputAdornment,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { Search } from "@mui/icons-material";

const AddSubjectUsersForm = () => {
  const [subjects, setSubjects] = useState([]);
  const [students, setStudents] = useState([]);
  const [allStudents, setAllStudents] = useState([]);
  const [selectedSubject, setSelectedSubject] = useState("");
  const [selectedStudents, setSelectedStudents] = useState([]);
  const [studentSearchTerm, setStudentSearchTerm] = useState("");

  const searchSubjects = async (searchTerm) => {
    return ["SUBJ101", "SUBJ102", "SUBJ103"].filter((code) =>
      code.toLowerCase().includes(searchTerm.toLowerCase())
    );
  };

  const searchStudents = async (searchTerm) => {
    const filteredStudents = [
      "student1@example.com",
      "student2@example.com",
      "student3@example.com",
    ].filter(
      (email) =>
        email.toLowerCase().includes(searchTerm.toLowerCase()) &&
        !selectedStudents.includes(email)
    );
    setStudents(filteredStudents);
    if (!searchTerm) {
      setAllStudents(filteredStudents);
    }
  };

  const handleSubjectSearch = async (event) => {
    const searchTerm = event.target.value;
    if (searchTerm.length > 0) {
      const foundSubjects = await searchSubjects(searchTerm);
      setSubjects(foundSubjects);
    } else {
      setSubjects([]);
    }
  };

  const handleStudentSearch = async (event) => {
    const searchTerm = event.target.value;
    setStudentSearchTerm(searchTerm);
    await searchStudents(searchTerm);
  };

  const handleDeselectSubject = () => {
    setSelectedSubject("");
    setSubjects([]);
    setSelectedStudents([]);
    setStudents(allStudents);
    setAllStudents([]);
    setStudentSearchTerm("");
  };

  const handleSelectStudent = (email) => {
    setSelectedStudents((prevSelectedStudents) => [
      ...prevSelectedStudents,
      email,
    ]);
    setStudents((prevStudents) =>
      prevStudents.filter((student) => student !== email)
    );
  };

  const handleDeselectStudent = (email) => {
    setSelectedStudents((prevSelectedStudents) =>
      prevSelectedStudents.filter((student) => student !== email)
    );
    if (email.toLowerCase().includes(studentSearchTerm.toLowerCase())) {
      setStudents((prevStudents) =>
        [...prevStudents, email].filter(
          (studentEmail) => !selectedStudents.includes(studentEmail)
        )
      );
    }
  };

  return (
    <form>
      <Typography variant="h6" sx={{ mb: 2 }}>
        საგანზე სტუდენტი და ლექტორი დაამატეთ მხოლოდ განრიგის შექმნის შემდეგ
      </Typography>
      <Grid container spacing={2}>
        <Grid item xs={8}>
          <Paper elevation={3} sx={{ p: 2, minHeight: "400px" }}>
            {selectedSubject && (
              <Chip
                label={selectedSubject}
                onDelete={handleDeselectSubject}
                deleteIcon={<CloseIcon />}
                sx={{
                  width: "100%",
                  bgcolor: "grey.300",
                  color: "black",
                  justifyContent: "space-between",
                  mb: 2,
                }}
              />
            )}
            {!selectedSubject && (
              <>
                <TextField
                  label="ძიება"
                  placeholder="შეიყვანეთ საგნის კოდი"
                  variant="outlined"
                  onChange={handleSubjectSearch}
                  fullWidth
                  sx={{ my: 2 }}
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <Search />
                      </InputAdornment>
                    ),
                  }}
                />
                {subjects.map((subject, index) => (
                  <Typography
                    key={index}
                    onClick={() => setSelectedSubject(subject)}
                    sx={{
                      cursor: "pointer",
                      "&:hover": { backgroundColor: "#f0f0f0" },
                      mb: 1,
                    }}
                  >
                    {subject}
                  </Typography>
                ))}
              </>
            )}
            {selectedStudents.map((email, index) => (
              <Chip
                key={index}
                label={email}
                onDelete={() => handleDeselectStudent(email)}
                deleteIcon={<CloseIcon />}
                sx={{ mt: 1, mr: 1 }}
              />
            ))}
          </Paper>
        </Grid>
        {selectedSubject && (
          <Grid item xs={4}>
            <Paper elevation={3} sx={{ p: 2, minHeight: "400px" }}>
              <TextField
                label="ძიება"
                placeholder="შეიყვანეთ მომხმარებლის მეილი"
                value={studentSearchTerm}
                onChange={handleStudentSearch}
                fullWidth
                sx={{ mb: 2 }}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <Search />
                    </InputAdornment>
                  ),
                }}
              />
              {students.map((student, index) => (
                <Typography
                  key={index}
                  onClick={() => handleSelectStudent(student)}
                  sx={{
                    cursor: "pointer",
                    "&:hover": { backgroundColor: "#f0f0f0" },
                    mb: 1,
                  }}
                >
                  {student}
                </Typography>
              ))}
            </Paper>
          </Grid>
        )}
      </Grid>
      <Button type="submit" variant="contained" sx={{ mt: 3 }}>
        Submit
      </Button>
    </form>
  );
};

export default AddSubjectUsersForm;
