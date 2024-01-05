import { Stack } from "@mui/material";
import React from "react";
import HiddenSidebar from "../LeftSidebar/HiddenSidebar";

const NoChatSelected = () => {
  return (
    <Stack alignItems={"baseline"}>
      <HiddenSidebar />
      <Stack
        alignItems={"center"}
        justifyContent={"center"}
        height={"80vh"}
        width={"100%"}
      >
        <img
          alt=""
          src=""
          style={{ width: "25%", objectFit: "contain", opacity: "50%" }}
        />
      </Stack>
    </Stack>
  );
};

export default NoChatSelected;
