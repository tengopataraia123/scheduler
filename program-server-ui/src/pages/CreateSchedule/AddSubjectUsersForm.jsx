import React, { useState } from "react";
import {
  Grid,
  TextField,
  Button,
  Paper,
  Typography,
  Chip,
  InputAdornment,
  Tooltip,
  IconButton,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { Search } from "@mui/icons-material";
import { getSubjects, getUsers } from "api/getSchedule/requests";
import { postAddSubjectUsers } from "api/createSchedule/requests";
import { toast } from "react-toastify";
import UploadFileIcon from "@mui/icons-material/UploadFile";
import * as XLSX from "xlsx";

const AddSubjectUsersForm = () => {
  const [subjects, setSubjects] = useState([]);
  const [students, setStudents] = useState([]);
  const [allStudents, setAllStudents] = useState([]);
  const [selectedSubject, setSelectedSubject] = useState("");
  const [selectedStudents, setSelectedStudents] = useState([]);
  const [studentSearchTerm, setStudentSearchTerm] = useState("");

  const handleSubjectSearch = async (event) => {
    const searchTerm = event.target.value;
    if (searchTerm.length > 0) {
      try {
        const response = await getSubjects();
        const data = response.data;
        const filteredSubjects = data
          .filter((subject) =>
            subject.code.toLowerCase().includes(searchTerm.toLowerCase())
          )
          .slice(0, 10);

        setSubjects(filteredSubjects.map((subject) => subject.code));
      } catch (error) {
        console.error("ვერ მოიძებნა საგნები", error);
        setSubjects([]);
      }
    } else {
      setSubjects([]);
    }
  };

  const handleStudentSearch = async (event) => {
    const searchTerm = event.target.value;
    setStudentSearchTerm(searchTerm);

    if (searchTerm.length > 0) {
      try {
        const response = await getUsers();
        const data = response.data;
        const filteredStudents = data
          .filter((user) =>
            user.email.toLowerCase().includes(searchTerm.toLowerCase())
          )
          .slice(0, 10);
        setStudents(filteredStudents.map((user) => user.email));
      } catch (error) {
        console.error("ვერ მოიძებნა მომხმარებლები", error);
        setStudents([]);
      }
    } else {
      setStudents([]);
    }
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
  const handleSubmit = async (e) => {
    e.preventDefault();

    const subjectUsersData = selectedStudents.map((userEmail) => ({
      subjectCode: selectedSubject,
      userEmail: userEmail,
      subjectId: 0,
      userId: 0,
    }));

    try {
      const response = await postAddSubjectUsers(subjectUsersData);
      toast.success("მომხმარებლების სია წარმატებით დაემატა საგანს");
      setSelectedSubject("");
      setStudents([]);
      setSelectedStudents([]);
      setStudentSearchTerm("");
      setSubjects([]);
      console.log("Success:", response.data);
    } catch (error) {
      toast.error("დაფიქსირდა შეცდომა");
      console.error("Error adding subject users:", error);
    }
  };

  const handleImportExcel = (event) => {
    const file = event.target.files[0];
    const reader = new FileReader();
    reader.onload = (e) => {
      const data = new Uint8Array(e.target.result);
      const workbook = XLSX.read(data, { type: "array" });
      const sheetName = workbook.SheetNames[0]; // Assuming the first sheet for demonstration
      if (!sheetName) {
        toast.error("No subject code found in the Excel file.");
        return;
      }
      const worksheet = workbook.Sheets[sheetName];
      const json = XLSX.utils.sheet_to_json(worksheet);
      const studentEmails = json
        .map((item) => item["ელ. ფოსტა"])
        .filter(Boolean);

      setSelectedSubject(sheetName);
      setSelectedStudents(studentEmails);
      setAllStudents(studentEmails);
    };
    reader.readAsArrayBuffer(file);
  };

  return (
    <form>
      <Typography variant="h6" sx={{ mb: 2 }}>
        საგანზე სტუდენტი და ლექტორი დაამატეთ მხოლოდ განრიგის შექმნის შემდეგ
        <label htmlFor="import-excel">
          <input
            accept=".xlsx, .xls"
            id="import-excel"
            type="file"
            hidden
            onChange={handleImportExcel}
          />
          <IconButton color="success" component="span" sx={{ ml: 0.5 }}>
            <UploadFileIcon />
          </IconButton>
        </label>
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
                <Tooltip key={index} title={student} placement="top" arrow>
                  <Typography
                    onClick={() => handleSelectStudent(student)}
                    sx={{
                      cursor: "pointer",
                      "&:hover": { backgroundColor: "#f0f0f0" },
                      mb: 1,
                      whiteSpace: "nowrap",
                      overflow: "hidden",
                      textOverflow: "ellipsis",
                      maxWidth: "100%",
                    }}
                  >
                    {student}
                  </Typography>
                </Tooltip>
              ))}
            </Paper>
          </Grid>
        )}
      </Grid>
      <Button
        type="submit"
        variant="contained"
        fullWidth
        sx={{ mt: 3 }}
        onClick={handleSubmit}
      >
        დამატება
      </Button>
    </form>
  );
};

export default AddSubjectUsersForm;
