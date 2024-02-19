import axios from "axios";

const apiUrl = process.env.REACT_APP_API_URL;

export const deleteEvents = (subjectIds) => {
  const queryString = subjectIds.map((id) => `subjectIds=${id}`).join("&");

  return axios.delete(`${apiUrl}/Event/DeleteBySubjectIds?${queryString}`, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  });
};
