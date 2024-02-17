import React, { useState, useEffect } from "react";
import {
  programs,
  putProgramActivate,
  putProgramBlock,
  putProgramDeactivate,
  putProgramUnblock,
} from "../../api";
import {
  Typography,
  TextField,
  InputAdornment,
  Box,
  Switch,
  FormControlLabel,
  TableContainer,
  Paper,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
  Link,
  Chip,
} from "@mui/material";
import {
  CheckCircle,
  OpenInNew,
  RemoveCircle,
  Search,
} from "@mui/icons-material";
import { toast } from "react-toastify";

export const Programs = () => {
  const [programList, setProgramList] = useState([]);
  const [filteredPrograms, setFilteredPrograms] = useState(programList);
  const [search, setSearch] = useState("");

  const isSearchResultsEmpty = !!search && filteredPrograms.length === 0;

  const fetchPrograms = async () => {
    try {
      const response = await programs();
      setProgramList(response.data);
    } catch (error) {
      console.error("Error fetching programs:", error);
    }
  };

  const programFilter = (event) => {
    const filteredPrograms = programList.filter((program) => {
      return program.name.toLowerCase().includes(search.toLowerCase());
    });
    setFilteredPrograms(filteredPrograms);
  };

  const activateProgram = (program) => {
    putProgramActivate(program)
      .then(() => {
        toast.success("პროგრამა გააქტიურდა");
        fetchPrograms();
      })
      .catch((error) => {
        console.log(error);
        toast.error("დაფიქსირდა შეცდომა");
      });
  };

  const deactivateProgram = (program) => {
    putProgramDeactivate(program)
      .then(() => {
        toast.success("პროგრამა გაუქმებულია");
        fetchPrograms();
      })
      .catch((error) => {
        console.log(error);
        toast.error("დაფიქსირდა შეცდომა");
      });
  };

  const blockProgram = (program) => {
    putProgramBlock(program)
      .then(() => {
        toast.success("პროგრამა დაბლოკილია");
        fetchPrograms();
      })
      .catch((error) => {
        console.log(error);
        toast.error("დაფიქსირდა შეცდომა");
      });
  };

  const unblockProgram = (program) => {
    putProgramUnblock(program)
      .then(() => {
        toast.success("პროგრამა გააქტიურდა");
        fetchPrograms();
      })
      .catch((error) => {
        console.log(error);
        toast.error("დაფიქსირდა შეცდომა");
      });
  };

  const handleProgramBlockChange = (program) => {
    if (!program.isBlocked) {
      blockProgram(program);
    } else {
      unblockProgram(program);
    }
  };

  const handleProgramActiveChange = (program) => {
    if (!program.isActive) {
      activateProgram(program);
    } else {
      deactivateProgram(program);
    }
  };

  useEffect(() => {
    programFilter();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [search]);

  useEffect(() => {
    fetchPrograms();
  }, []);

  return (
    <>
      <div className="flex flex-col gap-2">
        <Typography variant="h4" component="h1">
          პროგრამები
        </Typography>
        <TextField
          id="outlined-basic"
          label="ძიება"
          placeholder="შეიყვანეთ პროგრამის სახელი"
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
              პროგრამა ვერ მოიძებნა
            </Typography>
          </Box>
        ) : (
          <>
            <TableContainer component={Paper}>
              <Table sx={{ minWidth: 650 }} aria-label="table" size="small">
                <TableHead>
                  <TableRow>
                    <TableCell
                      sx={{
                        fontWeight: "bold",
                        whiteSpace: "nowrap",
                      }}
                      width="1%"
                      align="center"
                    ></TableCell>
                    <TableCell
                      sx={{ fontWeight: "bold", whiteSpace: "nowrap" }}
                      width="12%"
                    >
                      კოდი
                    </TableCell>
                    <TableCell
                      sx={{ fontWeight: "bold", whiteSpace: "nowrap" }}
                      width="auto"
                    >
                      პროგრამის სახელი
                    </TableCell>
                    <TableCell
                      sx={{ fontWeight: "bold", whiteSpace: "nowrap" }}
                      width="20%"
                    >
                      პროგრამის ლინკი
                    </TableCell>
                    <TableCell
                      sx={{ fontWeight: "bold", whiteSpace: "nowrap" }}
                      width="1%"
                    >
                      სტატუსი
                    </TableCell>
                    <TableCell
                      sx={{ fontWeight: "bold", whiteSpace: "nowrap" }}
                      width="1%"
                    >
                      აქტივაცია
                    </TableCell>
                    <TableCell
                      sx={{ fontWeight: "bold", whiteSpace: "nowrap" }}
                      width="1%"
                    >
                      დაბლოკვა
                    </TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {(filteredPrograms.length > 0
                    ? filteredPrograms
                    : programList
                  ).map((program) => (
                    <TableRow
                      key={program.id}
                      sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
                    >
                      <TableCell>
                        {program.isBlocked ? (
                          <RemoveCircle color="error" />
                        ) : (
                          <CheckCircle color="success" />
                        )}
                      </TableCell>
                      <TableCell component="th" scope="row">
                        {program.code}
                      </TableCell>
                      <TableCell>{program.name}</TableCell>
                      <TableCell sx={{ whiteSpace: "nowrap" }}>
                        <Link href={program.url} target="_blank">
                          {program.url.slice(0, 40)}...{" "}
                          <OpenInNew fontSize="10" />
                        </Link>
                      </TableCell>

                      <TableCell>
                        {program.isActive ? (
                          <Chip color="success" size="small" label="აქტიური" />
                        ) : (
                          <Chip color="error" size="small" label="არააქტიური" />
                        )}
                      </TableCell>

                      <TableCell align="right">
                        <FormControlLabel
                          control={
                            <Switch
                              checked={program.isActive}
                              disabled={program.isBlocked}
                              onChange={() =>
                                handleProgramActiveChange(program)
                              }
                              name="isActive"
                              color="success"
                            />
                          }
                        />
                      </TableCell>
                      <TableCell align="center">
                        <FormControlLabel
                          control={
                            <Switch
                              checked={program.isBlocked}
                              color="error"
                              onChange={() => handleProgramBlockChange(program)}
                              name="isBlocked"
                            />
                          }
                        />
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          </>
        )}
      </div>
    </>
  );
};
