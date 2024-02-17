import React from "react";
import { ReactComponent as SchedulerIcon } from "../../assets/icons/Scheduler.svg";
import { Link, useNavigate } from "react-router-dom";
import * as yup from "yup";
import { useFormik } from "formik";
import InputValidationError from "../../components/InputValidationError";
import { registration } from "../../api";
import { toast } from "react-toastify";
import {
  Container,
  CssBaseline,
  Box,
  Typography,
  TextField,
  Button,
} from "@mui/material";

const validationSchema = yup.object().shape({
  email: yup
    .string()
    .email("ელფოსტის ფორმატი არასწორია.")
    .required("ელფოსტის ველი სავალდებულოა."),
  password: yup.string().required("პაროლის ველი სავალდებულოა."),
  repeatPassword: yup
    .string()
    .oneOf([yup.ref("password"), null], "პაროლები ერთმანეთს არ ემთხვევა.")
    .required("პაროლის ველი სავალდებულოა."),
  firstName: yup.string().required("სახელის ველი სავალდებულოა."),
  lastName: yup.string().required("გვარის ველი სავალდებულოა."),
});

export const Register = () => {
  const navigate = useNavigate();
  const formik = useFormik({
    initialValues: {
      email: "",
      password: "",
      repeatPassword: "",
      firstName: "",
      lastName: "",
      userName: "",
    },

    validationSchema: validationSchema,
    onSubmit: (values) => {
      registration({
        email: values.email,
        password: values.password,
        firstName: values.firstName,
        lastName: values.lastName,
        userName: "",
      })
        .then((response) => {
          toast.success("თქვენ წარმატებით დარეგისტრირდით!");
          navigate("/login");
        })
        .catch((error) => {
          toast.error("დაფიქსირდა შეცდომა!");
        });
    },
  });

  return (
    <Container
      component="main"
      maxWidth="xs"
      className="min-h-screen items-center"
      sx={{ display: "flex" }}
    >
      <CssBaseline />
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
        className="min-w-[400px]"
      >
        <SchedulerIcon className="logo" />
        <Typography component="h1" variant="h5" mt={4}>
          ანგარიშის შექმნა
        </Typography>
        <Box
          component="form"
          onSubmit={formik.handleSubmit}
          noValidate
          sx={{ mt: 1 }}
          className="w-full"
        >
          <TextField
            margin="normal"
            required
            fullWidth
            id="firstName"
            label="სახელი"
            name="firstName"
            autoComplete="given-name"
            value={formik.values.firstName}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            error={formik.touched.firstName && Boolean(formik.errors.firstName)}
          />
          {formik.touched.firstName && formik.errors.firstName && (
            <InputValidationError message={formik.errors.firstName} />
          )}
          <TextField
            margin="normal"
            required
            fullWidth
            id="lastName"
            label="გვარი"
            name="lastName"
            autoComplete="family-name"
            value={formik.values.lastName}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            error={formik.touched.lastName && Boolean(formik.errors.lastName)}
          />
          {formik.touched.lastName && formik.errors.lastName && (
            <InputValidationError message={formik.errors.lastName} />
          )}
          <TextField
            margin="normal"
            required
            fullWidth
            id="email"
            label="ელ-ფოსტა"
            name="email"
            autoComplete="email"
            value={formik.values.email}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            error={formik.touched.email && Boolean(formik.errors.email)}
            autoFocus
          />
          {formik.touched.email && formik.errors.email && (
            <InputValidationError message={formik.errors.email} />
          )}
          <TextField
            margin="normal"
            required
            fullWidth
            name="password"
            label="პაროლი"
            type="password"
            id="password"
            autoComplete="new-password"
            value={formik.values.password}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            error={formik.touched.password && Boolean(formik.errors.password)}
          />
          {formik.touched.password && formik.errors.password && (
            <InputValidationError message={formik.errors.password} />
          )}
          <TextField
            margin="normal"
            required
            fullWidth
            name="repeatPassword"
            label="პაროლის გამეორება"
            type="password"
            id="repeatPassword"
            autoComplete="new-password"
            value={formik.values.repeatPassword}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            error={
              formik.touched.repeatPassword &&
              Boolean(formik.errors.repeatPassword)
            }
          />
          {formik.touched.repeatPassword && formik.errors.repeatPassword && (
            <InputValidationError message={formik.errors.repeatPassword} />
          )}
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            დადასტურება
          </Button>
          <p className="gray-text">
            არსებული ანგარიშით{" "}
            <Link className="link-btn" to="/login">
              შესვლა
            </Link>
          </p>
        </Box>
      </Box>
    </Container>
  );
};
