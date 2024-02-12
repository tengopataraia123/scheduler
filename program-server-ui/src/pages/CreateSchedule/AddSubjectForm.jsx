import React from "react";
import { useFormik } from "formik";
import {
  Box,
  TextField,
  Button,
  Typography,
  Switch,
  FormControlLabel,
} from "@mui/material";
import { postCreateSubject } from "api/createSchedule/requests";
import { toast } from "react-toastify";
import * as yup from "yup";
import InputValidationError from "components/InputValidationError";

const validationSchema = yup.object().shape({
  name: yup.string().required("საგნის სახელის ველი სავალდებულოა."),
  code: yup.string().required("საგნის კოდის ველი სავალდებულოა."),
  description: yup.object().shape({
    step: yup.string(),
    semester: yup.number(),
    credit: yup.number(),
    isMandatory: yup.boolean(),
    startDate: yup.date().required("დაწყების თარიღის ველი სავალდებულოა."),
    endDate: yup.date().required("დასრულების თარიღის ველი სავალდებულოა."),
    location: yup.object().shape({
      address: yup.string(),
    }),
  }),
});
const AddSubjectForm = () => {
  const handleSubmit = (values) => {
    postCreateSubject(values).then((response) => {
      toast.success("საგანი დაემატა");
      formik.resetForm();
    });
  };
  const formik = useFormik({
    initialValues: {
      name: "",
      code: "",
      description: {
        step: "",
        semester: 0,
        credit: 0,
        isMandatory: false,
        startDate: "",
        endDate: "",
        location: {
          latitude: "",
          longitude: "",
          address: "",
        },
      },
    },
    onSubmit: (values) => {
      handleSubmit(values);
    },
    validationSchema: validationSchema,
  });

  return (
    <>
      <Typography variant="h6">პროგრამაში საგნის დამატება</Typography>
      <Box component="form" onSubmit={formik.handleSubmit}>
        {/* Name */}
        <TextField
          margin="normal"
          required
          fullWidth
          id="name"
          label="საგნის სახელი"
          name="name"
          autoComplete="name"
          value={formik.values.name}
          onChange={formik.handleChange}
          onBlur={formik.handleBlur}
          error={formik.touched.name && Boolean(formik.errors.name)}
        />
        {formik.touched.name && formik.errors.name && (
          <InputValidationError message={formik.errors.name} />
        )}

        {/* Code */}
        <TextField
          margin="normal"
          required
          fullWidth
          id="code"
          label="საგნის კოდი"
          name="code"
          autoComplete="code"
          value={formik.values.code}
          onChange={formik.handleChange}
          onBlur={formik.handleBlur}
          error={formik.touched.code && Boolean(formik.errors.code)}
        />
        {formik.touched.code && formik.errors.code && (
          <InputValidationError message={formik.errors.code} />
        )}

        {/* Description Fields */}
        {/* Step */}
        <TextField
          margin="normal"
          fullWidth
          id="description.step"
          label="საფეხური"
          name="description.step"
          value={formik.values.description.step}
          onChange={formik.handleChange}
        />

        {/* Semester */}
        <TextField
          margin="normal"
          fullWidth
          id="description.semester"
          label="სემესტრი"
          name="description.semester"
          type="number"
          value={formik.values.description.semester}
          onChange={formik.handleChange}
        />

        {/* Credit */}
        <TextField
          margin="normal"
          fullWidth
          id="description.credit"
          label="კრედიტი"
          name="description.credit"
          type="number"
          value={formik.values.description.credit}
          onChange={formik.handleChange}
        />

        {/* Is Mandatory */}
        <FormControlLabel
          control={
            <Switch
              checked={formik.values.description.isMandatory}
              onChange={formik.handleChange}
              name="description.isMandatory"
              color="primary"
            />
          }
          label="სავალდებულო"
        />

        {/* Start Date */}
        <TextField
          margin="normal"
          fullWidth
          required
          id="description.startDate"
          label="დაწყების თარიღი"
          name="description.startDate"
          type="date"
          InputLabelProps={{
            shrink: true,
          }}
          value={formik.values.description.startDate}
          onChange={formik.handleChange}
          onBlur={() =>
            formik.setFieldTouched("description.startDate", true, true)
          }
        />
        {formik.touched.description?.startDate &&
          formik.errors.description?.startDate && (
            <InputValidationError
              message={formik.errors.description.startDate}
            />
          )}

        {/* End Date */}
        <TextField
          margin="normal"
          fullWidth
          required
          id="description.endDate"
          label="დასრულების თარიღი"
          name="description.endDate"
          type="date"
          InputLabelProps={{
            shrink: true,
          }}
          value={formik.values.description.endDate}
          onChange={formik.handleChange}
          onBlur={() =>
            formik.setFieldTouched("description.endDate", true, true)
          }
        />
        {formik.touched.description?.endDate &&
          formik.errors.description?.endDate && (
            <InputValidationError message={formik.errors.description.endDate} />
          )}

        {/* Location Fields */}
        {/* Address */}
        <TextField
          margin="normal"
          fullWidth
          id="description.location.address"
          label="მისამართი"
          name="description.location.address"
          value={formik.values.description.location.address}
          onChange={formik.handleChange}
        />

        <Button
          type="submit"
          fullWidth
          variant="contained"
          sx={{ mt: 3, mb: 2 }}
        >
          დამატება
        </Button>
      </Box>
    </>
  );
};

export default AddSubjectForm;
