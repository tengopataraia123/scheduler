import axios from "axios";

const apiUrl = process.env.REACT_APP_API_URL;

export const programs = () => {
  return axios.get(`${apiUrl}/Program/GetAllPrograms`);
};

export const postProgramCreate = (params) => {
  return axios.post(
    `${apiUrl}/Program/Create`,
    {
      name: params.name,
      url: params.url,
    },
    {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    }
  );
};

export const putProgramActivate = (params) => {
  return axios.put(
    `${apiUrl}/Program/Activate/${params.id}`,
    {},
    {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    }
  );
};

export const putProgramDeactivate = (params) => {
  return axios.put(
    `${apiUrl}/Program/Deactivate/${params.id}`,
    {},
    {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    }
  );
};

export const putProgramBlock = (params) => {
  return axios.put(
    `${apiUrl}/Program/Block/${params.id}`,
    {},
    {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    }
  );
};

export const putProgramUnblock = (params) => {
  return axios.put(
    `${apiUrl}/Program/Unblock/${params.id}`,
    {},
    {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    }
  );
};
