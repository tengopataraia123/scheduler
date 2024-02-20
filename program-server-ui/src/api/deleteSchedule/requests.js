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
