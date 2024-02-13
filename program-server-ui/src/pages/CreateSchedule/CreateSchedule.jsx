import React, { useState } from "react";
import {
  Box,
  Typography,
  Button,
  MenuItem,
  FormControl,
  Select,
  OutlinedInput,
  InputLabel,
} from "@mui/material";
import { useFormik } from "formik";
import AddSubjectForm from "./AddSubjectForm";
import AddStudentsForm from "./AddStudentsForm";

const CreateSchedule = () => {
  const [selectedOption, setSelectedOption] = useState("");

  const formik = useFormik({
    initialValues: {
      subject: "",
      student: "",
      event: "",
      subjectStudents: { subjectId: "", userId: "" },
    },
    onSubmit: (values) => {
      console.log(values);
      // Handle submission here
    },
  });

  return (
    <>
      <Typography variant="h4" component="h1" sx={{ mb: 4 }}>
        ცხრილის შექმნა
      </Typography>
      <Box sx={{ mb: 2 }}>
        <FormControl fullWidth>
          <InputLabel id="demo-simple-select-label">კატეგორია</InputLabel>
          <Select
            labelId="demo-simple-select-label"
            id="demo-simple-select"
            value={selectedOption}
            label="Add Option"
            onChange={(e) => setSelectedOption(e.target.value)}
            input={<OutlinedInput label="Add Option" />}
          >
            <MenuItem value="AddSubject">საგანი</MenuItem>
            <MenuItem value="AddStudent">მომხმარებლები</MenuItem>
            <MenuItem value="AddEvent">განრიგი</MenuItem>
            <MenuItem value="AddSubjectStudents">
              საგანი & მომხმარებელი
            </MenuItem>
          </Select>
        </FormControl>
      </Box>

      {selectedOption === "AddSubject" && <AddSubjectForm />}
      {selectedOption === "AddStudent" && <AddStudentsForm />}

      {selectedOption === "AddEvent" && (
        <Box component="form" onSubmit={formik.handleSubmit}>
          {/* Form for AddEvent */}
          <Typography variant="h6">ლექციების განრიგის შექმნა</Typography>
          {/* Add your form fields here */}
          <Button type="submit" variant="contained" sx={{ mt: 2 }}>
            დამატება
          </Button>
        </Box>
      )}

      {selectedOption === "AddSubjectStudents" && (
        <Box component="form" onSubmit={formik.handleSubmit}>
          {/* Form for AddSubjectStudents */}
          <Typography variant="h6">
            საგანზე სტუდენტი და ლექტორი დაამატეთ მხოლოდ განრიგის შექმნის შემდეგ
          </Typography>
          {/* Add your form fields here */}
          <Button type="submit" variant="contained" sx={{ mt: 2 }}>
            დამატება
          </Button>
        </Box>
      )}
    </>
  );
};

export default CreateSchedule;
