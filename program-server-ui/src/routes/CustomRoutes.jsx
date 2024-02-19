import { Login } from "pages/Login/Login";
import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Protected from "helpers/ProtectedRoute";
import { Responses } from "pages/Responses/Responses";

import Home from "pages/Home/Home";
import SiteWrapper from "components/Wrapper/Wrapper";
import CreateSchedule from "pages/CreateSchedule/CreateSchedule";
import Events from "pages/ScheduleEvent/Events";

const CustomRoutes = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route
          path="/"
          element={
            <Protected>
              <SiteWrapper>
                <Home />
              </SiteWrapper>
            </Protected>
          }
        />
        <Route
          path="/responses"
          element={
            <Protected>
              <SiteWrapper>
                <Responses />
              </SiteWrapper>
            </Protected>
          }
        />
        <Route
          path="/schedule"
          element={
            <Protected>
              <SiteWrapper>
                <CreateSchedule />
              </SiteWrapper>
            </Protected>
          }
        />
        <Route
          path="/events"
          element={
            <Protected>
              <SiteWrapper>
                <Events />
              </SiteWrapper>
            </Protected>
          }
        />
      </Routes>
    </BrowserRouter>
  );
};

export default CustomRoutes;
