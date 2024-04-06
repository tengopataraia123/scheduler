import axios from "axios";

const apiUrl = process.env.REACT_APP_API_URL;

export const deleteEvents = (eventIds) => {
  return axios.post(`${apiUrl}/Event/DeleteEvents`, eventIds, {
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  });
};

export const deleteUsers = (userIds) => {
  return axios.post(`${apiUrl}/SchedulerUSer/DeleteUsers`, userIds, {
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  });
};

export const deleteSubjects = (subjectIds) => {
  return axios.delete(`${apiUrl}/Subject/DeleteSubjects`, {
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
    data: subjectIds,
  });
};
