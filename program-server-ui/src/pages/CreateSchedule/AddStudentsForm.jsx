import React from "react";
import { useFormik, FieldArray, FormikProvider } from "formik";
import {
  TextField,
  Button,
  Typography,
  IconButton,
  Paper,
  Box,
  MenuItem,
} from "@mui/material";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import DeleteIcon from "@mui/icons-material/Delete";
import * as yup from "yup";
import { postAddStudent } from "api/createSchedule/requests";
import { toast } from "react-toastify";

const initialValues = {
  students: [
    {
      firstName: "",
      lastName: "",
      email: "",
      roleId: null,
      password: "",
    },
  ],
};

const validationSchema = yup.object({
  students: yup.array().of(
    yup.object({
      firstName: yup.string().required("მომხმარებლის სახელი სავალდებულოა."),
      lastName: yup.string().required("მომხმარებლის გვარი სავალდებულოა."),
      email: yup
        .string()
        .email("მეილი არასწორია.")
        .required("მეილის ველი სავალდებულოა"),
      roleId: yup
        .number()
        .nullable()
        .required("როლის ველი სავალდებულოა")
        .oneOf([1, 2], "როლი არასწორია"),
    })
  ),
});

const AddStudentsForm = () => {
  const formik = useFormik({
    initialValues,
    validationSchema,
    onSubmit: async (values) => {
      try {
        const submissionValues = values.students.map((student) => ({
          ...student,
          password: "",
        }));

        await postAddStudent(submissionValues);
        toast.success("Students list added successfully");
        formik.resetForm();
      } catch (error) {
        toast.error("An error occurred");
      }
    },
  });

  return (
    <FormikProvider value={formik}>
      <form onSubmit={formik.handleSubmit}>
        <Typography variant="h6" sx={{ mb: 2 }}>
          პროგრამაში მომხმარებლების დამატება
        </Typography>
        <FieldArray
          name="students"
          render={(arrayHelpers) => (
            <>
              {formik.values.students.map((student, index) => (
                <Paper
                  key={index}
                  elevation={2}
                  sx={{
                    p: 3,
                    mb: 3,
                    position: "relative",
                    backgroundColor: "#fafafa",
                  }}
                >
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "column",
                      gap: 2,
                    }}
                  >
                    <Box
                      sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                      }}
                    >
                      <Typography variant="h6">
                        მომხმარებელი #{index + 1}
                      </Typography>
                      <IconButton
                        aria-label="remove"
                        onClick={() => arrayHelpers.remove(index)}
                        sx={{
                          color: "error.main",
                        }}
                      >
                        <DeleteIcon />
                      </IconButton>
                    </Box>

                    <TextField
                      name={`students[${index}].firstName`}
                      required
                      label="სახელი"
                      value={student.firstName}
                      onChange={formik.handleChange}
                      onBlur={formik.handleBlur}
                      error={
                        formik.touched.students?.[index]?.firstName &&
                        Boolean(formik.errors.students?.[index]?.firstName)
                      }
                      helperText={
                        formik.touched.students?.[index]?.firstName &&
                        formik.errors.students?.[index]?.firstName
                      }
                      margin="dense"
                      fullWidth
                    />
                    <TextField
                      name={`students[${index}].lastName`}
                      required
                      label="გვარი"
                      value={student.lastName}
                      onChange={formik.handleChange}
                      onBlur={formik.handleBlur}
                      error={
                        formik.touched.students?.[index]?.lastName &&
                        Boolean(formik.errors.students?.[index]?.lastName)
                      }
                      helperText={
                        formik.touched.students?.[index]?.lastName &&
                        formik.errors.students?.[index]?.lastName
                      }
                      margin="dense"
                      fullWidth
                    />
                    <TextField
                      name={`students[${index}].email`}
                      required
                      label="მეილი"
                      value={student.email}
                      onChange={formik.handleChange}
                      onBlur={formik.handleBlur}
                      error={
                        formik.touched.students?.[index]?.email &&
                        Boolean(formik.errors.students?.[index]?.email)
                      }
                      helperText={
                        formik.touched.students?.[index]?.email &&
                        formik.errors.students?.[index]?.email
                      }
                      margin="dense"
                      fullWidth
                    />
                    <TextField
                      select
                      required
                      name={`students[${index}].roleId`}
                      label="როლი"
                      value={student.roleId || ""}
                      onChange={formik.handleChange}
                      onBlur={formik.handleBlur}
                      error={
                        formik.touched.students?.[index]?.roleId &&
                        Boolean(formik.errors.students?.[index]?.roleId)
                      }
                      helperText={
                        formik.touched.students?.[index]?.roleId &&
                        formik.errors.students?.[index]?.roleId
                      }
                      margin="dense"
                      fullWidth
                    >
                      <MenuItem value={1}>სტუდენტი</MenuItem>
                      <MenuItem value={2}>ლექტორი</MenuItem>
                    </TextField>
                  </Box>
                </Paper>
              ))}
              <Box display="flex" justifyContent="center" sx={{ mt: 2 }}>
                <IconButton
                  aria-label="add"
                  onClick={() =>
                    arrayHelpers.push({
                      firstName: "",
                      lastName: "",
                      email: "",
                      roleId: null,
                      password: "",
                    })
                  }
                  color="primary"
                  size="large"
                >
                  <AddCircleOutlineIcon fontSize="inherit" />
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

export default AddStudentsForm;