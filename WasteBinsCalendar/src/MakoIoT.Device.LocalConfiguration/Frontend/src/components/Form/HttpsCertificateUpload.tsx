import { FunctionComponent } from "preact";
import { useState } from "preact/hooks";

interface HttpsCertificateUploadProps {
  onUpload: (file: File) => void;
}

const HttpsCertificateUpload: FunctionComponent<
  HttpsCertificateUploadProps
> = ({ onUpload }) => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  const handleFileChange = (event: Event) => {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files[0]) {
      setSelectedFile(target.files[0]);
    } else {
      setSelectedFile(null); // Ensure file selection is reset if the user cancels the file dialog
    }
  };

  const handleUploadClick = () => {
    if (selectedFile) {
      onUpload(selectedFile);
    }
  };

  return (
    <div className="mb-3">
      <label htmlFor="httpsCertificateUpload" className="form-label">
        Certificate(s) file
      </label>
      <input
        className="form-control"
        type="file"
        id="httpsCertificateUpload"
        onChange={handleFileChange}
      />
      <button
        type="button"
        className="btn btn-primary mt-2"
        onClick={handleUploadClick}
        disabled={!selectedFile}
      >
        Upload
      </button>
    </div>
  );
};

export default HttpsCertificateUpload;
