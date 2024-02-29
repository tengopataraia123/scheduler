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
import CancelIcon from "@mui/icons-material/Cancel";
import * as yup from "yup";
import { postAddEvents } from "api/createSchedule/requests";
import { toast } from "react-toastify";
import AddRecurringEvents from "./AddRecurringEventsForm";

const initialValues = {
  subjectCode: "",
  events: [
    {
      subjectCode: "",
      startDate: "",
      endDate: "",
    },
  ],
  applySubjectCodeToAll: true,
  recurring: true,
  recurringStartDate: "",
  recurringEndDate: "",
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
  recurring: yup.boolean(),
});
function calculateEventDates(recurringStartDate, recurringEndDate, daysData) {
  const startDate = new Date(recurringStartDate);
  const endDate = new Date(recurringEndDate);
  let dates = [];

  while (startDate <= endDate) {
    daysData.forEach((day) => {
      if (day.isChecked) {
        const dayOfWeek = startDate.getDay();
        const dayIndex = [
          "კვირა",
          "ორშაბათი",
          "სამშაბათი",
          "ოთხშაბათი",
          "ხუთშაბათი",
          "პარასკევი",
          "შაბათი",
        ].indexOf(day.name);
        if (dayOfWeek === dayIndex) {
          const startDateTime = new Date(startDate);
          const [startHours, startMinutes] = day.startHour.split(":");
          startDateTime.setHours(startHours, startMinutes, 0, 0);

          const endDateTime = new Date(startDate);
          const [endHours, endMinutes] = day.endHour.split(":");
          endDateTime.setHours(endHours, endMinutes, 0, 0);

          dates.push({
            startDate: startDateTime.toISOString(),
            endDate: endDateTime.toISOString(),
          });
        }
      }
    });

    startDate.setDate(startDate.getDate() + 1);
  }

  return dates;
}

function generateRecurringEventsPayload(formValues, applySubjectCodeToAll) {
  const { recurringStartDate, recurringEndDate, events } = formValues;
  let subjectCode = formValues.subjectCode;
  let payload = [];

  if (applySubjectCodeToAll && events.length > 0 && events[0].subjectCode) {
    subjectCode = events[0].subjectCode;
  }

  const recurringDates = calculateEventDates(
    recurringStartDate,
    recurringEndDate,
    events
  );

  recurringDates.forEach((event) => {
    payload.push({
      subjectCode: subjectCode,
      startDate: event.startDate,
      endDate: event.endDate,
    });
  });

  return payload;
}

const AddEventsForm = () => {
  const [applySubjectCodeToAll, setApplySubjectCodeToAll] = useState(true);
  const [recurring, setRecurring] = useState(true);

  const formik = useFormik({
    initialValues,
    validationSchema,
    onSubmit: (values) => {
      let payload;
      if (recurring) {
        payload = generateRecurringEventsPayload(values, applySubjectCodeToAll);
      } else {
        payload = values.events.map((event) => ({
          subjectCode: applySubjectCodeToAll
            ? values.events[0]?.subjectCode || ""
            : event.subjectCode,
          startDate: event.startDate,
          endDate: event.endDate,
        }));
      }

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
    const { name, checked } = event.target;

    if (name === "recurring") {
      setRecurring(checked);
    } else if (name === "applySubjectCodeToAll") {
      setApplySubjectCodeToAll(checked);

      if (checked) {
        const firstSubjectCode =
          formik.values.events.length > 0
            ? formik.values.events[0].subjectCode
            : "";
        const updatedEvents = formik.values.events.map((event) => ({
          ...event,
          subjectCode: firstSubjectCode,
        }));
        formik.setFieldValue("events", updatedEvents);
      }
    }
  };

  return (
    <FormikProvider value={formik}>
      <form onSubmit={formik.handleSubmit}>
        <Typography variant="h6" sx={{ mb: 2 }}>
          პროგრამაში განრიგის დამატება უნდა მოხდეს ბოლოს
        </Typography>
        <FormControlLabel
          control={
            <Switch
              checked={applySubjectCodeToAll}
              onChange={handleToggleChange}
              name="applySubjectCodeToAll"
              disabled={recurring}
            />
          }
          label="დაამატეთ საგნის კოდი ყველა ლექციისთვის"
        />
        <FormControlLabel
          control={
            <Switch
              checked={recurring}
              onChange={handleToggleChange}
              name="recurring"
            />
          }
          label="გამეორებადი ლექციების გენერაცია"
        />
        {recurring ? (
          <AddRecurringEvents formik={formik} />
        ) : (
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
                          formik.setFieldTouched(
                            `events[${index}].endDate`,
                            true
                          )
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
                            color: "black",
                            "&:hover": {
                              color: "red",
                              backgroundColor: "rgba(255, 0, 0, 0.1)",
                            },
                          }}
                        >
                          <CancelIcon fontSize="large" />
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
                          ? formik.values.events[0]?.subjectCode || ""
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
        )}
        <Button type="submit" variant="contained" fullWidth sx={{ mt: 3 }}>
          დამატება
        </Button>
      </form>
    </FormikProvider>
  );
};

export default AddEventsForm;
