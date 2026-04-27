import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        proxy: {
            '/backend': {
<<<<<<< HEAD
                target: 'http://localhost:5186',
=======
                target: 'http://localhost:5000',
>>>>>>> 463f711bf444ae5d3bddb97621b049f9f2973bc0
                changeOrigin: true,
                rewrite: (path) => path.replace(/^\/backend/, ''),
            },
        },
    },
})