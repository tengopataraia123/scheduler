import React from "react";
import { ReactComponent as SchedulerIcon } from "../../assets/icons/Scheduler.svg";
import { Link } from "react-router-dom";
import * as yup from "yup";
import { useFormik } from "formik";
import InputValidationError from "../../components/InputValidationError";
import { registration } from "../../api";
import { toast } from "react-toastify";

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
});

export const Register = () => {
  const formik = useFormik({
    initialValues: {
      email: "",
      password: "",
      repeatPassword: "",
    },
    validationSchema: validationSchema,
    onSubmit: (values) => {
      registration({ email: values.email, password: values.password })
        .then((response) => {
          console.log(response);
          toast.success("თქვენ წარმატებით დარეგისტრირდით!");
        })
        .catch((error) => {
          console.log(error);
          toast.error("დაფიქსირდა შეცდომა!");
        });
    },
  });

  return (
    <div className="auth-form-container">
      <SchedulerIcon className="logo" />
      <form className="register-form" onSubmit={formik.handleSubmit} noValidate>
        <h2>ანგარიშის შექმნა</h2>
        <input
          value={formik.values.email}
          type="email"
          placeholder="შეიყვანეთ ელფოსტა"
          id="email"
          name="email"
          onChange={formik.handleChange}
          onBlur={formik.handleBlur}
        />
        {formik.touched.email && formik.errors.email && (
          <InputValidationError message={formik.errors.email} />
        )}
        <input
          value={formik.values.password}
          onChange={formik.handleChange}
          type="password"
          placeholder="შეიყვანეთ პაროლი"
          id="password"
          name="password"
          onBlur={formik.handleBlur}
        />
        {formik.touched.password && formik.errors.password && (
          <InputValidationError message={formik.errors.password} />
        )}
        <input
          value={formik.values.repeatPassword}
          onChange={formik.handleChange}
          type="password"
          placeholder="დაადასტურეთ პაროლი"
          id="repeatPassword"
          name="repeatPassword"
          onBlur={formik.handleBlur}
        />
        {formik.touched.repeatPassword && formik.errors.repeatPassword && (
          <InputValidationError message={formik.errors.repeatPassword} />
        )}
        <button type="submit">დადასტურება</button>
      </form>
      <p className="gray-text">
        არსებული ანგარიშით{" "}
        <Link className="link-btn" to="/login">
          შესვლა
        </Link>
      </p>
    </div>
  );
};
