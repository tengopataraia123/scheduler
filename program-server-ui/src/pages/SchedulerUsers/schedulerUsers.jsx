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
import { CircularProgress } from "@mui/material";
import * as XLSX from "xlsx";

const SchedulerUsers = () => {
  const [users, setUsers] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedUsers, setSelectedUsers] = useState({});
  const [currentPage, setCurrentPage] = useState(1);
  const [usersPerPage] = useState(15);
  const [isLoading, setIsLoading] = useState(false);

  const fetchData = useCallback(async () => {
    try {
      setIsLoading(true);
      const usersResponse = await getUsers();
      setUsers(usersResponse.data);
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
        await deleteUsers(selectedUserIds);
        alert("მონიშნული მომხმარებელი წაიშალა");
        setSelectedUsers({});
        fetchData();
      } catch (error) {
        alert("ვერ მოხერხდა მომხარებლის წაშლა:");
      }
    } else {
      alert("მომხმარებელი არაა მონიშნული!");
    }
  };

  const exportToExcel = () => {
    const selectedUserIds = Object.keys(selectedUsers).filter(
      (key) => selectedUsers[key]
    );
    if (selectedUserIds.length === 0) {
      alert("მონიშნეთ მომხმარებელი ექსპორტისთვის");
      return;
    }

    const usersToExport = users.filter((user) =>
      selectedUserIds.includes(user.userId.toString())
    );

    const worksheet = XLSX.utils.json_to_sheet(
      usersToExport.map((user) => ({
        "#": user.userId,
        სახელი: user.firstName,
        გვარი: user.lastName,
        "ელ. ფოსტა": user.email,
        როლი:
          user.roleId === 1
            ? "სტუდენტი"
            : user.roleId === 2
            ? "ლექტორი"
            : "N/A",
      }))
    );

    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Users");
    XLSX.writeFile(workbook, "სტუდენტები და ლექტორები.xlsx");
  };

  return (
    <Paper sx={{ p: 2 }}>
      <Typography variant="h4" sx={{ mb: 4 }}>
        მომხმარებლების სია
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
            label="ძიება სახელით ან ელ. ფოსტით"
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
                        Object.keys(selectedUsers).length > 0 &&
                        Object.keys(selectedUsers).length < filteredUsers.length
                      }
                      checked={
                        filteredUsers.length > 0 &&
                        Object.keys(selectedUsers).length ===
                          filteredUsers.length
                      }
                      onChange={handleSelectAllClick}
                    />
                  </TableCell>
                  <TableCell>სახელი</TableCell>
                  <TableCell>გვარი</TableCell>
                  <TableCell>ელ. ფოსტა</TableCell>
                  <TableCell>როლი</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {currentUsers.map((user) => (
                  <TableRow
                    key={user.userId}
                    selected={isSelected(user.userId)}
                  >
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
                        ? "სტუდენტი"
                        : user.roleId === 2
                        ? "ლექტორი"
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

export default SchedulerUsers;
