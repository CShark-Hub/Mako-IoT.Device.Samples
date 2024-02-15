import { FunctionComponent } from "preact";
import Tooltip from "../Tooltip";
import useLocalize from "../../utils/useLocalize ";

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
  const localize = useLocalize();

  return (
    <div className="mb-3">
      <h3 className="mb-3">{localize('wifisection.header')}</h3>
      <div className="mb-3">
        <label htmlFor="ssid" className="form-label">
        {localize('wifisection.ssid.label')}<Tooltip text={localize('wifisection.ssid.tooltip')}/>
        </label>
        <input
          type="text"
          className="form-control"
          id="ssid"
          value={ssid}
          onChange={(e) => onSSIDChange(e.currentTarget.value)}
        />

        <label htmlFor="password" className="form-label">
        {localize('wifisection.password.label')}
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
