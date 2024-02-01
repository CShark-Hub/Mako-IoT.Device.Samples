import { h, FunctionComponent } from "preact";
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
        HTTPS Certificate
      </label>
      <input
        className="form-control"
        type="file"
        id="httpsCertificateUpload"
        onChange={handleFileChange}
      />
      <button type="button" className="btn btn-primary mt-2" onClick={handleUploadClick}>
        Upload
      </button>
    </div>
  );
};

export default HttpsCertificateUpload;
