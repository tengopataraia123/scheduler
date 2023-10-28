import { Login } from "pages/Login/Login";
import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Protected from "helpers/ProtectedRoute";
import { Responses } from "pages/Responses/Responses";
import { Register } from "pages/Register/Register";
import Home from "pages/Home/Home";
import SiteWrapper from "components/Wrapper/Wrapper";

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
        <Route path="/register" element={<Register />} />
      </Routes>
    </BrowserRouter>
  );
};

export default CustomRoutes;
