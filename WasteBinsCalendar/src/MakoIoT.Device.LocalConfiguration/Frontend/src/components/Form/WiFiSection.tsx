import { FunctionComponent } from "preact";
import Tooltip from "../Tooltip";

interface WiFiSectionProps {
  ssid: string;
  password: string;
  onSSIDChange: (value: string) => void;
  onPasswordChange: (value: string) => void;
}

const WiFiSection: FunctionComponent<WiFiSectionProps> = ({
  ssid,
  password,
  onSSIDChange,
  onPasswordChange,
}) => {
  return (
    <div className="mb-3">
      <h3 className="mb-3">WiFi Settings</h3>
      <div className="mb-3">
        <label htmlFor="ssid" className="form-label">
          SSID:<Tooltip text="Your network name."/>
        </label>
        <input
          type="text"
          className="form-control"
          id="ssid"
          value={ssid}
          onChange={(e) => onSSIDChange(e.currentTarget.value)}
        />

        <label htmlFor="password" className="form-label">
          Password:
        </label>
        <input
          type="password"
          className="form-control"
          id="password"
          value={password}
          onChange={(e) => onPasswordChange(e.currentTarget.value)}
        />
      </div>
    </div>
  );
};

export default WiFiSection;
