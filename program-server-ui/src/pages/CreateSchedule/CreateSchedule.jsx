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
import AddEventsForm from "./AddEventsForm";
import AddSubjectUsersForm from "./AddSubjectUsersForm";

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
      {selectedOption === "AddEvent" && <AddEventsForm />}
      {selectedOption === "AddSubjectStudents" && <AddSubjectUsersForm />}
    </>
  );
};

export default CreateSchedule;
