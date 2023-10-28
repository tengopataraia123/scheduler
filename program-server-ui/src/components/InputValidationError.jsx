import React from "react";
import { ReactComponent as InfoIcon } from "../assets/icons/info-icon.svg";

const InputValidationError = (props) => {
  return (
    <p className="error-message">
      <InfoIcon width="20px" height="20px" /> {props.message}
    </p>
  );
};

export default InputValidationError;
