import { NativeModules } from "react-native";

const { ReactNativeWindowsMsal } = NativeModules;

// Exposing Native methods directly:
const {
  getLoginToken = () => {},
  logoutUser = () => {},
} = ReactNativeWindowsMsal;

// Exporting only the functions so that incase used, no platform
// specific code is required.
// **RECOMMENDED TO USE**
export { getLoginToken, logoutUser };

// Exporting the module for backward compatability
// **OLD IMPLEMENTATION**
export default { ReactNativeWindowsMsal };
