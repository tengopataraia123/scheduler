import React, { useEffect, useState, useCallback } from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Typography,
  Paper,
  Box,
  Button,
  Checkbox,
  IconButton,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import DeleteIcon from "@mui/icons-material/Delete";
import SaveAltIcon from "@mui/icons-material/SaveAlt";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import { getSubjects } from "api/getSchedule/requests";
import { deleteSubjects } from "api/deleteSchedule/requests";
import { CircularProgress } from "@mui/material";
import RemoveCircleOutlineIcon from "@mui/icons-material/RemoveCircleOutline";
import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline";

import * as XLSX from "xlsx";
import { toast } from "react-toastify";

const SchedulerSubjects = () => {
  const [subjects, setSubjects] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedSubjects, setSelectedSubjects] = useState({});
  const [currentPage, setCurrentPage] = useState(1);
  const [subjectsPerPage] = useState(15);
  const [isLoading, setIsLoading] = useState(false);

  const fetchData = useCallback(async () => {
    try {
      setIsLoading(true);
      const subjectsResponse = await getSubjects();
      setSubjects(subjectsResponse.data);
    } catch (error) {
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const handleSearchChange = (e) => {
    setSearchTerm(e.target.value);
    setCurrentPage(1);
    setSelectedSubjects({});
  };

  const filteredSubjects = subjects.filter(
    (subject) =>
      subject.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      subject.code.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const indexOfLastSubject = currentPage * subjectsPerPage;
  const indexOfFirstSubject = indexOfLastSubject - subjectsPerPage;
  const currentSubjects = filteredSubjects.slice(
    indexOfFirstSubject,
    indexOfLastSubject
  );

  const totalPages = Math.ceil(filteredSubjects.length / subjectsPerPage);

  const handlePreviousPage = () => {
    setCurrentPage(currentPage - 1);
  };

  const handleNextPage = () => {
    setCurrentPage(currentPage + 1);
  };

  const handleSelectAllClick = () => {
    const allFilteredSubjectIds = filteredSubjects.map((subject) =>
      subject.subjectId.toString()
    );
    const areAllFilteredSelected = allFilteredSubjectIds.every(
      (id) => selectedSubjects[id]
    );

    if (areAllFilteredSelected) {
      const newSelectedSubjects = { ...selectedSubjects };
      allFilteredSubjectIds.forEach((id) => {
        delete newSelectedSubjects[id];
      });
      setSelectedSubjects(newSelectedSubjects);
    } else {
      const newSelectedSubjects = {};
      allFilteredSubjectIds.forEach((id) => {
        newSelectedSubjects[id] = true;
      });
      setSelectedSubjects(newSelectedSubjects);
    }
  };

  const handleSelectClick = (subjectId) => {
    const isSelected = !!selectedSubjects[subjectId.toString()];
    setSelectedSubjects((prev) => ({
      ...prev,
      [subjectId.toString()]: !isSelected,
    }));
  };

  const isSelected = (subjectId) => !!selectedSubjects[subjectId.toString()];

  const handleDeleteSelected = async () => {
    const selectedSubjectIds = Object.keys(selectedSubjects)
      .filter((key) => selectedSubjects[key])
      .map((key) => parseInt(key));

    if (selectedSubjectIds.length > 0) {
      try {
        await deleteSubjects(selectedSubjectIds);
        alert("მონიშნული საგანი წაიშალა");
        setSelectedSubjects({});
        fetchData();
      } catch (error) {
        if (
          error.response &&
          error.response.data &&
          error.response.data.error
        ) {
          toast(`ვერ მოხერხდა საგნის წაშლა: ${error.response.data.error}`);
        } else {
          toast("ვერ მოხერხდა საგნის წაშლა. გთხოვთ, ცადეთ თავიდან.");
        }
      }
    } else {
      toast("საგანი არაა მონიშნული!");
    }
  };

  const exportToExcel = () => {
    const selectedSubjectIds = Object.keys(selectedSubjects).filter(
      (key) => selectedSubjects[key]
    );
    if (selectedSubjectIds.length === 0) {
      alert("მონიშნეთ საგანი ექსპორტისთვის");
      return;
    }

    // Corrected to match subjects based on subjectId
    const subjectsToExport = subjects.filter((subject) =>
      selectedSubjectIds.includes(subject.subjectId.toString())
    );

    const worksheet = XLSX.utils.json_to_sheet(
      subjectsToExport.map((subject) => ({
        "#": subject.subjectId,
        "საგნის სახელი": subject.name,
        "საგნის კოდი": subject.code,
        საფეხური: subject.description.step,
        სემესტრი: subject.description.semester,
        კრედიტი: subject.description.credit,
        სავალდებულო: subject.description.isMandatory ? "Yes" : "No",
        "დაწყების თარიღი": subject.description.startDate
          ? new Date(subject.description.startDate).toLocaleDateString()
          : "N/A",
        "დასრულების თარიღი": subject.description.endDate
          ? new Date(subject.description.endDate).toLocaleDateString()
          : "N/A",
        "ჩატარების ადგილი": subject.description.location.address,
      }))
    );

    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Subjects");
    XLSX.writeFile(workbook, "Subjects.xlsx");
  };

  return (
    <Paper sx={{ p: 2 }}>
      <Typography variant="h4" sx={{ mb: 4 }}>
        საგნების სია
        <IconButton
          color="error"
          size="medium"
          sx={{ ml: 3 }}
          onClick={handleDeleteSelected}
        >
          <DeleteIcon />
        </IconButton>
        <IconButton
          color="success"
          size="medium"
          sx={{ ml: 0.2 }}
          onClick={exportToExcel}
        >
          <SaveAltIcon />
        </IconButton>
      </Typography>
      {isLoading ? (
        <Box
          display="flex"
          justifyContent="center"
          alignItems="center"
          sx={{ height: "calc(100vh - 200px)" }}
        >
          <CircularProgress />
        </Box>
      ) : (
        <>
          <TextField
            fullWidth
            variant="outlined"
            label="ძიება სახელით ან კოდით"
            InputProps={{ endAdornment: <SearchIcon /> }}
            onChange={handleSearchChange}
            value={searchTerm}
            sx={{ mb: 3 }}
          />
          <TableContainer component={Paper}>
            <Table>
              <TableHead
                sx={{
                  "& .MuiTableCell-head": {
                    fontWeight: "bold",
                    backgroundColor: "rgba(0, 0, 0, 0.1)",
                  },
                }}
              >
                <TableRow>
                  <TableCell padding="checkbox">
                    <Checkbox
                      indeterminate={
                        Object.keys(selectedSubjects).length > 0 &&
                        Object.keys(selectedSubjects).length <
                          filteredSubjects.length
                      }
                      checked={
                        filteredSubjects.length > 0 &&
                        Object.keys(selectedSubjects).length ===
                          filteredSubjects.length
                      }
                      onChange={handleSelectAllClick}
                    />
                  </TableCell>
                  <TableCell>სახელი</TableCell>
                  <TableCell>კოდი</TableCell>
                  <TableCell>საფეხური</TableCell>
                  <TableCell>სემესტრი</TableCell>
                  <TableCell>კრედიტი</TableCell>
                  <TableCell>სავალდებულო</TableCell>
                  <TableCell>დაწყება</TableCell>
                  <TableCell>დასრულება</TableCell>
                  <TableCell>ჩატარების ადგილი</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {currentSubjects.map((subject) => (
                  <TableRow
                    key={subject.subjectId}
                    selected={isSelected(subject.subjectId)}
                  >
                    <TableCell padding="checkbox">
                      <Checkbox
                        checked={isSelected(subject.subjectId)}
                        onChange={() => handleSelectClick(subject.subjectId)}
                      />
                    </TableCell>
                    <TableCell>{subject.name ? subject.name : "N/A"}</TableCell>
                    <TableCell>{subject.code ? subject.code : "N/A"}</TableCell>
                    <TableCell>
                      {subject.description.step
                        ? subject.description.step
                        : "N/A"}
                    </TableCell>
                    <TableCell>
                      {subject.description.semester
                        ? subject.description.semester
                        : 0}
                    </TableCell>
                    <TableCell>
                      {subject.description.credit
                        ? subject.description.credit
                        : 0}
                    </TableCell>
                    <TableCell>
                      {subject.description.isMandatory !== undefined ? (
                        subject.description.isMandatory ? (
                          <CheckCircleOutlineIcon
                            checked={true}
                            style={{ color: "green" }}
                          />
                        ) : (
                          <RemoveCircleOutlineIcon style={{ color: "red" }} />
                        )
                      ) : (
                        "N/A"
                      )}
                    </TableCell>
                    <TableCell>
                      {subject.description.startDate
                        ? new Date(
                            subject.description.startDate
                          ).toLocaleDateString()
                        : "N/A"}
                    </TableCell>
                    <TableCell>
                      {subject.description.endDate
                        ? new Date(
                            subject.description.endDate
                          ).toLocaleDateString()
                        : "N/A"}
                    </TableCell>
                    <TableCell>
                      {subject.description.location.address
                        ? subject.description.location.address
                        : "N/A"}
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
          <Box
            display="flex"
            justifyContent="center"
            alignItems="center"
            marginTop={2}
          >
            <Button disabled={currentPage <= 1} onClick={handlePreviousPage}>
              <ArrowBackIcon />
            </Button>
            <Typography
              sx={{ marginX: 2 }}
            >{`${currentPage} / ${totalPages}`}</Typography>
            <Button
              disabled={currentPage >= totalPages}
              onClick={handleNextPage}
            >
              <ArrowForwardIcon />
            </Button>
          </Box>
        </>
      )}
    </Paper>
  );
};

export default SchedulerSubjects;
