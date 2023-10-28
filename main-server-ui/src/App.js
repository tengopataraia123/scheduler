import React from "react";
import "./assets/css/reset.css";
import "./assets/css/main.css";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { AuthProvider } from "context/authContext";
import CustomRoutes from "routes/CustomRoutes";

function App() {
  return (
    <AuthProvider>
      <div className="main">
        <CustomRoutes />
        <ToastContainer />
      </div>
    </AuthProvider>
  );
}

export default App;
