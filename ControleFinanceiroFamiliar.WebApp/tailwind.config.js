/** @type {import('talwindcss').Config}*/
module.exports = {
  darkMod: 'class',
  content: ['./src/**/*.(html,ts)'],
  theme: {
    extend: {
      colors: {
        bg: { light: '#FAFAFA', dark: '#0B0B0F' },
        surface: { light: '#FFFFFF', dark: '#15151A' },
        accent: { DEFAULT: '#4F46E5', dark: '#818CF8' },
        success: '#10B981',
        danger: '#EF4444',
      },
      fontFamily: { sans: ['Inter', 'system-ui', 'sans-serif'] }
    }
  },
  plugins: []
}
