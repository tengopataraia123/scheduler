import axios from "axios";

const apiUrl = process.env.REACT_APP_API_URL;

export const getSubjects = () => {
  return axios.get(`${apiUrl}/Subject/GetAllSubjects`, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  });
};

export const getUsers = () => {
  return axios.get(`${apiUrl}/SchedulerUser/GetAll`, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  });
};

export const getEvents = () => {
  return axios.get(`${apiUrl}/Event/GetAll`, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  });
};
