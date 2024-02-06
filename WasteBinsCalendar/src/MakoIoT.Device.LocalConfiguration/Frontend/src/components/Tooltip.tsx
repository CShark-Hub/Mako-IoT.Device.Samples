import { FunctionComponent } from 'preact';
import infoIconPath from '../assets/info-square.svg';

interface TooltipProps {
  text: string; // Add a text prop to pass custom tooltip text
}

const Tooltip: FunctionComponent<TooltipProps> = ({ text }) => {
  return (
      <img src={infoIconPath} class="infotooltip" alt="More information" title={text} />
  );
};

export default Tooltip;
