import React from "react";
// import { ReactComponent as SchedulerLogo } from "../../assets/icons/Scheduler.svg";
import {
  ListItemText,
  ListItemIcon,
  ListItemButton,
  ListItem,
  List,
  Divider,
} from "@mui/material";
import QuizIcon from "@mui/icons-material/Quiz";
import LogoutIcon from "@mui/icons-material/Logout";
import { useAuth } from "context/authContext";

const SidebarFooter = () => {
  const open = true;
  const { userLogout } = useAuth();
  return (
    <>
      <List sx={{ marginTop: "auto" }}>
        <Divider />
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
              <QuizIcon />
            </ListItemIcon>
            <ListItemText primary="დახმარება" sx={{ opacity: open ? 1 : 0 }} />
          </ListItemButton>
        </ListItem>

        <ListItem disablePadding sx={{ display: "block" }}>
          <ListItemButton
            onClick={() => userLogout()}
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
              <LogoutIcon />
            </ListItemIcon>
            <ListItemText primary="გასვლა" sx={{ opacity: open ? 1 : 0 }} />
          </ListItemButton>
        </ListItem>
      </List>
    </>
  );
};

export default SidebarFooter;
