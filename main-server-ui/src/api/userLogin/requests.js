import axios from "axios";

const apiUrl = process.env.REACT_APP_API_URL;

export const login = (params) => {
  return axios.post(`${apiUrl}/UserLogin/UserLogin`, {
    mail: params.email,
    password: params.password,
  });
};

export const registration = (params) => {
  return axios.post(`${apiUrl}/UserLogin/UserRegistration`, {
    mail: params.email,
    password: params.password,
  });
};
