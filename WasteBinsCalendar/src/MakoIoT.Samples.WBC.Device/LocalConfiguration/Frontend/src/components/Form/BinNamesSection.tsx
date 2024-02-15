import { FunctionComponent } from "preact";
import Tooltip from "../Tooltip";
import useLocalize from "../../utils/useLocalize ";

interface BinNamesSectionProps {
  binNames: Record<string, string>;
  onBinNameChange: (color: string, value: string) => void;
}

const BinNamesSection: FunctionComponent<BinNamesSectionProps> = ({
  binNames,
  onBinNameChange,
}) => {
  const localize = useLocalize();

  return (
    <div className="mb-3">
      <h3 className="mb-3">{localize('binnamessection.header')}<Tooltip text={localize('binnamessection.header.tooltip')}/></h3>

      {Object.entries(binNames).map(([color, name]) => (
        <div key={color} className="mb-3">
          <label htmlFor={color} className="form-label">
          {localize(`binnamessection.bin.${color}`)}
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
