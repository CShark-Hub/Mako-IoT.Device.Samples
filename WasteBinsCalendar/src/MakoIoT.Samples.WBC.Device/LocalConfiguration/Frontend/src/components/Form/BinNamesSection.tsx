import { FunctionComponent } from "preact";
import Tooltip from "../Tooltip";

interface BinNamesSectionProps {
  binNames: Record<string, string>;
  onBinNameChange: (color: string, value: string) => void;
}

const BinNamesSection: FunctionComponent<BinNamesSectionProps> = ({
  binNames,
  onBinNameChange,
}) => {
  const capitalizeFirstLetter = (string) => {
    return string.charAt(0).toUpperCase() + string.slice(1);
  };
  
  return (
    <div className="mb-3">
      <h3 className="mb-3">Bin Names<Tooltip text='Here you enter the text which identifies each bin in the calendar. For example if you enter "glass" under green, green light will show on the days with "glass" text in the calendar.'/></h3>

      {Object.entries(binNames).map(([color, name]) => (
        <div key={color} className="mb-3">
          <label htmlFor={color} className="form-label">
            {capitalizeFirstLetter(color)}:
          </label>
          <input
            type="text"
            className="form-control"
            id={color}
            value={name}
            onChange={(e) => onBinNameChange(color, (e.target as HTMLInputElement).value)} // Type assertion here
          />
        </div>
      ))}
    </div>
  );
};

export default BinNamesSection;
