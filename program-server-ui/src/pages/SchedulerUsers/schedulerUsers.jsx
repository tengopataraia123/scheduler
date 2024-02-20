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
import { getUsers } from "api/getSchedule/requests";
import { deleteUsers } from "api/deleteSchedule/requests";
import * as XLSX from "xlsx";

const SchedulerUsers = () => {
  const [users, setUsers] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedUsers, setSelectedUsers] = useState({});
  const [currentPage, setCurrentPage] = useState(1);
  const [usersPerPage] = useState(15);

  const fetchData = useCallback(async () => {
    try {
      const usersResponse = await getUsers();
      setUsers(usersResponse.data);
    } catch (error) {
      console.error("Error encountered:", error);
    }
  }, []);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const handleSearchChange = (e) => {
    setSearchTerm(e.target.value);
    setCurrentPage(1);
    setSelectedUsers({});
  };

  const filteredUsers = users.filter(
    (user) =>
      user.firstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.lastName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const indexOfLastUser = currentPage * usersPerPage;
  const indexOfFirstUser = indexOfLastUser - usersPerPage;
  const currentUsers = filteredUsers.slice(indexOfFirstUser, indexOfLastUser);

  const totalPages = Math.ceil(filteredUsers.length / usersPerPage);

  const handlePreviousPage = () => {
    setCurrentPage(currentPage - 1);
  };

  const handleNextPage = () => {
    setCurrentPage(currentPage + 1);
  };

  const handleSelectAllClick = () => {
    const allFilteredUserIds = filteredUsers.map((user) =>
      user.userId.toString()
    );
    const areAllFilteredSelected = allFilteredUserIds.every(
      (id) => selectedUsers[id]
    );

    if (areAllFilteredSelected) {
      const newSelectedUsers = { ...selectedUsers };
      allFilteredUserIds.forEach((id) => {
        delete newSelectedUsers[id];
      });
      setSelectedUsers(newSelectedUsers);
    } else {
      const newSelectedUsers = {};
      allFilteredUserIds.forEach((id) => {
        newSelectedUsers[id] = true;
      });
      setSelectedUsers(newSelectedUsers);
    }
  };

  const handleSelectClick = (userId) => {
    const isSelected = !!selectedUsers[userId.toString()];
    setSelectedUsers((prev) => ({
      ...prev,
      [userId.toString()]: !isSelected,
    }));
  };

  const isSelected = (userId) => !!selectedUsers[userId.toString()];

  const handleDeleteSelected = async () => {
    const selectedUserIds = Object.keys(selectedUsers)
      .filter((key) => selectedUsers[key])
      .map((key) => parseInt(key));

    if (selectedUserIds.length > 0) {
      try {
        await deleteUsers(selectedUserIds); // Implement this function in your API
        alert("Selected users deleted successfully.");
        setSelectedUsers({});
        fetchData();
      } catch (error) {
        console.error("Failed to delete users:", error);
        alert("Failed to delete selected users.");
      }
    } else {
      alert("No users selected.");
    }
  };

  const exportToExcel = () => {
    const selectedUserIds = Object.keys(selectedUsers).filter(
      (key) => selectedUsers[key]
    );
    if (selectedUserIds.length === 0) {
      alert("No users selected for export.");
      return;
    }

    const usersToExport = users.filter((user) =>
      selectedUserIds.includes(user.userId.toString())
    );

    const worksheet = XLSX.utils.json_to_sheet(
      usersToExport.map((user) => ({
        UserID: user.userId,
        FirstName: user.firstName,
        LastName: user.lastName,
        Email: user.email,
        Role:
          user.roleId === 1
            ? "Student"
            : user.roleId === 2
            ? "Lecturer"
            : "N/A",
      }))
    );

    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Users");
    XLSX.writeFile(workbook, "UsersExport.xlsx");
  };

  return (
    <Paper sx={{ p: 2 }}>
      <Typography variant="h4" sx={{ mb: 4 }}>
        Scheduler Users
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
      <TextField
        fullWidth
        variant="outlined"
        label="Search by Name or Email"
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
                    Object.keys(selectedUsers).length > 0 &&
                    Object.keys(selectedUsers).length < filteredUsers.length
                  }
                  checked={
                    filteredUsers.length > 0 &&
                    Object.keys(selectedUsers).length === filteredUsers.length
                  }
                  onChange={handleSelectAllClick}
                />
              </TableCell>
              <TableCell>First Name</TableCell>
              <TableCell>Last Name</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Role</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {currentUsers.map((user) => (
              <TableRow key={user.userId} selected={isSelected(user.userId)}>
                <TableCell padding="checkbox">
                  <Checkbox
                    checked={isSelected(user.userId)}
                    onChange={() => handleSelectClick(user.userId)}
                  />
                </TableCell>
                <TableCell>{user.firstName}</TableCell>
                <TableCell>{user.lastName}</TableCell>
                <TableCell>{user.email}</TableCell>
                <TableCell>
                  {user.roleId === 1
                    ? "Student"
                    : user.roleId === 2
                    ? "Lecturer"
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
        <Button disabled={currentPage >= totalPages} onClick={handleNextPage}>
          <ArrowForwardIcon />
        </Button>
      </Box>
    </Paper>
  );
};

export default SchedulerUsers;
