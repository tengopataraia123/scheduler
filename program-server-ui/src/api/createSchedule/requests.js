import axios from "axios";

const apiUrl = process.env.REACT_APP_API_URL;

export const postCreateSubject = (subjectData) => {
  return axios.post(`${apiUrl}/Subject/Create`, subjectData, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  });
};
