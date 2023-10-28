import React, { createContext, useContext } from "react";
import { toast } from "react-toastify";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = React.useState(true);
  const token = localStorage.getItem("token");

  React.useEffect(() => {
    if (!token) {
      setIsAuthenticated(false);
    }
  }, [token]);

  const userLogin = (token) => {
    setIsAuthenticated(true);
    localStorage.setItem("token", token);
  };

  const userLogout = () => {
    setIsAuthenticated(false);
    localStorage.removeItem("token");
    toast.info("თქვენ გახვედით სისტემიდან");
  };

  return (
    <AuthContext.Provider
      value={{ isAuthenticated, userLogin, userLogout, setIsAuthenticated }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};
