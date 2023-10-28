import { Box, Typography, TextField, Button } from "@mui/material";
import { useFormik } from "formik";
import React from "react";
import * as yup from "yup";
import InputValidationError from "components/InputValidationError";
import { postSurveyCreate } from "api";
import { toast } from "react-toastify";

const validationSchema = yup.object().shape({
  subject: yup.string().required("საგნის სახელის ველი სავალდებულოა."),
  question: yup.string().required("შეკითხვის ველი სავალდებულოა."),
});

const Home = () => {
  const handleSubmit = (values) => {
    postSurveyCreate(values).then((response) => {
      toast.success("კითხვარი დაემატა");
      formik.resetForm();
    });
  };
  const formik = useFormik({
    initialValues: {
      subject: "",
      question: "",
    },
    onSubmit: (values) => {
      handleSubmit(values);
    },
    validationSchema: validationSchema,
  });
  return (
    <>
      <Typography variant="h4" component="h1">
        გამოკითხვის დამატება
      </Typography>
      <Box component="form" onSubmit={formik.handleSubmit}>
        <TextField
          margin="normal"
          required
          fullWidth
          id="subject"
          label="საგნის სახელი"
          name="subject"
          autoComplete="subject"
          value={formik.values.subject}
          onChange={formik.handleChange}
          onBlur={formik.handleBlur}
          error={formik.touched.subject && Boolean(formik.errors.subject)}
        />
        {formik.touched.subject && formik.errors.subject && (
          <InputValidationError message={formik.errors.subject} />
        )}
        <TextField
          margin="normal"
          required
          fullWidth
          id="question"
          label="შეკითხვა"
          name="question"
          autoComplete="question"
          value={formik.values.question}
          onChange={formik.handleChange}
          onBlur={formik.handleBlur}
          error={formik.touched.question && Boolean(formik.errors.question)}
        />
        {formik.touched.question && formik.errors.question && (
          <InputValidationError message={formik.errors.question} />
        )}
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

export default Home;
