import React from "react";
import {
  TextField,
  Button,
  Grid,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Checkbox,
  Typography,
} from "@mui/material";
import { useFormik, FormikProvider } from "formik";
import * as yup from "yup";
import { postAddRecurringEvents } from "api/createSchedule/requests";
import { toast } from "react-toastify";

const AddRecurringEventsForm = () => {
  const daysOfWeek = [
    "ორშაბათი",
    "სამშაბათი",
    "ოთხშაბათი",
    "ხუთშაბათი",
    "პარასკევი",
    "შაბათი",
    "კვირა",
  ];

  const initialValues = {
    subjectCode: "",
    recurringStartDate: "",
    recurringEndDate: "",
    daysOfWeek: daysOfWeek.map((day) => ({
      day: day,
      startHour: "",
      endHour: "",
      isChecked: false,
    })),
  };

  const validationSchema = yup.object({
    subjectCode: yup.string().required("საგნის კოდი სავალდებულოა"),
    recurringStartDate: yup
      .date()
      .required("დაწყების თარიღი სავალდებულოა")
      .typeError("თარიღი არავალოდური"),
    recurringEndDate: yup
      .date()
      .required("დასრულების თარიღი სავალდებულოა")
      .typeError("თარიღი არავალიდურია"),
    // daysOfWeek: yup.array().of(
    //   yup.object({
    //     day: yup.string().required(),
    //     startHour: yup.string().when("isChecked", {
    //       is: true,
    //       then: yup.string().required("დაწყების საათი სავალდებულოა"),
    //       otherwise: yup.string().notRequired(),
    //     }),
    //     endHour: yup.string().when("isChecked", {
    //       is: true,
    //       then: yup.string().required("დასრულების საათი სავალდებულოა"),
    //       otherwise: yup.string().notRequired(),
    //     }),
    //     isChecked: yup.boolean(),
    //   })
    // ),
  });

  const formik = useFormik({
    initialValues,
    validationSchema,
    onSubmit: (values) => {
      const filteredDays = values.daysOfWeek
        .filter((day) => day.isChecked)
        .map(({ day, startHour, endHour, isChecked }) => ({
          day,
          startHour,
          endHour,
          isChecked,
        }));

      const payload = {
        subjectCode: values.subjectCode,
        recurringStartDate: values.recurringStartDate,
        recurringEndDate: values.recurringEndDate,
        daysOfWeek: filteredDays,
      };

      postAddRecurringEvents(payload)
        .then((response) => {
          console.log("Success:", response);
          toast.success("განრიგი წარმატებით დაემატა");
          formik.resetForm();
        })
        .catch((error) => {
          toast.error("დაფიქსირდა შეცდომა");
          console.error(error);
        });
    },
  });

  const handleCheckboxChange = (index) => {
    console.log("index", index);
    const updatedDays = formik.values.daysOfWeek.map((item, i) =>
      i === index ? { ...item, isChecked: !item.isChecked } : item
    );
    formik.setFieldValue("daysOfWeek", updatedDays);
  };

  const handleHourChange = (index, field, value) => {
    const updatedDays = formik.values.daysOfWeek.map((item, i) =>
      i === index ? { ...item, [field]: value } : item
    );
    formik.setFieldValue("daysOfWeek", updatedDays);
  };

  return (
    <FormikProvider value={formik}>
      <form onSubmit={formik.handleSubmit}>
        <Typography variant="h6" sx={{ mb: 2 }}>
          Add Recurring Schedule
        </Typography>
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
                  formik.touched.subjectCode &&
                  Boolean(formik.errors.subjectCode)
                }
                helperText={
                  formik.touched.subjectCode && formik.errors.subjectCode
                }
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
                  formik.touched.recurringEndDate &&
                  formik.errors.recurringEndDate
                }
              />
            </Grid>
            <Grid item xs={12}>
              <Table size="small">
                <TableHead>
                  <TableRow>
                    <TableCell sx={{ minWidth: 180 }}>კვირის დღეები</TableCell>
                    <TableCell sx={{ minWidth: 130 }}>დაწყების საათი</TableCell>
                    <TableCell sx={{ minWidth: 130 }}>
                      დასრულების საათი
                    </TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {daysOfWeek.map((day, index) => (
                    <TableRow key={day}>
                      <TableCell padding="checkbox">
                        <Checkbox
                          checked={formik.values.daysOfWeek[index].isChecked}
                          onChange={() => handleCheckboxChange(index)}
                        />
                        {day}
                      </TableCell>
                      <TableCell>
                        <TextField
                          type="time"
                          value={formik.values.daysOfWeek[index].startHour}
                          onChange={(e) =>
                            handleHourChange(index, "startHour", e.target.value)
                          }
                          disabled={!formik.values.daysOfWeek[index].isChecked}
                          InputLabelProps={{ shrink: true }}
                          inputProps={{ step: 60 }}
                          fullWidth
                        />
                      </TableCell>
                      <TableCell>
                        <TextField
                          type="time"
                          value={formik.values.daysOfWeek[index].endHour}
                          onChange={(e) =>
                            handleHourChange(index, "endHour", e.target.value)
                          }
                          disabled={!formik.values.daysOfWeek[index].isChecked}
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
        <Button type="submit" variant="contained" fullWidth sx={{ mt: 3 }}>
          დამატება
        </Button>
      </form>
    </FormikProvider>
  );
};

export default AddRecurringEventsForm;
