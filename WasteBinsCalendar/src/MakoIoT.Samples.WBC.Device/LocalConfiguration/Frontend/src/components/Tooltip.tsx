import { FunctionComponent } from 'preact';
import infoIconPath from '../assets/info-square.svg';

interface TooltipProps {
  text: string;
}

const Tooltip: FunctionComponent<TooltipProps> = ({ text }) => {
  return (
      <img src={infoIconPath} class="infotooltip" alt="More information" title={text} />
  );
};

export default Tooltip;
