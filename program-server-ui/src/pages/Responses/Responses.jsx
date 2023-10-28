import React, { useState, useEffect } from "react";
import { responses, questions } from "../../api";
import { Chip } from "@mui/material";

import {
  Typography,
  TextField,
  InputAdornment,
  Box,
  TableContainer,
  Paper,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
} from "@mui/material";
import { Search } from "@mui/icons-material";

export const Responses = () => {
  const [responseList, setResponseList] = useState([]);
  const [filteredResponses, setFilteredResponses] = useState(responseList);
  const [search, setSearch] = useState("");
  const [questionList, setQuestionList] = useState([]);

  const isSearchResultsEmpty = !!search && filteredResponses.length === 0;

  const fetchResponses = async () => {
    try {
      const response = await responses();
      setResponseList(response.data);
    } catch (error) {
      console.error("Error fetching answers:", error);
    }
  };
  const fetchQuestions = async () => {
    try {
      const question = await questions();
      setQuestionList(question.data);
    } catch (error) {
      console.error("Error fetching answers:", error);
    }
  };
  const surveyFilter = (event) => {
    const filteredResponses = responseList.filter((response) => {
      return response.subject.toLowerCase().includes(search.toLowerCase());
    });
    setFilteredResponses(filteredResponses);
  };

  useEffect(() => {
    surveyFilter();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [search]);

  useEffect(() => {
    fetchResponses();
  }, []);

  useEffect(() => {
    fetchQuestions();
  }, []);

  const getChoiceDetails = (choice) => {
    switch (choice) {
      case 0:
        return { label: "Strongly Disagree", color: "red", textColor: "white" };
      case 1:
        return { label: "Disagree", color: "orange", textColor: "black" };
      case 2:
        return { label: "Neutral", color: "grey", textColor: "white" };
      case 3:
        return { label: "Agree", color: "lightgreen", textColor: "black" };
      case 4:
        return { label: "Strongly Agree", color: "green", textColor: "white" };
      default:
        return { label: "Unknown", color: "grey", textColor: "white" };
    }
  };
  return (
    <>
      <div className="flex flex-col gap-2">
        <Typography variant="h4" component="h1">
          გამოკითხვის შედეგები
        </Typography>
        <TextField
          id="outlined-basic"
          label="ძიება"
          placeholder="შეიყვანეთ საგნის სახელი"
          variant="outlined"
          value={search}
          onChange={(event) => setSearch(event.target.value)}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <Search />
              </InputAdornment>
            ),
          }}
        />
        {isSearchResultsEmpty ? (
          <Box display={"flex"} justifyContent={"center"} mt={5}>
            <Typography variant="h5" component="h1">
              საგანი ვერ მოიძებნა
            </Typography>
          </Box>
        ) : (
          <>
            <TableContainer component={Paper}>
              <Table sx={{ minWidth: 650 }} aria-label="table" size="small">
                <TableHead>
                  <TableRow>
                    <TableCell
                      sx={{ fontWeight: "bold", whiteSpace: "nowrap" }}
                      width="10%"
                    >
                      საგნის სახელი
                    </TableCell>
                    <TableCell
                      sx={{ fontWeight: "bold", whiteSpace: "nowrap" }}
                      width="10%"
                    >
                      სტუდენტის მეილი
                    </TableCell>
                    <TableCell
                      sx={{ fontWeight: "bold", whiteSpace: "nowrap" }}
                      width="auto"
                    >
                      შეკითხვა
                    </TableCell>
                    <TableCell
                      sx={{ fontWeight: "bold", whiteSpace: "nowrap" }}
                      width="10%"
                    >
                      პასუხი
                    </TableCell>
                    <TableCell
                      sx={{ fontWeight: "bold", whiteSpace: "nowrap" }}
                      width="10%"
                    >
                      თარიღი
                    </TableCell>
                  </TableRow>
                </TableHead>

                <TableBody>
                  {(filteredResponses.length > 0
                    ? filteredResponses
                    : responseList
                  ).map((response) => {
                    const survey = questionList.find(
                      (q) => q.id === response.surveyId
                    );
                    const choiceDetails = getChoiceDetails(response.choice);
                    return (
                      <TableRow
                        key={response.id}
                        sx={{
                          "&:last-child td, &:last-child th": { border: 0 },
                        }}
                      >
                        <TableCell component="th" scope="row">
                          {response.subject}
                        </TableCell>
                        <TableCell>{response.userMail}</TableCell>
                        <TableCell>
                          {survey ? survey.question : "კითხვა არ მოიძებნა"}
                        </TableCell>
                        <TableCell>
                          <Chip
                            label={choiceDetails.label}
                            style={{
                              backgroundColor: choiceDetails.color,
                              color: choiceDetails.textColor,
                            }}
                          />
                        </TableCell>
                        <TableCell>{response.timestamp}</TableCell>
                      </TableRow>
                    );
                  })}
                </TableBody>
              </Table>
            </TableContainer>
          </>
        )}
      </div>
    </>
  );
};
