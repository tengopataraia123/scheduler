import axios from "axios";

const mainApiUrl = process.env.REACT_APP_MAIN_API_URL;

export const login = (params) => {
  return axios.post(`${mainApiUrl}/UserLogin/UserLogin`, {
    mail: params.email,
    password: params.password,
  });
};
