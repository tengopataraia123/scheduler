import React, { useState } from "react";
import { useFormik, FieldArray, FormikProvider } from "formik";
import {
  TextField,
  Button,
  Typography,
  IconButton,
  Paper,
  Box,
  FormControlLabel,
  Switch,
} from "@mui/material";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import DeleteIcon from "@mui/icons-material/Delete";
import * as yup from "yup";
import { postAddEvents } from "api/createSchedule/requests";
import { toast } from "react-toastify";

const initialValues = {
  events: [
    {
      subjectCode: "",
      startDate: "",
      endDate: "",
    },
  ],
  applySubjectCodeToAll: false,
};

const validationSchema = yup.object({
  events: yup.array().of(
    yup.object({
      subjectCode: yup.string().required("საგნის კოდი სავალდებულოა"),
      startDate: yup
        .date()
        .required("დაწყების თარიღი სავალდებულოა")
        .typeError("თარიღი არავალოდური"),
      endDate: yup
        .date()
        .required("დასრულების თარიღი სავალდებულოა")
        .typeError("თარიღი არავალიდურია"),
    })
  ),
  applySubjectCodeToAll: yup.boolean(),
});

const AddEventsForm = () => {
  const [applySubjectCodeToAll, setApplySubjectCodeToAll] = useState(false);

  const formik = useFormik({
    initialValues,
    validationSchema,
    onSubmit: (values) => {
      const payload = values.events.map((event) => ({
        subjectCode: event.subjectCode,
        startDate: event.startDate,
        endDate: event.endDate,
      }));

      postAddEvents(payload)
        .then(() => {
          toast.success("განრიგი წარმატებით დაემატა");
          formik.resetForm();
        })
        .catch((error) => {
          toast.error("დაფიქსირდა შეცდომა");
          console.error(error);
        });
    },
  });

  const handleToggleChange = (event) => {
    const { checked } = event.target;
    setApplySubjectCodeToAll(checked);
    if (
      checked &&
      formik.values.events.length > 0 &&
      formik.values.events[0].subjectCode
    ) {
      const firstSubjectCode = formik.values.events[0].subjectCode;
      const updatedEvents = formik.values.events.map((event) => ({
        ...event,
        subjectCode: firstSubjectCode,
      }));
      formik.setFieldValue("events", updatedEvents);
    }
  };

  return (
    <FormikProvider value={formik}>
      <form onSubmit={formik.handleSubmit}>
        <Typography variant="h6" sx={{ mb: 2 }}>
          პროგრამაში განრიგის დამატება
        </Typography>
        <FormControlLabel
          control={
            <Switch
              checked={applySubjectCodeToAll}
              onChange={handleToggleChange}
              name="applySubjectCodeToAll"
            />
          }
          label="დაამატე საგნის კოდი ყველა ლექციისთვის"
        />
        <FieldArray
          name="events"
          render={({ insert, remove }) => (
            <>
              {formik.values.events.map((event, index) => (
                <Paper
                  key={index}
                  elevation={2}
                  sx={{
                    p: 7,
                    mb: 5,
                    position: "relative",
                    backgroundColor: "#fafafa",
                    overflow: "visible",
                  }}
                >
                  <Box display="flex" flexDirection="column" gap={2}>
                    <TextField
                      name={`events[${index}].subjectCode`}
                      label="საგნის კოდი"
                      required
                      value={event.subjectCode}
                      onChange={formik.handleChange}
                      disabled={applySubjectCodeToAll && index !== 0}
                      fullWidth
                      onBlur={formik.handleBlur}
                      error={
                        formik.touched.events?.[index]?.subjectCode &&
                        Boolean(formik.errors.events?.[index]?.subjectCode)
                      }
                      helperText={
                        formik.touched.events?.[index]?.subjectCode &&
                        formik.errors.events?.[index]?.subjectCode
                      }
                    />
                    <TextField
                      name={`events[${index}].startDate`}
                      label="დაწყების თარიღი"
                      type="datetime-local"
                      InputLabelProps={{ shrink: true }}
                      value={event.startDate}
                      onChange={formik.handleChange}
                      fullWidth
                      onBlur={() =>
                        formik.setFieldTouched(
                          `events[${index}].startDate`,
                          true
                        )
                      }
                      error={
                        formik.touched.events?.[index]?.startDate &&
                        Boolean(formik.errors.events?.[index]?.startDate)
                      }
                      helperText={
                        formik.touched.events?.[index]?.startDate &&
                        formik.errors.events?.[index]?.startDate
                      }
                    />
                    <TextField
                      name={`events[${index}].endDate`}
                      label="დასრულების თარიღი"
                      type="datetime-local"
                      InputLabelProps={{ shrink: true }}
                      value={event.endDate}
                      onChange={formik.handleChange}
                      fullWidth
                      onBlur={() =>
                        formik.setFieldTouched(`events[${index}].endDate`, true)
                      }
                      error={
                        formik.touched.events?.[index]?.endDate &&
                        Boolean(formik.errors.events?.[index]?.endDate)
                      }
                      helperText={
                        formik.touched.events?.[index]?.endDate &&
                        formik.errors.events?.[index]?.endDate
                      }
                    />
                    {index > 0 && (
                      <IconButton
                        aria-label="remove"
                        onClick={() => remove(index)}
                        sx={{
                          position: "absolute",
                          top: 4,
                          right: 4,
                          color: "error.main",
                        }}
                      >
                        <DeleteIcon fontSize="large" />
                      </IconButton>
                    )}
                  </Box>
                </Paper>
              ))}
              <Box sx={{ display: "flex", justifyContent: "center", mt: 2 }}>
                <IconButton
                  aria-label="add"
                  onClick={() =>
                    insert(formik.values.events.length, {
                      subjectCode: applySubjectCodeToAll
                        ? formik.values.events[0].subjectCode
                        : "",
                      startDate: "",
                      endDate: "",
                    })
                  }
                >
                  <AddCircleOutlineIcon />
                </IconButton>
              </Box>
            </>
          )}
        />
        <Button type="submit" variant="contained" fullWidth sx={{ mt: 3 }}>
          დამატება
        </Button>
      </form>
    </FormikProvider>
  );
};

export default AddEventsForm;
