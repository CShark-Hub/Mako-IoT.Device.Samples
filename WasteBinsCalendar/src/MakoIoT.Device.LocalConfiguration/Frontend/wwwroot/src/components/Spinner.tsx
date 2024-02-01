import { h, FunctionComponent } from 'preact';

const Spinner: FunctionComponent = () => (
  <div className="spinner-container">
    <div className="spinner-border text-primary" role="status">
      <span className="visually-hidden">Loading...</span>
    </div>
  </div>
);

export default Spinner;
