import { FunctionComponent } from 'preact';

interface AlertMessageProps {
  type: 'success' | 'danger';
  message: string;
  onClose: () => void;
}

const AlertMessage: FunctionComponent<AlertMessageProps> = ({ type, message, onClose }) => {
  return (
    <div className={`alert alert-${type} d-flex justify-content-between align-items-center m-2`} role="alert">
      <span>{message}</span>
      <button type="button" className="btn-close" data-bs-dismiss="alert" aria-label="Close" onClick={onClose}></button>
    </div>
  );
};

export default AlertMessage;
