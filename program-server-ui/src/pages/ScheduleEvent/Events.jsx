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
import SaveAltIcon from "@mui/icons-material/SaveAlt"; // Import an icon for Excel export
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

  const fetchData = useCallback(async () => {
    try {
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
          subjectName: subject ? subject.name : "Unknown",
          subjectCode: subject ? subject.code : "N/A",
          subjectId: event.subjectId, // Include the subject ID
        };
      });

      setEvents(enrichedEvents);
    } catch (error) {
      console.error("Error encountered:", error);
    }
  }, []); // If fetchData depends on props or state, include them in this dependency array

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const handleSearchChange = (e) => {
    setSearchTerm(e.target.value);
    setCurrentPage(1); // Reset to first page on search change
    setSelectedEvents({}); // Clear selections when search criteria changes
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

  // Pagination logic
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
      // If all filtered are currently selected, deselect them
      const newSelectedEvents = { ...selectedEvents };
      allFilteredEventIds.forEach((id) => {
        delete newSelectedEvents[id];
      });
      setSelectedEvents(newSelectedEvents);
    } else {
      // If not all filtered are selected, select all filtered
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
    const selectedSubjectIds = Object.keys(selectedEvents)
      .filter((key) => selectedEvents[key])
      .map(
        (key) => events.find((event) => event.subjectCode === key)?.subjectId
      ) // Map subjectCode to subjectId
      .filter((id) => id !== undefined); // Ensure undefined IDs are removed

    if (selectedSubjectIds.length === 0) {
      alert("No events selected for deletion.");
      return;
    }

    try {
      await deleteEvents(selectedSubjectIds);
      alert("Selected events successfully deleted.");
      setSelectedEvents({});
      fetchData(); // Re-fetch the events
    } catch (error) {
      console.error("Error deleting events:", error);
      alert("Failed to delete selected events.");
    }
  };

  const exportToExcel = () => {
    const selectedEventIds = Object.keys(selectedEvents).filter(
      (key) => selectedEvents[key]
    );
    if (selectedEventIds.length === 0) {
      alert("Please select at least one event to export.");
      return;
    }

    const eventsToExport = events.filter((event) =>
      selectedEventIds.includes(event.eventId.toString())
    );

    if (eventsToExport.length === 0) {
      alert("No events found to export.");
      return;
    }

    const worksheet = XLSX.utils.json_to_sheet(
      eventsToExport.map((event) => ({
        "Event ID": event.eventId,
        "Subject Code": event.subjectCode || "N/A",
        "Subject Name": event.subjectName,
        "Start Time": event.startDate,
        "End Time": event.endDate,
      }))
    );

    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Events");
    XLSX.writeFile(workbook, "events_export.xlsx");
  };

  return (
    <Paper sx={{ p: 2 }}>
      <Typography variant="h4" sx={{ mb: 4 }}>
        Events
        <IconButton
          color="error"
          onClick={handleDeleteSelected}
          size="medium"
          sx={{
            ml: 3, // Adjust spacing as needed
            transition: "box-shadow 0.3s ease-in-out", // Smooth transition for shadow
            "&:hover": {
              boxShadow: "0px 4px 8px rgba(0, 0, 0, 0.2)", // Subtle grey shadow
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
            ml: 0.2, // Adjust spacing as needed
            transition: "box-shadow 0.3s ease-in-out", // Smooth transition for shadow
            "&:hover": {
              boxShadow: "0px 4px 8px rgba(0, 0, 0, 0.2)", // Subtle grey shadow on hover
            },
          }}
        >
          <SaveAltIcon /> {/* This icon represents Excel export */}
        </IconButton>
      </Typography>
      <TextField
        fullWidth
        variant="outlined"
        label="Search by Subject Name or Code"
        InputProps={{ endAdornment: <SearchIcon /> }}
        onChange={handleSearchChange}
        value={searchTerm}
        sx={{ mb: 3 }}
      />
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell padding="checkbox">
                <Checkbox
                  indeterminate={
                    Object.keys(selectedEvents).length > 0 &&
                    Object.keys(selectedEvents).length < currentEvents.length
                  }
                  checked={
                    currentEvents.length > 0 &&
                    Object.keys(selectedEvents).length === currentEvents.length
                  }
                  onChange={handleSelectAllClick}
                />
              </TableCell>
              <TableCell>Subject Code</TableCell>
              <TableCell>Subject Name</TableCell>
              <TableCell>Weekday</TableCell>
              <TableCell
                style={{ cursor: "pointer" }}
                onClick={() => sortEvents("startDate")}
              >
                Start Time{" "}
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
                End Time{" "}
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
                <TableCell>
                  {new Date(event.startDate).toLocaleString()}
                </TableCell>
                <TableCell>
                  {new Date(event.endDate).toLocaleString()}
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
          Previous
        </Button>
        <Typography
          sx={{ marginX: 2 }}
        >{`Page ${currentPage} of ${totalPages}`}</Typography>
        <Button disabled={currentPage >= totalPages} onClick={handleNextPage}>
          Next
        </Button>
      </Box>
    </Paper>
  );
};

export default Events;
