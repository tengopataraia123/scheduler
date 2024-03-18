import axios from "axios";

const programServerUri = process.env.REACT_APP_API_URL;

export const login = (params) => {
  return axios.post(`${programServerUri}/Auth/AuthenticateAdmin`, {
    mail: params.email,
    password: params.password,
  });
};
