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
import ArrowDownwardIcon from "@mui/icons-material/ArrowDownward";
import ArrowUpwardIcon from "@mui/icons-material/ArrowUpward";
import DeleteIcon from "@mui/icons-material/Delete";
import { getSubjects, getEvents } from "api/getSchedule/requests";
import { deleteEvents } from "api/deleteSchedule/requests";
import SaveAltIcon from "@mui/icons-material/SaveAlt";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import { CircularProgress } from "@mui/material";
import * as XLSX from "xlsx";

const Events = () => {
  const [events, setEvents] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [sortDirection, setSortDirection] = useState({
    startDate: "asc",
    endDate: "asc",
  });
  const [selectedEvents, setSelectedEvents] = useState({});
  const [currentPage, setCurrentPage] = useState(1);
  const [eventsPerPage] = useState(15);
  const [isLoading, setIsLoading] = useState(false);

  const fetchData = useCallback(async () => {
    try {
      setIsLoading(true);
      const subjectsResponse = await getSubjects();
      const eventsResponse = await getEvents();

      const subjectsMap = subjectsResponse.data.reduce((acc, subject) => {
        acc[subject.subjectId] = subject;
        return acc;
      }, {});

      const enrichedEvents = eventsResponse.data.map((event) => {
        const subject = subjectsMap[event.subjectId];
        return {
          ...event,
          subjectName: subject ? subject.name : "ვერ მოიძებნა",
          subjectCode: subject ? subject.code : "N/A",
          subjectId: event.subjectId,
        };
      });

      setEvents(enrichedEvents);
    } catch (error) {
      console.error("Error encountered:", error);
    } finally {
      setIsLoading(false); // End loading
    }
  }, []);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  function formatUTCDate(dateString) {
    const date = new Date(dateString);
    const year = date.getUTCFullYear();
    const month = (date.getUTCMonth() + 1).toString().padStart(2, "0");
    const day = date.getUTCDate().toString().padStart(2, "0");
    const hours = date.getUTCHours().toString().padStart(2, "0");
    const minutes = date.getUTCMinutes().toString().padStart(2, "0");
    const seconds = date.getUTCSeconds().toString().padStart(2, "0");
    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
  }

  const handleSearchChange = (e) => {
    setSearchTerm(e.target.value);
    setCurrentPage(1);
    setSelectedEvents({});
  };

  const filteredEvents = events.filter(
    (event) =>
      event.subjectName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      event.subjectCode.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const sortEvents = (column) => {
    const direction = sortDirection[column] === "asc" ? 1 : -1;
    const sortedEvents = [...filteredEvents].sort(
      (a, b) => (new Date(a[column]) - new Date(b[column])) * direction
    );

    setEvents(sortedEvents);
    setSortDirection((prevState) => ({
      ...prevState,
      [column]: prevState[column] === "asc" ? "desc" : "asc",
    }));
  };

  const indexOfLastEvent = currentPage * eventsPerPage;
  const indexOfFirstEvent = indexOfLastEvent - eventsPerPage;
  const currentEvents = filteredEvents.slice(
    indexOfFirstEvent,
    indexOfLastEvent
  );

  const totalPages = Math.ceil(filteredEvents.length / eventsPerPage);

  const handlePreviousPage = () => {
    setCurrentPage(currentPage - 1);
  };

  const handleNextPage = () => {
    setCurrentPage(currentPage + 1);
  };

  const handleSelectAllClick = () => {
    const allFilteredEventIds = filteredEvents.map((e) => e.eventId.toString());
    const areAllFilteredSelected = allFilteredEventIds.every(
      (id) => selectedEvents[id]
    );

    if (areAllFilteredSelected) {
      const newSelectedEvents = { ...selectedEvents };
      allFilteredEventIds.forEach((id) => {
        delete newSelectedEvents[id];
      });
      setSelectedEvents(newSelectedEvents);
    } else {
      const newSelectedEvents = { ...selectedEvents };
      allFilteredEventIds.forEach((id) => {
        newSelectedEvents[id] = true;
      });
      setSelectedEvents(newSelectedEvents);
    }
  };

  const handleSelectClick = (event, eventId) => {
    const isSelected = !!selectedEvents[eventId];
    setSelectedEvents((prev) => ({
      ...prev,
      [eventId]: !isSelected,
    }));
  };

  const isSelected = (eventId) => !!selectedEvents[eventId];

  const handleDeleteSelected = async () => {
    const selectedEventIds = Object.keys(selectedEvents)
      .filter((key) => selectedEvents[key])
      .map((key) => parseInt(key));

    if (selectedEventIds.length === 0) {
      alert("ჩანაწერი არაა მონიშნული!");
      return;
    }

    try {
      await deleteEvents(selectedEventIds);
      alert("ჩანაწერები წაიშალა.");
      setSelectedEvents({});
      fetchData();
    } catch (error) {
      alert("ვერ მოხერხდა ჩანაწერების წაშლა");
    }
  };

  const exportToExcel = () => {
    const selectedEventIds = Object.keys(selectedEvents).filter(
      (key) => selectedEvents[key]
    );
    if (selectedEventIds.length === 0) {
      alert("ექსელში ეხპორტისთვის მონიშნეთ ჩანაწერი!");
      return;
    }

    const eventsToExport = events.filter((event) =>
      selectedEventIds.includes(event.eventId.toString())
    );

    if (eventsToExport.length === 0) {
      alert("ვერ მოიძებნა ჩანაწერი");
      return;
    }

    const worksheet = XLSX.utils.json_to_sheet(
      eventsToExport.map((event) => ({
        "#": event.eventId,
        "საგნის კოდი": event.subjectCode || "N/A",
        "საგნის სახელი": event.subjectName,
        "დაწყების დრო": event.startDate,
        "დასრულების დრო": event.endDate,
      }))
    );

    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Events");
    XLSX.writeFile(workbook, "განრიგი.xlsx");
  };

  return (
    <Paper sx={{ p: 2 }}>
      <Typography variant="h4" sx={{ mb: 4 }}>
        განრიგი
        <IconButton
          color="error"
          onClick={handleDeleteSelected}
          size="medium"
          sx={{
            ml: 3,
            transition: "box-shadow 0.3s ease-in-out",
            "&:hover": {
              boxShadow: "0px 4px 8px rgba(0, 0, 0, 0.2)",
            },
          }}
        >
          <DeleteIcon />
        </IconButton>
        <IconButton
          color="success"
          onClick={exportToExcel}
          size="medium"
          sx={{
            ml: 0.2,
            transition: "box-shadow 0.3s ease-in-out",
            "&:hover": {
              boxShadow: "0px 4px 8px rgba(0, 0, 0, 0.2)",
            },
          }}
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
            label="ძიება საგნის კოდით ან სახელით"
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
                        Object.keys(selectedEvents).length > 0 &&
                        Object.keys(selectedEvents).length <
                          currentEvents.length
                      }
                      checked={
                        currentEvents.length > 0 &&
                        Object.keys(selectedEvents).length ===
                          currentEvents.length
                      }
                      onChange={handleSelectAllClick}
                    />
                  </TableCell>
                  <TableCell>საგნის კოდი</TableCell>
                  <TableCell>საგნის სახელი</TableCell>
                  <TableCell>კვირის დღე</TableCell>
                  <TableCell
                    style={{ cursor: "pointer" }}
                    onClick={() => sortEvents("startDate")}
                  >
                    დაწყების დრო{" "}
                    {sortDirection.startDate === "asc" ? (
                      <ArrowUpwardIcon />
                    ) : (
                      <ArrowDownwardIcon />
                    )}
                  </TableCell>
                  <TableCell
                    style={{ cursor: "pointer" }}
                    onClick={() => sortEvents("endDate")}
                  >
                    დასრულების დრო{" "}
                    {sortDirection.endDate === "asc" ? (
                      <ArrowUpwardIcon />
                    ) : (
                      <ArrowDownwardIcon />
                    )}
                  </TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {currentEvents.map((event) => (
                  <TableRow
                    key={event.eventId}
                    selected={isSelected(event.eventId)}
                  >
                    <TableCell padding="checkbox">
                      <Checkbox
                        checked={isSelected(event.eventId)}
                        onChange={(e) => handleSelectClick(e, event.eventId)}
                      />
                    </TableCell>
                    <TableCell>{event.subjectCode}</TableCell>
                    <TableCell>{event.subjectName}</TableCell>
                    <TableCell>
                      {new Date(event.startDate).toLocaleDateString("en-US", {
                        weekday: "long",
                      })}
                    </TableCell>
                    <TableCell>{formatUTCDate(event.startDate)}</TableCell>
                    <TableCell>{formatUTCDate(event.endDate)}</TableCell>
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

export default Events;
