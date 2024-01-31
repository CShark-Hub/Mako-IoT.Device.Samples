import preactLogo from '../../assets/preact.svg';

export function Exit() {
	return (
		<div class="home">
			<a href="https://preactjs.com" target="_blank">
				<img src={preactLogo} alt="Preact logo" height="160" width="160" />
			</a>
			<div>The device will now reboot</div>
		</div>
	);
}

