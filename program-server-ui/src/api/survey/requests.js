import axios from "axios";

const apiUrl = process.env.REACT_APP_API_URL;

export const responses = () => {
  return axios.get(`${apiUrl}/Survey/SurveyResults`);
};
export const questions = () => {
  return axios.get(`${apiUrl}/Survey/GetAllQuestions`);
};

export const postSurveyCreate = (params) => {
  return axios.post(
    `${apiUrl}/Survey/Create`,
    {
      subject: params.subject,
      question: params.question,
    },
    {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    }
  );
};
