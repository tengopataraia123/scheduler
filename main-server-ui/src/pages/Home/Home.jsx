import { Box, Typography, TextField, Button } from "@mui/material";
import { useFormik } from "formik";
import React from "react";
import * as yup from "yup";
import InputValidationError from "components/InputValidationError";
import { postProgramCreate } from "api";
import { toast } from "react-toastify";

const validationSchema = yup.object().shape({
  name: yup.string().required("სახელის ველი სავალდებულოა."),
  url: yup
    .string()
    .url("შეიყვანეთ სწორი URL მისამართი")
    .required("საიტის მისამართის ველი სავალდებულოა."),
});

const Home = () => {
  const handleSubmit = (values) => {
    postProgramCreate(values).then((response) => {
      toast.success("პროგრამა დაემატა");
      formik.resetForm();
    });
  };
  const formik = useFormik({
    initialValues: {
      name: "",
      url: "",
    },
    onSubmit: (values) => {
      handleSubmit(values);
    },
    validationSchema: validationSchema,
  });
  return (
    <>
      <Typography variant="h4" component="h1">
        პროგრამის დამატება
      </Typography>
      <Box component="form" onSubmit={formik.handleSubmit}>
        <TextField
          margin="normal"
          required
          fullWidth
          id="name"
          label="პროგრამის სახელი"
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
        <TextField
          margin="normal"
          required
          fullWidth
          id="url"
          label="პროგრამის ლინკი"
          name="url"
          autoComplete="url"
          value={formik.values.url}
          onChange={formik.handleChange}
          onBlur={formik.handleBlur}
          error={formik.touched.url && Boolean(formik.errors.url)}
        />
        {formik.touched.url && formik.errors.url && (
          <InputValidationError message={formik.errors.url} />
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
