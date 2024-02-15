import axios from "axios";

const apiUrl = process.env.REACT_APP_API_URL;

export const postCreateSubject = (subjectData) => {
  return axios.post(`${apiUrl}/Subject/Create`, subjectData, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  });
};

export const postAddStudent = (studentData) => {
  return axios.post(`${apiUrl}/SchedulerUser/AddUsers`, studentData, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  });
};

export const postAddEvents = (eventData) => {
  return axios.post(`${apiUrl}/Event/AddEvents`, eventData, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  });
};
