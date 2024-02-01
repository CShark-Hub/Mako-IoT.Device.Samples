import { h, FunctionComponent } from "preact";

interface BinNamesSectionProps {
  binNames: Record<string, string>;
  onBinNameChange: (color: string, value: string) => void;
}

const BinNamesSection: FunctionComponent<BinNamesSectionProps> = ({
  binNames,
  onBinNameChange,
}) => {
  return (
    <div className="mb-3">
      <h3 className="mb-3">Bin Names</h3>

      {Object.entries(binNames).map(([color, name]) => (
        <div key={color} className="mb-3">
          <label htmlFor={color} className="form-label">
            {color} Bin:
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
