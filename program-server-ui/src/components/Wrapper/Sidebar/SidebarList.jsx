import React from "react";
import {
  ListItemText,
  ListItemIcon,
  ListItemButton,
  ListItem,
  List,
} from "@mui/material";
import ManageAccountsIcon from "@mui/icons-material/ManageAccounts";
import { useLocation, useNavigate } from "react-router-dom";
import { FormatListBulleted, Poll, TableChart } from "@mui/icons-material";

const SidebarList = () => {
  const { pathname } = useLocation();
  const navigate = useNavigate();
  const open = true;
  return (
    <List>
      <ListItem disablePadding sx={{ display: "block" }}>
        <ListItemButton
          onClick={() => navigate("/schedule")}
          selected={pathname === "/schedule"}
          sx={{
            minHeight: 48,
            justifyContent: open ? "initial" : "center",
            px: 2.5,
          }}
        >
          <ListItemIcon
            sx={{
              minWidth: 0,
              mr: open ? 3 : "auto",
              justifyContent: "center",
            }}
          >
            <TableChart />
          </ListItemIcon>
          <ListItemText
            primary="ცხრილის შექმნა"
            sx={{ opacity: open ? 1 : 0 }}
          />
        </ListItemButton>
      </ListItem>
      <ListItem disablePadding sx={{ display: "block" }}>
        <ListItemButton
          selected={pathname === "/"}
          onClick={() => navigate("/")}
          sx={{
            minHeight: 48,
            justifyContent: open ? "initial" : "center",
            px: 2.5,
          }}
        >
          <ListItemIcon
            sx={{
              minWidth: 0,
              mr: open ? 3 : "auto",
              justifyContent: "center",
            }}
          >
            <Poll />
          </ListItemIcon>
          <ListItemText primary="კითხვარი" sx={{ opacity: open ? 1 : 0 }} />
        </ListItemButton>
      </ListItem>

      <ListItem disablePadding sx={{ display: "block" }}>
        <ListItemButton
          onClick={() => navigate("/responses")}
          selected={pathname === "/responses"}
          sx={{
            minHeight: 48,
            justifyContent: open ? "initial" : "center",
            px: 2.5,
          }}
        >
          <ListItemIcon
            sx={{
              minWidth: 0,
              mr: open ? 3 : "auto",
              justifyContent: "center",
            }}
          >
            <FormatListBulleted />
          </ListItemIcon>
          <ListItemText primary="შედეგები" sx={{ opacity: open ? 1 : 0 }} />
        </ListItemButton>
      </ListItem>

      <ListItem disablePadding sx={{ display: "block" }}>
        <ListItemButton
          sx={{
            minHeight: 48,
            justifyContent: open ? "initial" : "center",
            px: 2.5,
          }}
        >
          <ListItemIcon
            sx={{
              minWidth: 0,
              mr: open ? 3 : "auto",
              justifyContent: "center",
            }}
          >
            <ManageAccountsIcon />
          </ListItemIcon>
          <ListItemText
            primary="ჩემი ანგარიში"
            sx={{ opacity: open ? 1 : 0 }}
          />
        </ListItemButton>
      </ListItem>
    </List>
  );
};

export default SidebarList;
