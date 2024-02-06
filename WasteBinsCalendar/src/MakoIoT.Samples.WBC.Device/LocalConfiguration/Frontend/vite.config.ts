import { defineConfig } from 'vite';
import preact from '@preact/preset-vite';

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [preact()],
	build: {
		rollupOptions: {
		  output: {
			// This places all files in the root of dist directory
			assetFileNames: '[name][extname]',
			chunkFileNames: '[name].js',
			entryFileNames: '[name].js',
		  },
		},
	  },
});
