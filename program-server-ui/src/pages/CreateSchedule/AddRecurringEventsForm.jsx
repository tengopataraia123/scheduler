import React, { useState } from "react";
import {
  TextField,
  Grid,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Checkbox,
} from "@mui/material";

const AddRecurringEvents = ({ formik }) => {
  const daysOfWeek = [
    "ორშაბათი",
    "სამშაბათი",
    "ოთხშაბათი",
    "ხუთშაბათი",
    "პარასკევი",
    "შაბათი",
    "კვირა",
  ];

  const [daysData, setDaysData] = useState(
    daysOfWeek.map((day) => ({
      name: day,
      isChecked: false,
      startHour: "09:00",
      endHour: "22:00",
    }))
  );

  const handleCheckboxChange = (index) => {
    const updatedDays = daysData.map((item, i) =>
      i === index ? { ...item, isChecked: !item.isChecked } : item
    );
    setDaysData(updatedDays);
  };

  const handleHourChange = (index, field, value) => {
    const updatedDays = daysData.map((item, i) =>
      i === index ? { ...item, [field]: value } : item
    );
    setDaysData(updatedDays);
  };

  return (
    <Paper
      elevation={2}
      sx={{ p: 2, mb: 5, mt: 2, backgroundColor: "#fafafa" }}
    >
      <Grid container spacing={2}>
        <Grid item xs={12}>
          <TextField
            name="subjectCode"
            label="საგნის კოდი"
            type="text"
            InputLabelProps={{ shrink: true }}
            value={formik.values.subjectCode}
            onChange={formik.handleChange}
            fullWidth
            error={
              formik.touched.subjectCode && Boolean(formik.errors.subjectCode)
            }
            helperText={formik.touched.subjectCode && formik.errors.subjectCode}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            name="recurringStartDate"
            label="დაწყების თარიღი"
            type="date"
            InputLabelProps={{ shrink: true }}
            value={formik.values.recurringStartDate}
            onChange={formik.handleChange}
            fullWidth
            error={
              formik.touched.recurringStartDate &&
              Boolean(formik.errors.recurringStartDate)
            }
            helperText={
              formik.touched.recurringStartDate &&
              formik.errors.recurringStartDate
            }
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            name="recurringEndDate"
            label="დასრულების თარიღი"
            type="date"
            InputLabelProps={{ shrink: true }}
            value={formik.values.recurringEndDate}
            onChange={formik.handleChange}
            fullWidth
            error={
              formik.touched.recurringEndDate &&
              Boolean(formik.errors.recurringEndDate)
            }
            helperText={
              formik.touched.recurringEndDate && formik.errors.recurringEndDate
            }
          />
        </Grid>

        {/* Days - Now using full width for each section */}
        <Grid item xs={12}>
          <Table size="small">
            <TableHead>
              <TableRow>
                <TableCell sx={{ minWidth: 180 }}>კვირის დღეები</TableCell>
                <TableCell sx={{ minWidth: 130 }}>დაწყების საათი</TableCell>
                <TableCell sx={{ minWidth: 130 }}>დასრულების საათი</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {daysData.map((day, index) => (
                <TableRow key={day.name}>
                  <TableCell padding="checkbox">
                    <Checkbox
                      checked={day.isChecked}
                      onChange={() => handleCheckboxChange(index)}
                    />
                    {day.name}
                  </TableCell>
                  <TableCell>
                    <TextField
                      type="time"
                      value={day.startHour}
                      onChange={(e) =>
                        handleHourChange(index, "startHour", e.target.value)
                      }
                      disabled={!day.isChecked}
                      InputLabelProps={{ shrink: true }}
                      inputProps={{ step: 60 }}
                      fullWidth
                    />
                  </TableCell>
                  <TableCell>
                    <TextField
                      type="time"
                      value={day.endHour}
                      onChange={(e) =>
                        handleHourChange(index, "endHour", e.target.value)
                      }
                      disabled={!day.isChecked}
                      InputLabelProps={{ shrink: true }}
                      inputProps={{ step: 60 }}
                      fullWidth
                    />
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </Grid>
      </Grid>
    </Paper>
  );
};

export default AddRecurringEvents;
