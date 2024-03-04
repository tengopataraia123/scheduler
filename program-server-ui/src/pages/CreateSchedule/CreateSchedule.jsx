import React, { useState } from "react";
import {
  Box,
  Typography,
  MenuItem,
  FormControl,
  Select,
  OutlinedInput,
  InputLabel,
} from "@mui/material";
import AddSubjectForm from "./AddSubjectForm";
import AddStudentsForm from "./AddStudentsForm";
import AddEventsForm from "./AddEventsForm";
import AddSubjectUsersForm from "./AddSubjectUsersForm";
import AddRecurringEventsForm from "./AddRecurringEventsForm";

const CreateSchedule = () => {
  const [selectedOption, setSelectedOption] = useState("");

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
            <MenuItem value="AddSubject"> საგანი</MenuItem>
            <MenuItem value="AddStudent"> მომხმარებლები</MenuItem>
            <MenuItem value="AddSubjectStudents">
              საგანი & მომხმარებელი
            </MenuItem>
            <MenuItem value="AddEvent"> განრიგის ხელით დამატება</MenuItem>
            <MenuItem value="AddRecurringEventsForm">
              {" "}
              განრიგის გენერაცია
            </MenuItem>
          </Select>
        </FormControl>
      </Box>

      {selectedOption === "AddSubject" && <AddSubjectForm />}
      {selectedOption === "AddStudent" && <AddStudentsForm />}
      {selectedOption === "AddSubjectStudents" && <AddSubjectUsersForm />}
      {selectedOption === "AddEvent" && <AddEventsForm />}
      {selectedOption === "AddRecurringEventsForm" && (
        <AddRecurringEventsForm />
      )}
    </>
  );
};

export default CreateSchedule;
