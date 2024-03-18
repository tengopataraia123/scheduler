import React from "react";
import { ReactComponent as SchedulerIcon } from "../../assets/icons/Scheduler.svg";
import { useNavigate } from "react-router-dom";
import * as yup from "yup";
import { useFormik } from "formik";
import InputValidationError from "../../components/InputValidationError";
import { login } from "../../api";
import { toast } from "react-toastify";
import {
  Container,
  CssBaseline,
  Box,
  Typography,
  TextField,
  Button,
} from "@mui/material";
import { useAuth } from "context/authContext";

const validationSchema = yup.object().shape({
  email: yup
    .string()
    // .email("ელფოსტის ფორმატი არასწორია.")
    .required("ელფოსტის ველი სავალდებულოა."),
  password: yup.string().required("პაროლის ველი სავალდებულოა."),
});

export const Login = () => {
  const navigate = useNavigate();
  const { userLogin } = useAuth();

  const formik = useFormik({
    initialValues: {
      email: "",
      password: "",
    },
    validationSchema: validationSchema,
    onSubmit: (values) => {
      login({ email: values.email, password: values.password })
        .then((response) => {
          userLogin(response.data.token);
          toast.success("ავტორიზაცია წარმატებულია");
          navigate("/");
        })
        .catch((error) => {
          console.log(error);
          toast.error("დაფიქსირდა შეცდომა");
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
          სისტემაში შესვლა
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
            autoComplete="current-password"
            value={formik.values.password}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            error={formik.touched.password && Boolean(formik.errors.password)}
          />
          {formik.touched.password && formik.errors.password && (
            <InputValidationError message={formik.errors.password} />
          )}
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            შესვლა
          </Button>
        </Box>
      </Box>
    </Container>
  );
};
